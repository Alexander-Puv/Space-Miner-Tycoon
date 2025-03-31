using System.Collections.Generic;
using UnityEngine;

public class LocationModel : MonoBehaviour {
    [SerializeField] private Vector3 rotationFrom;
    [SerializeField] private Vector3 rotationTo;
    [SerializeField] private Vector3 positionFrom;
    [SerializeField] private Vector3 positionTo;
    [SerializeField] private Vector3 ScaleFrom;
    [SerializeField] private Vector3 ScaleTo;

    [SerializeField] private List<GameManager.Rarity> rarity;
    public int minLayers = 1;
    public int maxLayers = 5;

    private bool canBeSelected = false;

    public void SetCanBeSelected(bool value) {
        canBeSelected = value;
    }

    private void OnMouseDown() {
        Location location = GetComponentInParent<Location>();
        if (canBeSelected) {
            TravelMenu.Instance.SelectLocation(location);
        } else if (location is Asteroid asteroid) {
            Spaceship.Instance.OnAsteroidClick(asteroid);
        }
    }

    public GameManager.Rarity GetRarity() {
        if (rarity == null) {
            return GameManager.Rarity.Common;
        }

        return rarity[Random.Range(0, rarity.Count - 1)];
    }
}
