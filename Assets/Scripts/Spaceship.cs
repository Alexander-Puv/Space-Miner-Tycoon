using UnityEngine;

public class Spaceship : MonoBehaviour {
    public static Spaceship Instance {  get; private set; }

    private void MineResource(Resource resource, int amount) {
        Inventory.Instance.AddResource(resource, amount);
    }
}