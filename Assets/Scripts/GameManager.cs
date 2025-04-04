using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField] private Planet planetPrefab;
    [SerializeField] private List<PlanetModel> planetModels;

    public List<Resource> resourcesList;

    private Location currentLocation;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {
        TravelMenu.Instance.HideMenu();
        NewLocation(CreateAsteroid());
    }

    public void NewLocation(Location location) {
        if (currentLocation is Planet) {
            PlanetEconomy.Instance.currentEvent = PlanetEconomy.EventType.None;
        }

        if (currentLocation != null) {
            Destroy(currentLocation.gameObject);
        }

        currentLocation = location;
        currentLocation.transform.localScale = Vector3.one;
        Spaceship.Instance.UpdateLocation(currentLocation);

        TravelMenu.Instance.ShowMenu();
        TravelMenu.Instance.GenerateNewLocations();
        TravelMenu.Instance.HideMenu();

        UIManager.Instance.SetTravelButtonsDisplay(false);
    }

    public Location GetLocation() {
        return currentLocation;
    }


    public Asteroid CreateAsteroid(Transform parent = null) {
        return CreateLocation(asteroidPrefab, new List<LocationModel>(asteroidModels), parent) as Asteroid;
    }
    public Planet CreatePlanet(Transform parent = null) {
        return CreateLocation(planetPrefab, new List<LocationModel>(planetModels), parent) as Planet;
    }
    public Location CreateLocation(Location prefab, List<LocationModel> models, Transform parent = null) {
        Location location = Instantiate(prefab, parent);
        LocationModel locationModel = models[Random.Range(0, models.Count)];
        Instantiate(locationModel, location.transform);
        return location;
    }


    public static Vector2 ClampToScreen(Vector3 position) {
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        position.x = Mathf.Clamp(position.x, min.x, max.x);
        position.y = Mathf.Clamp(position.y, min.y, max.y);

        return (Vector2)position;
    }


    public void StartTravel(Location location) {
        Spaceship.Instance.TravelTo(location);
    }
}
