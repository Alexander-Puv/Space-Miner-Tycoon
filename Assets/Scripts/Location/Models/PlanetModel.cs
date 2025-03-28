using System.Collections.Generic;
using UnityEngine;

public class PlanetModel : LocationModel {
    public enum PlanetType {
        Industrial, // ������� �������, ������� ������
        Mining,     // ������� �������, ������� ��������
        Trading,    // ������� ����, �������� ������/��������
        Petrocracy  // ���� �� ����� �������, �� ������� - ������
    }

    [SerializeField] private List<PlanetType> planetTypes;

    public PlanetType GetRandomPlanetType() {
        if (planetTypes == null) {
            return PlanetType.Trading;
        }
        return planetTypes[Random.Range(0, planetTypes.Count - 1)];
    }
}
