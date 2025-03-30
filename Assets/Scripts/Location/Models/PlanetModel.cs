using System.Collections.Generic;
using UnityEngine;

public class PlanetModel : LocationModel {
    public enum PlanetType {
        Industrial, // Дорогие ресурсы, дешевый ремонт
        Mining,     // Дешевые ресурсы, дорогие апгрейды
        Trading,    // Средние цены, возможны скидки/надбавки
        Petrocracy  // Цены на нефть дешевле, на ресурсы - дороже
    }

    [SerializeField] private List<PlanetType> planetTypes;

    public PlanetType GetRandomPlanetType() {
        if (planetTypes.Count == 0) {
            return PlanetType.Trading;
        }
        return planetTypes[Random.Range(0, planetTypes.Count - 1)];
    }
}
