using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class PlanetEconomy : MonoBehaviour {
    public static PlanetEconomy Instance { get; private set; }

    public enum EventType {
        None,
        War,          // Всё сильно дороже, меньше спрос на невоенные товары
        EconomicBoom, // Покупают дороже и продают дороже (улучшения дешевле?)
        Crisis        // Топливо в дефиците, цены на него выше
    }

    public enum ServiceType {
        Repair,
        Fuel,
        Upgrade,
        CrewHire
    }

    public const float BASE_REPAIR_PRICE = 500f;
    public const float BASE_FUEL_PRICE = 2f;
    public const float BASE_UPGRADE_PRICE = 1000f;
    public const float BASE_CREW_HIRE_PRICE = 100f;

    public EventType currentEvent = EventType.None;

    private Dictionary<Resource, float> resourcePrices = new();
    private Dictionary<ServiceType, float> servicePrices = new();

    private void Awake() {
        Instance = this;
    }

    public void GenerateBasePrices(PlanetModel.PlanetType planetType) {
        servicePrices.Clear();
        resourcePrices.Clear();

        servicePrices.Add(ServiceType.Repair, BASE_REPAIR_PRICE);
        servicePrices.Add(ServiceType.Fuel, BASE_FUEL_PRICE);
        servicePrices.Add(ServiceType.Upgrade, BASE_UPGRADE_PRICE);
        servicePrices.Add(ServiceType.CrewHire, BASE_CREW_HIRE_PRICE);

        foreach (var resource in GameManager.Instance.resourcesList) {
            resourcePrices[resource] = GetBaseResourcePrice(resource, planetType);
        }

        switch (planetType) {
            case PlanetModel.PlanetType.Industrial:
                servicePrices[ServiceType.Upgrade] *= .8f;
                servicePrices[ServiceType.CrewHire] *= 1.2f;
                break;
            case PlanetModel.PlanetType.Trading:
                var rand = Random.Range(0f, 1f);
                var crewHireModifier = 1f;
                if (rand < .2f) {
                    crewHireModifier = .9f;
                }
                if (rand > .8f) {
                    crewHireModifier = 1.1f;
                }
                servicePrices[ServiceType.CrewHire] *= crewHireModifier;
                break;
            case PlanetModel.PlanetType.Petrocracy:
                servicePrices[ServiceType.Fuel] *= .8f;
                servicePrices[ServiceType.CrewHire] *= 1.1f;
                break;
            case PlanetModel.PlanetType.Mining:
                servicePrices[ServiceType.Upgrade] *= 1.2f;
                servicePrices[ServiceType.CrewHire] *= 1.0f;
                break;
        }
    }

    private float GetBaseResourcePrice(Resource resource, PlanetModel.PlanetType planetType) {
        switch (planetType) {
            case PlanetModel.PlanetType.Industrial:
                return resource.basePrice * 1.2f;
            case PlanetModel.PlanetType.Mining:
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
                return resource.basePrice * 1.2f;
            default: return resource.basePrice;
        }
    }

    public void ApplyEventModifiers() {
        var keys = new List<Resource>(resourcePrices.Keys);

        switch (currentEvent) {
            case EventType.War:
                foreach (var key in keys) resourcePrices[key] *= 1.5f;
                servicePrices[ServiceType.Repair] *= 2f;
                servicePrices[ServiceType.CrewHire] *= 1.5f;
                break;
            case EventType.EconomicBoom:
                foreach (var key in keys) resourcePrices[key] *= 1.3f;
                servicePrices[ServiceType.Upgrade] *= 1.1f;
                servicePrices[ServiceType.CrewHire] *= 1.2f;
                break;
            case EventType.Crisis:
                servicePrices[ServiceType.Fuel] *= 2f;
                servicePrices[ServiceType.CrewHire] *= .9f;
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
