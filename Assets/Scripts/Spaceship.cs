using UnityEngine;

public class Spaceship : MonoBehaviour {
    public static Spaceship Instance {  get; private set; }

    private Location currentLoaction;

    private void Start() {
        Instance = this;
    }

    //private void MineResource(Resource resource, int amount) {
    //    Inventory.Instance.AddResource(resource, amount);
    //}
}