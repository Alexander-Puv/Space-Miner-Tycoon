using UnityEngine;
using UnityEngine.InputSystem;

public class Spaceship : MonoBehaviour {
    public static Spaceship Instance { get; private set; }

    public float miningSpeed = 1f;

    private Location currentLocation;
    private InputActions inputActions;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        inputActions = new();
        inputActions.Touch.Enable();
    }

    private void Update() {
        if (currentLocation is Asteroid asteroid) {
            //asteroid.MineResource();

            if (inputActions.Touch.ClickOnAsteroid.WasPerformedThisFrame()) {
                asteroid.MineResource();

                Vector2 screenPosition = Mouse.current.position.ReadValue();
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

                BonusManager.Instance.TryDropBonus(worldPosition);
            }
        }
    }

    public void UpdateLocation() {
        currentLocation = GameManager.Instance.GetLocation();
    }
}
