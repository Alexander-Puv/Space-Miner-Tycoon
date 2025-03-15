using UnityEngine;

public class Bonus : MonoBehaviour {
    [SerializeField] private Vector2 ValueFromTo;
    [SerializeField] private Vector2 DurationFromTo;
    public BonusManager.BonusType Type;
    public float bonusDropChance = .05f;

    public float GetRandomValue() {
        return Random.Range(ValueFromTo.x, ValueFromTo.y);
    }

    public float GetRandomDuration() {
        return Random.Range(DurationFromTo.x, DurationFromTo.y);
    }
}