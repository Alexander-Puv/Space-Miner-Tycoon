using UnityEngine;
using UnityEngine.InputSystem;

public class Spaceship : MonoBehaviour {
    public static Spaceship Instance { get; private set; }

    [System.Serializable]
    public class ShipUpgrades {
        public float fuelCapacityMultiplier = 1f;
        public float fuelEfficiencyMultiplier = 1f;
        public float durabilityMultiplier = 1f;
        public float miningSpeedMultiplier = 1f;
    }

    [Header("Base Stats")]
    public float baseMiningSpeed = 1f;
    public float baseMaxFuel = 1000f;
    public float baseMaxDurability = 1000f;
    public float baseFuelConsumption = 1f;
    public float baseDurabilityConsumption = 0.1f;

    [Header("Current Stats")]
    public float fuel;
    public float durability;
    private float currentMaxFuel;
    private float currentMaxDurability;
    private float currentMiningSpeed;

    [SerializeField]
    private ShipUpgrades upgrades = new();

    private Location currentLocation;
    private InputActions inputActions;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        inputActions = new InputActions();
        inputActions.Touch.Enable();

        UpdateShipStats();
        fuel = currentMaxFuel;
        durability = currentMaxDurability;
    }

    private void Update() {
        if (currentLocation is Asteroid asteroid && fuel > 0 && durability > 0
            && Inventory.Instance.currentCapacity < Inventory.Instance.maxCapacity) {
            asteroid.MineResource(currentMiningSpeed * Time.deltaTime);
            fuel -= baseFuelConsumption * upgrades.fuelEfficiencyMultiplier * Time.deltaTime;
            durability -= baseDurabilityConsumption * Time.deltaTime;

            if (inputActions.Touch.ClickOnAsteroid.WasPerformedThisFrame()) {
                asteroid.MineResource(currentMiningSpeed * 0.5f);
                durability -= baseDurabilityConsumption * 3f;

                Vector2 screenPosition = Mouse.current.position.ReadValue();
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
                BonusManager.Instance.TryDropBonus(worldPosition);
            }
        }
    }

    public void UpdateLocation() {
        currentLocation = GameManager.Instance.GetLocation();
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

    private void UpdateShipStats() {
        currentMaxFuel = baseMaxFuel * upgrades.fuelCapacityMultiplier;
        currentMaxDurability = baseMaxDurability * upgrades.durabilityMultiplier;
        currentMiningSpeed = baseMiningSpeed * upgrades.miningSpeedMultiplier;
    }

    public float GetCurrentMaxFuel() => currentMaxFuel;
    public float GetCurrentMaxDurability() => currentMaxDurability;
    public float GetCurrentMiningSpeed() => currentMiningSpeed;
}