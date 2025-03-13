using UnityEngine;

public class GameManager : MonoBehaviour {

    private Location currentLocation;

    public void NewLocation(Location location) {
        Destroy(currentLocation.gameObject);

        currentLocation = location;
    }

    public Location GetLocation() {
        return currentLocation;
    }
}
