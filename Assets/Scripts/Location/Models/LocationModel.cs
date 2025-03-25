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

    public GameManager.Rarity GetRarity() {
        if (rarity == null) {
            return GameManager.Rarity.Common;
        }

        return rarity[Random.Range(0, rarity.Count - 1)];
    }
}
