using UnityEngine;
using UnityEngine.InputSystem;

public class Spaceship : MonoBehaviour {
    public static Spaceship Instance {  get; private set; }

    public float miningSpeed = 1f;

    private Location currentLoaction;
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
        if (currentLoaction is Asteroid) {
            Asteroid asteroid = currentLoaction as Asteroid;
            //asteroid.MineResource();

            if (inputActions.Touch.ClickOnAsteroid.WasPerformedThisFrame()) {
                asteroid.MineResource();
                TryDropBonus();
            }
        }
    }

    public void UpdateLocation() {
        currentLoaction = GameManager.Instance.GetLocation();
    }

    private void TryDropBonus() {
        Bonus bonus = BonusManager.Instance.GetRandomBonus();
        if (bonus != null) {
            BonusManager.Instance.ApplyBonus(bonus.Type, bonus.GetRandomValue(), bonus.GetRandomDuration());
        }
    }
}