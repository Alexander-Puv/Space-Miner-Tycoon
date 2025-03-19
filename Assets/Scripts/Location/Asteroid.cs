using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Location {
    public class ResourceLayer {
        public Resource resource;
        public int amount;
    }

    [SerializeField] private List<Resource> resourcesList;

    public GameManager.Rarity rarity;
    public int minResourcesAmount = 300;
    public int maxResourcesAmount = 1000;

    private float miningDifficulty;
    private List<ResourceLayer> resourceLayers = new();
    private AsteroidModel asteroidModel;
    private Dictionary<GameManager.Rarity, float> rarityChances = new() {
        { GameManager.Rarity.Common, 50f },
        { GameManager.Rarity.Uncommon, 30f },
        { GameManager.Rarity.Rare, 15f },
        { GameManager.Rarity.Epic, 4f },
        { GameManager.Rarity.Legendary, 1f }
    };

    private void Start() {
        asteroidModel = GetComponentInChildren<AsteroidModel>();
        rarity = asteroidModel.GetRarity();

        GenerateResources();
    }

    private void GenerateResources() {
        resourceLayers.Clear();

        int layerCount = Random.Range(asteroidModel.minLayers, asteroidModel.maxLayers + 1);

        for (int i = 0; i < layerCount; i++) {
            ResourceLayer layer = new() {
                resource = GetResourceBasedOnRarity(),
                amount = GetResourceAmountBasedOnRarity()
            };
            resourceLayers.Add(layer);
        }

        CalculateMiningDifficulty();
    }

    private Resource GetResourceBasedOnRarity() {
        float totalChances = 0f;
        float lostChances = 0f;
        int lostChancesAmount = 0;
        foreach (var rarityChance in rarityChances) {
            if (rarityChance.Key <= rarity) {
                totalChances += rarityChance.Value;
            } else {
                lostChances += rarityChance.Value;
                lostChancesAmount++;
            }
        }

        float randomValue = Random.Range(0, totalChances);
        foreach (var resource in resourcesList) {
            if (randomValue < (rarityChances[resource.rarity] - lostChances / lostChancesAmount)) {
                return resource;
            }
            randomValue -= rarityChances[resource.rarity];
        }

        return resourcesList[0];
    }

    private int GetResourceAmountBasedOnRarity() {
        switch (rarity) {
            case GameManager.Rarity.Common:
                return Random.Range(minResourcesAmount, maxResourcesAmount);
            case GameManager.Rarity.Uncommon:
                return Random.Range(minResourcesAmount / 2, maxResourcesAmount / 2);
            case GameManager.Rarity.Rare:
                return Random.Range(minResourcesAmount / 4, maxResourcesAmount / 4);
            case GameManager.Rarity.Epic:
                return Random.Range(minResourcesAmount / 8, maxResourcesAmount / 8);
            case GameManager.Rarity.Legendary:
                return Random.Range(minResourcesAmount / 16, maxResourcesAmount / 16);
            default:
                return minResourcesAmount;
        }
    }

    private void CalculateMiningDifficulty() {
        miningDifficulty = 1f;

        foreach (var layer in resourceLayers) {
            miningDifficulty += .2f * (int)layer.resource.rarity;
            miningDifficulty -= .05f * ((int)rarity - (int)layer.resource.rarity);
        }

    }



    public void MineResource(float miningSpeed) {
        if (resourceLayers.Count == 0) {
            Debug.Log("Asteroid is depleted!");
            return;
        }

        ResourceLayer layer = resourceLayers[0];
        int minedAmount = (int)(Mathf.Min(layer.amount, 10) * miningDifficulty * layer.resource.miningDifficulty / miningSpeed);

        if (layer.amount - minedAmount < 0) {
            Inventory.Instance.AddResource(layer.resource, layer.amount);
            layer.amount = 0;
        } else {
            Inventory.Instance.AddResource(layer.resource, minedAmount);
            layer.amount -= minedAmount;
        }

        if (layer.amount <= 0) {
            resourceLayers.RemoveAt(0);
        }
    }
}
