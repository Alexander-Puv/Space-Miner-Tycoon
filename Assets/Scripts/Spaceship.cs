using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Spaceship;
using static UnityEngine.Rendering.DebugUI;

public class Spaceship : MonoBehaviour {
    public static Spaceship Instance { get; private set; }

    public enum DeliveryItems {
        Fuel,
        Durability
    }

    private class DeliveryData {
        public bool isPaidDelivery = false;
        public float deliveryProgress = 0f;
        public float value = 0f;
    }

    [System.Serializable]
    public class ShipUpgrades {
        public float fuelCapacityMultiplier = 1f;
        public float fuelEfficiencyMultiplier = 1f;
        public float durabilityMultiplier = 1f;
        public float miningSpeedMultiplier = 1f;
        public float travelSpeedMultiplier = 1f;
    }

    [Header("Base Stats")]
    public float baseMiningSpeed = 1f;
    public float baseMaxFuel = 1000f;
    public float baseMaxDurability = 1000f;
    public float baseFuelConsumption = 1f;
    public float baseDurabilityConsumption = 0.1f;
    public float baseTravelSpeed = 1f;

    [Header("Current Stats")]
    public float fuel;
    public float durability;
    private float currentMaxFuel;
    private float currentMaxDurability;
    private float currentMiningSpeed;
    private float currentTravelSpeed;

    [SerializeField] private ShipUpgrades upgrades = new();

    private Location currentLocation;
    private Location targetLocation;
    private bool isTraveling = false;
    private float travelProgress = 0f;
    private Dictionary<DeliveryItems, DeliveryData> deliveryItems = new();

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        UpdateShipStats();
        fuel = currentMaxFuel;
        durability = currentMaxDurability;
    }

    private void Update() {
        if (deliveryItems.Count != 0) {
            Delivery();
            if (fuel <= 0 || durability <= 0) return;
        }

        if (fuel <= 0) {
            OrderSupplies(DeliveryItems.Fuel, PlanetEconomy.BASE_FUEL_PRICE * currentMaxFuel / 2);
            return;
        } else if (durability <= 0) {
            OrderSupplies(DeliveryItems.Durability);
            return;
        }

        if (currentLocation is Asteroid asteroid && targetLocation == null
            && Inventory.Instance.currentCapacity < Inventory.Instance.maxCapacity) {
            asteroid.MineResource(currentMiningSpeed * Time.deltaTime);
            fuel -= baseFuelConsumption * upgrades.fuelEfficiencyMultiplier * Time.deltaTime;
            durability -= baseDurabilityConsumption * Time.deltaTime;
        } else if (isTraveling) {
            TravelToLocation();
        }
    }

    public void OnAsteroidClick(Asteroid asteroid) {
        asteroid.MineResource(currentMiningSpeed * 0.5f);
        durability -= baseDurabilityConsumption * 3f;

        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        BonusManager.Instance.TryDropBonus(worldPosition);
    }

    public void UpdateLocation(Location currentLocation) {
        this.currentLocation = currentLocation;
    }


    public void UpgradeFuelCapacity(float multiplierIncrease) {
        upgrades.fuelCapacityMultiplier += multiplierIncrease;
        UpdateShipStats();
        fuel = Mathf.Min(fuel, currentMaxFuel);
    }

    public void UpgradeFuelEfficiency(float multiplierDecrease) {
        upgrades.fuelEfficiencyMultiplier -= multiplierDecrease;
        upgrades.fuelEfficiencyMultiplier = Mathf.Max(0.1f, upgrades.fuelEfficiencyMultiplier);
    }

    public void UpgradeDurability(float multiplierIncrease) {
        upgrades.durabilityMultiplier += multiplierIncrease;
        UpdateShipStats();
        durability = Mathf.Min(durability, currentMaxDurability);
    }

    public void UpgradeMiningSpeed(float multiplierIncrease) {
        upgrades.miningSpeedMultiplier += multiplierIncrease;
        UpdateShipStats();
    }

    public void UpgradeTravelSpeed(float multiplierIncrease) {
        upgrades.travelSpeedMultiplier += multiplierIncrease;
        UpdateShipStats();
    }

    private void UpdateShipStats() {
        currentMaxFuel = baseMaxFuel * upgrades.fuelCapacityMultiplier;
        currentMaxDurability = baseMaxDurability * upgrades.durabilityMultiplier;
        currentMiningSpeed = baseMiningSpeed * upgrades.miningSpeedMultiplier;
        currentTravelSpeed = baseTravelSpeed * upgrades.travelSpeedMultiplier;
    }

    public float GetCurrentMaxFuel() => currentMaxFuel;
    public float GetCurrentMaxDurability() => currentMaxDurability;
    public float GetCurrentMiningSpeed() => currentMiningSpeed;


    public void TravelTo(Location location) {
        if (isTraveling) {
            Debug.LogWarning("Already traveling!");
            return;
        }

        if (fuel <= 0) {
            Debug.LogWarning("Not enough fuel to travel!");
            return;
        }

        if (currentLocation != null) {
            Destroy(currentLocation.gameObject);
        }

        targetLocation = location;
        isTraveling = true;
        travelProgress = 0f;
    }

    private void TravelToLocation() {
        float travelTime = 10f;
        travelProgress += Time.deltaTime / travelTime * 100f * currentTravelSpeed;

        fuel -= baseFuelConsumption * upgrades.fuelEfficiencyMultiplier * Time.deltaTime;

        if (travelProgress >= 100f) {
            isTraveling = false;
            var location = Instantiate(targetLocation, Vector3.zero, Quaternion.identity);
            GameManager.Instance.NewLocation(location);
            targetLocation = null;
            Debug.Log("Arrived at destination!");;
        }
    }


    private void OrderSupplies(DeliveryItems supply, float value = 0f) {
        if (supply == DeliveryItems.Fuel) {
            deliveryItems.Add(supply, new());
            deliveryItems[DeliveryItems.Fuel].value = value != 0f ? value : currentMaxFuel / 2;
            deliveryItems[DeliveryItems.Fuel].isPaidDelivery = Inventory.Instance.SpendMoney(
                PlanetEconomy.BASE_FUEL_PRICE * deliveryItems[DeliveryItems.Fuel].value);
        } else if (supply == DeliveryItems.Durability) {
            deliveryItems.Add(supply, new());
            deliveryItems[DeliveryItems.Fuel].isPaidDelivery = Inventory.Instance.SpendMoney(
                PlanetEconomy.BASE_REPAIR_PRICE * (currentMaxDurability - durability) / currentMaxDurability);
            deliveryItems[DeliveryItems.Durability].value = currentMaxDurability;
        } else {
            Debug.LogError("Incorrect supply value");
            return;
        }
    }

    private void Delivery() {
        List<DeliveryItems> removeKey = new();
        foreach (var item in deliveryItems) {
            float travelTime = 10f;
            float deliveryTravelSpeed = 1f;
            if (item.Value.isPaidDelivery) {
                deliveryTravelSpeed = 2f;
            }
            item.Value.deliveryProgress += Time.deltaTime / travelTime * 100f * deliveryTravelSpeed;

            if (item.Value.deliveryProgress >= 100f) {
                if (item.Key == DeliveryItems.Fuel) {
                    fuel += item.Value.value;
                } else if (item.Key == DeliveryItems.Durability) {
                    durability = item.Value.value;
                }
                removeKey.Add(item.Key);
                Debug.Log("Delivery is here!");
            }
        }

        foreach (var key in removeKey) {
            deliveryItems.Remove(key);
        }
    }
}