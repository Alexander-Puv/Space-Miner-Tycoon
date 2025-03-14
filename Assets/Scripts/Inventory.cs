using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public static Inventory Instance { get; private set; }

    public Dictionary<Resource, int> resources = new();

    private void Start() {
        Instance = this;
    }

    public void AddResource(Resource resource, int amount) {
        if (resources.ContainsKey(resource)) {
            resources[resource] += amount;
        } else {
            resources.Add(resource, amount);
        }
        
        Debug.Log($"Added {amount} of {resource.resourceName}. Total: {resources[resource]}");
    }

    public void RemoveResources(Resource resource, int amount) {
        if (resources.ContainsKey(resource)) {
            resources[resource] -= amount;
            if (resources.ContainsKey(resource)) {
                resources.Remove(resource);
            }
        }

        int total = resources.ContainsKey(resource) ? resources[resource] : 0;
        Debug.Log($"Removed {amount} of {resource.resourceName}. Total: {total}");
    }

    public int GetResourceAmount(Resource resource) {
        return resources.ContainsKey(resource) ? resources[resource] : 0;
    }
}
