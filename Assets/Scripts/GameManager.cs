using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

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

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {
        NewLocation(CreateAsteroid());
    }

    public void NewLocation(Location location) {
        if (currentLocation != null) {
            Destroy(currentLocation.gameObject);
        }

        currentLocation = location;
        Spaceship.Instance.UpdateLocation();
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


    public static Vector2 ClampToScreen(Vector3 position) {
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        position.x = Mathf.Clamp(position.x, min.x, max.x);
        position.y = Mathf.Clamp(position.y, min.y, max.y);

        return (Vector2)position;
    }
}
