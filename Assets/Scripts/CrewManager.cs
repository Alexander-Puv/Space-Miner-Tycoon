using System.Collections.Generic;
using UnityEngine;

public class CrewManager : MonoBehaviour {
    public static CrewManager Instance { get; private set; }

    public enum CrewType {
        Engineer, // ”скор€ет ремонт, снижает расход прочности
        Pilot,    // ”величивает скорость перемещени€, снижает расход топлива
        Scientist // ”величивает скорость добычи
    }

    [System.Serializable]
    public class CrewMember {
        public string name;
        public CrewType type;
        public GameManager.Rarity rarity;
        public float bonusMultiplier;
        public float hireCost;

        public CrewMember(string name, CrewType type, GameManager.Rarity rarity) {
            this.name = name;
            this.type = type;
            this.rarity = rarity;
            this.bonusMultiplier = GetBonusMultiplier(rarity);
            this.hireCost = GetHireCost(rarity);
        }

        private float GetBonusMultiplier(GameManager.Rarity rarity) {
            return rarity switch {
                GameManager.Rarity.Common => 1.1f,
                GameManager.Rarity.Uncommon => 1.25f,
                GameManager.Rarity.Rare => 1.5f,
                GameManager.Rarity.Epic => 2f,
                GameManager.Rarity.Legendary => 3f,
                _ => 1f
            };
        }

        private float GetHireCost(GameManager.Rarity rarity) {
            return rarity switch {
                GameManager.Rarity.Common => 100f,
                GameManager.Rarity.Uncommon => 250f,
                GameManager.Rarity.Rare => 500f,
                GameManager.Rarity.Epic => 1000f,
                GameManager.Rarity.Legendary => 2500f,
                _ => 50f
            };
        }
    }

    private List<CrewMember> crew = new();
    private Dictionary<CrewType, float> crewBonuses = new() {
        { CrewType.Engineer, 1f },
        { CrewType.Pilot, 1f },
        { CrewType.Scientist, 1f }
    };

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void HireCrewMember(string name, CrewType type, GameManager.Rarity rarity) {
        CrewMember member = new CrewMember(name, type, rarity);
        if (Inventory.Instance.SpendMoney(member.hireCost)) {
            crew.Add(member);
            UpdateCrewBonuses();
            Debug.Log($"Hired {name} ({type}, {rarity}) for {member.hireCost} credits!");
        } else {
            Debug.Log("Not enough money to hire crew member!");
        }
    }

    private void UpdateCrewBonuses() {
        crewBonuses[CrewType.Engineer] = 1f;
        crewBonuses[CrewType.Pilot] = 1f;
        crewBonuses[CrewType.Scientist] = 1f;

        foreach (var member in crew) {
            crewBonuses[member.type] *= member.bonusMultiplier;
        }

        Spaceship.Instance.UpdateShipStats();
    }

    public float GetCrewBonus(CrewType type) {
        return crewBonuses[type];
    }

    public List<CrewMember> GetCrew() {
        return crew;
    }
}