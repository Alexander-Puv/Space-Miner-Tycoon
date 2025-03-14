using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum Rarity {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    private Location currentLocation;

    public void NewLocation(Location location) {
        Destroy(currentLocation.gameObject);

        currentLocation = location;
    }

    public Location GetLocation() {
        return currentLocation;
    }
}
