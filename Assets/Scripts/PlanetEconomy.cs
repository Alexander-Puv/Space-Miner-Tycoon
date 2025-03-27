using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class PlanetEconomy : MonoBehaviour {
    public static PlanetEconomy Instance {  get; private set; }

    public enum EventType {
        None,
        War,          // Всё сильно дороже, меньше спрос на невоенные товары
        EconomicBoom, // Покупают дороже и продают дороже
        Crisis        // Топливо в дефиците, цены на него выше
    }

    public enum ServiceType {
        Repair,
        Fuel,
        Upgrade,
    }

    public PlanetModel.PlanetType Type;
    public EventType CurrentEvent = EventType.None;

    private Dictionary<Resource, float> resourcePrices = new();
    private Dictionary<ServiceType, float> servicePrices = new();

    public void GenerateBasePrices() {
        foreach (var resource in GameManager.Instance.resourcesList) {
            resourcePrices[resource] = GetBaseResourcePrice(resource);
        }

        servicePrices[ServiceType.Repair] = 500f;
        servicePrices[ServiceType.Fuel] = 2f;
        servicePrices[ServiceType.Upgrade] = 1000f;
    }

    private float GetBaseResourcePrice(Resource resource) {
        return Type switch {
            PlanetModel.PlanetType.Industrial => resource.basePrice * 0.8f,
            PlanetModel.PlanetType.Mining => resource.basePrice * 1.2f,
            PlanetModel.PlanetType.Trading => resource.basePrice,
            _ => resource.basePrice
        };
    }

    public void ApplyEventModifiers() {
        switch (CurrentEvent) {
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
                servicePrices[ServiceType.Upgrade] *= 0.8f;
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
        CurrentEvent = (EventType)Random.Range(0, 4);
        ApplyEventModifiers();
    }
}
