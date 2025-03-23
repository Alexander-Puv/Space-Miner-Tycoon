using System.Collections.Generic;
using UnityEngine;

public class TravelMenu : MonoBehaviour {
    public static TravelMenu Instance { get; private set; }

    [SerializeField] private Transform locationsContainer;
    [SerializeField] private Camera mainCamera;

    private List<Location> locations = new();
    private Vector3 cameraOriginalPosition;
    private float menuScaleFactor = 0.5f;

    private void Awake() {
        Instance = this;

        cameraOriginalPosition = mainCamera.transform.position;
    }

    public void ShowMenu() {
        float cameraWidth = mainCamera.orthographicSize * 2 * mainCamera.aspect;
        mainCamera.transform.position = cameraOriginalPosition + Vector3.right * cameraWidth;
        transform.position = mainCamera.transform.position + new Vector3(0, 0, 10);

        GameManager.Instance.SetTravelButtonsDisplay(true);
        gameObject.SetActive(true);
    }

    public void HideMenu() {
        mainCamera.transform.position = cameraOriginalPosition;

        GameManager.Instance.SetTravelButtonsDisplay(false);
        gameObject.SetActive(false);
    }

    public void GenerateNewLocations() {
        foreach (var loc in locations) {
            if (loc == null || loc.gameObject == null) continue;
            Destroy(loc.gameObject);
        }
        locations.Clear();

        locations.Add(CreateLocation(GameManager.Instance.CreateAsteroid(locationsContainer), new Vector2(0.8f, 0.8f)));
        locations.Add(CreateLocation(GameManager.Instance.CreateAsteroid(locationsContainer), new Vector2(0.6f, 0.2f)));
        locations.Add(CreateLocation(GameManager.Instance.CreatePlanet(locationsContainer), new Vector2(0.2f, 0.8f)));
    }

    public void SelectLocation(Location location) {
        foreach (var loc in locations) {
            if (loc != location) {
                Destroy(loc.gameObject);
            }
        }

        HideMenu();
        GameManager.Instance.SetTravelButtonsDisplay(false);
        location.SetCanBeSelected(false);
        GameManager.Instance.StartTravel(location);
    }

    private Location CreateLocation(Location location, Vector2 screenPosition) {
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10));
        location.transform.position = worldPosition;
        location.transform.localScale *= menuScaleFactor;

        location.SetCanBeSelected(true);

        return location;
    }
}
