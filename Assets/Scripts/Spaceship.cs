using UnityEngine;

public class Spaceship : MonoBehaviour {
    public static Spaceship Instance {  get; private set; }

    public float miningSpeed = 1f;

    private Location currentLoaction;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update() {
        Location location = GameManager.Instance.GetLocation();
        if (location is Asteroid) {
            Asteroid asteroid = location as Asteroid;
            asteroid.MineResource();
        }
    }
}