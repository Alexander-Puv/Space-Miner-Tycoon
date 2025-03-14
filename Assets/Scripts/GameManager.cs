using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public enum Rarity {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private List<AsteroidModel> asteroidModels;

    private Location currentLocation;

    private void Start() {
        NewLocation(CreateAsteroid());
    }

    public void NewLocation(Location location) {
        if (currentLocation != null) {
            Destroy(currentLocation.gameObject);
        }

        currentLocation = location;
    }

    public Location GetLocation() {
        return currentLocation;
    }


    public Asteroid CreateAsteroid() {
        Asteroid asteroidObject = Instantiate(asteroidPrefab);
        AsteroidModel asteroidModel = asteroidModels[Random.Range(0, asteroidModels.Count)];
        Instantiate(asteroidModel, asteroidObject.transform);
        return asteroidObject.GetComponent<Asteroid>();
    }
}
