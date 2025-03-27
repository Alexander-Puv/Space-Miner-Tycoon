using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class PlanetModel : LocationModel {
    public enum PlanetType {
        Industrial, // Дешевое топливо, дорогие апгрейды
        Mining,     // Дорогие ресурсы, дешевый ремонт
        Trading     // Средние цены, возможны скидки/надбавки
    }

    [SerializeField] private List<PlanetType> planetTypes;

    public PlanetType GetRandomPlanetType() {
        if (planetTypes == null) {
            return PlanetType.Trading;
        }
        return planetTypes[Random.Range(0, planetTypes.Count - 1)];
    }
}
