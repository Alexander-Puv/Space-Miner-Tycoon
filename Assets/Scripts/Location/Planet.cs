using UnityEngine;

public class Planet : Location {
    public GameManager.Rarity rarity;

    private PlanetModel planetModel;
    private PlanetModel.PlanetType planetType;

    private void Start() {
        planetModel = GetComponentInChildren<PlanetModel>();
        planetType = planetModel.GetRandomPlanetType();
        rarity = planetModel.GetRarity();

        PlanetEconomy.Instance.GenerateBasePrices(planetType);
        PlanetEconomy.Instance.ApplyEventModifiers();
        //if (Random.Range(0f, 1f) < .4f) {
            PlanetEconomy.Instance.SetRandomEvent();
        //}
    }

    public void RepairSpaceship() {
        float cost = PlanetEconomy.Instance.GetServicePrice(PlanetEconomy.ServiceType.Repair);

        if (Inventory.Instance.SpendMoney(cost)) {
            Spaceship.Instance.durability = Spaceship.Instance.GetCurrentMaxDurability();
            Debug.Log($"Spaceship repaired for {cost} credits!");
        } else {
            Debug.Log("Not enough money for repair!");
        }
    }

    public void RefuelSpaceship() {
        float cost = PlanetEconomy.Instance.GetServicePrice(PlanetEconomy.ServiceType.Fuel) * (Spaceship.Instance.GetCurrentMaxFuel() - Spaceship.Instance.fuel);

        if (Inventory.Instance.SpendMoney(cost)) {
            Spaceship.Instance.fuel = Spaceship.Instance.GetCurrentMaxFuel();
            Debug.Log($"Spaceship refueled for {cost} credits!");
        } else {
            Debug.Log("Not enough money for fuel!");
        }
    }

    public void BuyUpgrade() {
        float cost = PlanetEconomy.Instance.GetServicePrice(PlanetEconomy.ServiceType.Upgrade);

        if (Inventory.Instance.SpendMoney(cost)) {
            Spaceship.Instance.UpgradeTravelSpeed(0.1f);
            Debug.Log($"Upgrade purchased for {cost} credits!");
        } else {
            Debug.Log("Not enough money for upgrade!");
        }
    }

    public void SellResource(Resource resource, int amount) {
        float price = PlanetEconomy.Instance.GetResourcePrice(resource) * amount;
        Inventory.Instance.RemoveResources(resource, amount);
        Inventory.Instance.EarnMoney(price);
        Debug.Log($"Sold {amount} {resource.resourceName} for {price} credits!");
    }
}

