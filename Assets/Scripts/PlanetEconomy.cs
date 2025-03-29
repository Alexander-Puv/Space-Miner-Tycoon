using System.Collections.Generic;
using UnityEngine;

public class PlanetEconomy : MonoBehaviour {
    public static PlanetEconomy Instance { get; private set; }

    public enum EventType {
        None,
        War,          // �� ������ ������, ������ ����� �� ��������� ������
        EconomicBoom, // �������� ������ � ������� ������ (��������� �������?)
        Crisis        // ������� � ��������, ���� �� ���� ����
    }

    public enum ServiceType {
        Repair,
        Fuel,
        Upgrade,
    }

    public const float BASE_REPAIR_PRICE = 500f;
    public const float BASE_FUEL_PRICE = 2f;
    public const float BASE_UPGRADE_PRICE = 1000f;

    public EventType currentEvent = EventType.None;

    private Dictionary<Resource, float> resourcePrices = new();
    private Dictionary<ServiceType, float> servicePrices = new();

    public void GenerateBasePrices(PlanetModel.PlanetType planetType) {
        foreach (var resource in GameManager.Instance.resourcesList) {
            resourcePrices[resource] = GetBaseResourcePrice(resource, planetType);
        }

        servicePrices[ServiceType.Repair] = BASE_REPAIR_PRICE;
        servicePrices[ServiceType.Fuel] = BASE_FUEL_PRICE;
        servicePrices[ServiceType.Upgrade] = BASE_UPGRADE_PRICE;
    }

    private float GetBaseResourcePrice(Resource resource, PlanetModel.PlanetType planetType) {
        switch (planetType) {
            case PlanetModel.PlanetType.Industrial:
                servicePrices[ServiceType.Upgrade] *= .8f;
                return resource.basePrice * 1.2f;
            case PlanetModel.PlanetType.Mining:
                servicePrices[ServiceType.Upgrade] *= 1.2f;
                return resource.basePrice * .8f;
            case PlanetModel.PlanetType.Trading:
                var rand = Random.Range(0f, 1f);
                if (rand < .2f) {
                    return resource.basePrice * .8f;
                }
                if (rand > .8f) {
                    return resource.basePrice * 1.2f;
                }
                return resource.basePrice;
            case PlanetModel.PlanetType.Petrocracy:
                servicePrices[ServiceType.Fuel] *= .8f;
                return resource.basePrice * 1.2f;
            default: return resource.basePrice;
        }
    }

    public void ApplyEventModifiers() {
        switch (currentEvent) {
            case EventType.War:
                foreach (var key in resourcePrices.Keys) {
                    resourcePrices[key] *= 1.5f;
                }
                servicePrices[ServiceType.Repair] *= 2f;
                break;
            case EventType.EconomicBoom:
                foreach (var key in resourcePrices.Keys) {
                    resourcePrices[key] *= 1.3f;
                }
                servicePrices[ServiceType.Upgrade] *= .8f;
                break;
            case EventType.Crisis:
                servicePrices[ServiceType.Fuel] *= 2f;
                break;
        }
    }

    public float GetResourcePrice(Resource resource) {
        return resourcePrices.ContainsKey(resource) ? resourcePrices[resource] : 0f;
    }

    public float GetServicePrice(ServiceType service) {
        return servicePrices.ContainsKey(service) ? servicePrices[service] : 0f;
    }

    public void SetRandomEvent() {
        currentEvent = (EventType)Random.Range(0, 4);
        ApplyEventModifiers();
    }
}
