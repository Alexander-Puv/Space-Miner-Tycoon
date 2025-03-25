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

    private const string TRAVEL_BUTTON = "TravelButton";
    private const string TRAVEL_CLOSE_MENU_BUTTON = "CloseMenu";

    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private List<AsteroidModel> asteroidModels;
    [SerializeField] private Planet planetPrefab;
    [SerializeField] private List<PlanetModel> planetModels;
    [SerializeField] private UIDocument UIDoc;
    
    private Location currentLocation;
    private Button travelButton;
    private Button travelCloseMenuButton;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        travelButton = UIDoc.rootVisualElement.Q<Button>(TRAVEL_BUTTON);
        travelCloseMenuButton = UIDoc.rootVisualElement.Q<Button>(TRAVEL_CLOSE_MENU_BUTTON);

        travelButton.RegisterCallback<MouseEnterEvent>(OnTravelButtonHover);
        travelButton.RegisterCallback<MouseLeaveEvent>(OnTravelButtonUnhover);
        travelButton.RegisterCallback<ClickEvent>(OnTravelButtonClick);

        travelCloseMenuButton.RegisterCallback<MouseEnterEvent>(OnTravelCloseMenuButtonHover);
        travelCloseMenuButton.RegisterCallback<MouseLeaveEvent>(OnTravelCloseMenuButtonUnhover);
        travelCloseMenuButton.RegisterCallback<ClickEvent>(OnTravelCloseMenuButtonClick);
    }

    private void Start() {
        TravelMenu.Instance.HideMenu();
        NewLocation(CreateAsteroid());
    }

    public void NewLocation(Location location, bool start = false) {
        if (currentLocation != null) {
            Destroy(currentLocation.gameObject);
        }

        currentLocation = location;
        currentLocation.transform.localScale = Vector3.one;
        Spaceship.Instance.UpdateLocation(currentLocation);

        TravelMenu.Instance.ShowMenu();
        TravelMenu.Instance.GenerateNewLocations();
        TravelMenu.Instance.HideMenu();

        SetTravelButtonsDisplay(false);
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


    public void SetTravelButtonsDisplay(bool showMenu) {
        if (showMenu) {
            travelButton.style.display = DisplayStyle.None;
            travelCloseMenuButton.style.display = DisplayStyle.Flex;
        } else {
            travelButton.style.display = DisplayStyle.Flex;
            travelCloseMenuButton.style.display = DisplayStyle.None;
        }
    }

    private void OnTravelButtonHover(MouseEnterEvent e) {
        travelButton.style.scale = new StyleScale(new Scale(new Vector2(1.1f, 1.1f)));
    }

    private void OnTravelButtonUnhover(MouseLeaveEvent e) {
        travelButton.style.scale = new StyleScale(new Scale(Vector2.one));
    }

    private void OnTravelButtonClick(ClickEvent e) {
        TravelMenu.Instance.ShowMenu();
    }

    private void OnTravelCloseMenuButtonHover(MouseEnterEvent e) {
        //////////////////////////
    }

    private void OnTravelCloseMenuButtonUnhover(MouseLeaveEvent e) {
        //////////////////////////
    }

    private void OnTravelCloseMenuButtonClick(ClickEvent e) {
        TravelMenu.Instance.HideMenu();
    }

    public void StartTravel(Location location) {
        Spaceship.Instance.TravelTo(location);
    }
}
