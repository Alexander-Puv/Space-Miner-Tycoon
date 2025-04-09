using System.Collections.Generic;
using UnityEngine;

public class Planet : Location {
    public GameManager.Rarity rarity;

    private PlanetModel planetModel;
    private PlanetModel.PlanetType planetType;
    private List<CrewManager.CrewMember> availableCrew;

    private void Start() {
        planetModel = GetComponentInChildren<PlanetModel>();
        planetType = planetModel.GetRandomPlanetType();
        rarity = planetModel.GetRarity();

        PlanetEconomy.Instance.GenerateBasePrices(planetType);
        PlanetEconomy.Instance.ApplyEventModifiers();
        if (Random.Range(0f, 1f) < .4f) {
            PlanetEconomy.Instance.SetRandomEvent();
        }

        GenerateAvailableCrew();
    }

    private void GenerateAvailableCrew() {
        availableCrew = new List<CrewManager.CrewMember>();
        int crewCount = Random.Range(1, 4);
        for (int i = 0; i < crewCount; i++) {
            string name = $"CrewMember{i + 1}";
            CrewManager.CrewType type = (CrewManager.CrewType)Random.Range(0, 3);
            GameManager.Rarity crewRarity = GetRandomCrewRarity();
            availableCrew.Add(new CrewManager.CrewMember(name, type, crewRarity));
        }
    }

    private GameManager.Rarity GetRandomCrewRarity() {
        float chance = Random.value;
        return rarity switch {
            GameManager.Rarity.Common => chance < 0.8f ? GameManager.Rarity.Common : GameManager.Rarity.Uncommon,
            GameManager.Rarity.Uncommon => chance < 0.6f ? GameManager.Rarity.Common : chance < 0.9f ? GameManager.Rarity.Uncommon : GameManager.Rarity.Rare,
            GameManager.Rarity.Rare => chance < 0.4f ? GameManager.Rarity.Uncommon : chance < 0.8f ? GameManager.Rarity.Rare : GameManager.Rarity.Epic,
            GameManager.Rarity.Epic => chance < 0.3f ? GameManager.Rarity.Rare : chance < 0.7f ? GameManager.Rarity.Epic : GameManager.Rarity.Legendary,
            GameManager.Rarity.Legendary => chance < 0.5f ? GameManager.Rarity.Epic : GameManager.Rarity.Legendary,
            _ => GameManager.Rarity.Common
        };
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
            Spaceship.Instance.GetShipUpgradeManager().UpgradeTravelSpeed(0.1f);
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

    public void HireCrewMember(int index) {
        if (index < 0 || index >= availableCrew.Count) {
            Debug.LogError("Invalid crew member index!");
            return;
        }

        CrewManager.CrewMember member = availableCrew[index];
        CrewManager.Instance.HireCrewMember(member.name, member.type, member.rarity);
        availableCrew.RemoveAt(index);
    }

    public List<CrewManager.CrewMember> GetAvailableCrew() {
        return availableCrew;
    }
}

