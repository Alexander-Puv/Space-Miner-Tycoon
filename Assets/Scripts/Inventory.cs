using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public static Inventory Instance { get; private set; }
    
    public float money;

    public Dictionary<Resource, int> resources = new();
    public int maxCapacity = 1000;
    public int currentCapacity = 0;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool AddResource(Resource resource, int amount) {
        if (currentCapacity + amount > maxCapacity) {
            Debug.Log("Inventory is full!");
            return false;
        }

        if (resources.ContainsKey(resource)) {
            resources[resource] += amount;
        } else {
            resources.Add(resource, amount);
        }

        currentCapacity += amount;
        UIManager.Instance.UpdateResourcesDisplay(currentCapacity);
        Debug.Log($"Added {amount} of {resource.resourceName}. Total: {resources[resource]}");
        return true;
    }

    public void RemoveResources(Resource resource, int amount) {
        if (resources.ContainsKey(resource)) {
            resources[resource] -= amount;
            currentCapacity -= amount;

            if (resources.ContainsKey(resource)) {
                resources.Remove(resource);
            }
        }

        UIManager.Instance.UpdateResourcesDisplay(currentCapacity);
        int total = resources.ContainsKey(resource) ? resources[resource] : 0;
        Debug.Log($"Removed {amount} of {resource.resourceName}. Total: {total}");
    }

    public int GetResourceAmount(Resource resource) {
        return resources.ContainsKey(resource) ? resources[resource] : 0;
    }

    public void UpgradeCapacity(int additionalCapacity) {
        maxCapacity += additionalCapacity;
        Debug.Log($"Inventory capacity upgraded to {maxCapacity}");
    }


    public bool SpendMoney(float amount) {
        if (money < amount) return false;
        money -= amount;
        return true;
    }

    public void EarnMoney(float amount) {
        money += amount;
    }
}
