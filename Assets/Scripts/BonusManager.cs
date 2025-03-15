using System.Collections.Generic;
using UnityEngine;
using static BonusManager;

public class BonusManager : MonoBehaviour {
    public static BonusManager Instance { get; private set; }

    public enum BonusType {
        MiningSpeed,
        MovementSpeed
    }

    [SerializeField] private List<Bonus> possibleBonuses;
    
    private List<BonusListElement> activeBonuses = new();

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update() {
        for (int i = activeBonuses.Count - 1; i >= 0; i--) {
            activeBonuses[i].duration -= Time.deltaTime;
            if (activeBonuses[i].duration <= 0) {
                activeBonuses.RemoveAt(i);

                Debug.Log("Bonus was removed");
                foreach (var bonus in activeBonuses) {
                    Debug.Log($"All active bonuses: {bonus}");
                }
            }
        }
    }

    public void ApplyBonus(BonusType type, float value, float duration) {
        activeBonuses.Add(new BonusListElement(type, value, duration));

        Debug.Log($"New Bonus. Type: {type}, value: {value}, duration: {duration}");
        foreach (var bonus in activeBonuses) {
            Debug.Log($"All active bonuses: {bonus}");
        }
    }

    public float GetBonusValue(BonusType type) {
        float totalBonus = 0f;
        foreach (var bonus in activeBonuses) {
            if (bonus.type == type) {
                totalBonus += bonus.value;
            }
        }
        return totalBonus;
    }


    public Bonus GetRandomBonus() {
        var randomBonus = possibleBonuses[Random.Range(0, possibleBonuses.Count)];

        if (possibleBonuses.Count == 0 || Random.value > randomBonus.bonusDropChance) {
            return null;
        }
        return possibleBonuses[Random.Range(0, possibleBonuses.Count)];
    }
}

public class BonusListElement {
    public BonusType type;
    public float value;
    public float duration;

    public BonusListElement(BonusType type, float value, float duration) {
        this.type = type;
        this.value = value;
        this.duration = duration;
    }
}