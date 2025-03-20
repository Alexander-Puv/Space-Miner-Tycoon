using UnityEngine;

public class Planet : Location {
    public void RepairSpaceship() {
        Spaceship.Instance.durability = Spaceship.Instance.GetCurrentMaxDurability();
        Debug.Log("Spaceship repaired!");
    }

    public void RefuelSpaceship() {
        Spaceship.Instance.fuel = Spaceship.Instance.GetCurrentMaxFuel();
        Debug.Log("Spaceship refueled!");
    }
}
