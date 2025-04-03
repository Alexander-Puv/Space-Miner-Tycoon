using UnityEngine;

public class ShipUpgradeManager {
    [System.Serializable]
    public class ShipUpgrades {
        public float fuelCapacityMultiplier = 1f;
        public float fuelEfficiencyMultiplier = 1f;
        public float durabilityMultiplier = 1f;
        public float miningSpeedMultiplier = 1f;
        public float travelSpeedMultiplier = 1f;
    }

    private Spaceship spaceship;
    private ShipUpgrades upgrades;

    public ShipUpgradeManager(Spaceship spaceship) {
        this.spaceship = spaceship;
        upgrades = new ShipUpgrades();
    }

    public void UpgradeFuelCapacity(float multiplierIncrease) {
        upgrades.fuelCapacityMultiplier += multiplierIncrease;
        spaceship.UpdateShipStats();
        spaceship.ClampFuel();
    }

    public void UpgradeFuelEfficiency(float multiplierDecrease) {
        upgrades.fuelEfficiencyMultiplier -= multiplierDecrease;
        upgrades.fuelEfficiencyMultiplier = Mathf.Max(0.1f, upgrades.fuelEfficiencyMultiplier);
    }

    public void UpgradeDurability(float multiplierIncrease) {
        upgrades.durabilityMultiplier += multiplierIncrease;
        spaceship.UpdateShipStats();
        spaceship.ClampDurability();
    }

    public void UpgradeMiningSpeed(float multiplierIncrease) {
        upgrades.miningSpeedMultiplier += multiplierIncrease;
        spaceship.UpdateShipStats();
    }

    public void UpgradeTravelSpeed(float multiplierIncrease) {
        upgrades.travelSpeedMultiplier += multiplierIncrease;
        spaceship.UpdateShipStats();
    }

    public float GetFuelCapacityMultiplier() => upgrades.fuelCapacityMultiplier;
    public float GetFuelEfficiencyMultiplier() => upgrades.fuelEfficiencyMultiplier;
    public float GetDurabilityMultiplier() => upgrades.durabilityMultiplier;
    public float GetMiningSpeedMultiplier() => upgrades.miningSpeedMultiplier;
    public float GetTravelSpeedMultiplier() => upgrades.travelSpeedMultiplier;
}