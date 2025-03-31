using UnityEngine;

public class Background : MonoBehaviour {
    private void OnMouseDown() {
        if (GameManager.Instance.GetLocation() is Asteroid asteroid) {
            Spaceship.Instance.OnAsteroidClick(asteroid);
        }
    }
}
