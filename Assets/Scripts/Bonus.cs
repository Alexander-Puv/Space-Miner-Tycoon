using UnityEngine;

public class Bonus : MonoBehaviour {
    [SerializeField] private Vector2 ValueFromTo;
    [SerializeField] private Vector2 DurationFromTo;
    public BonusManager.BonusType Type;
    public float bonusDropChance = .05f;

    private float lifetime = 30f;
    private float floatSpeed = 6f;
    private float slowDownRate = 0.98f;
    private Vector2 moveDirection;
    private float rotationSpeedX;
    private float rotationSpeedY;
    private float rotationDamping = 0.95f;
    private float collectibleTime = .5f;
    private bool isCollectible = false;

    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector2 spawnPosition) {
        transform.position = spawnPosition;
        moveDirection = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f)).normalized;

        rotationSpeedX = Random.Range(200f, 400f);
        rotationSpeedY = Random.Range(200f, 400f);

        rb.linearVelocity = (Vector3)moveDirection * floatSpeed;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        Destroy(gameObject, lifetime);
        Invoke(nameof(MakeCollectible), collectibleTime);
    }

    private void Update() {
        rb.linearVelocity *= slowDownRate;

        transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, 0);
        rotationSpeedX *= rotationDamping;
        rotationSpeedY *= rotationDamping;

        KeepInsideScreen();
    }

    private void KeepInsideScreen() {
        Vector3 pos = transform.position;
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        bool bounced = false;

        if (pos.x < min.x || pos.x > max.x) {
            moveDirection.x *= -1;
            bounced = true;
        }
        if (pos.y < min.y || pos.y > max.y) {
            moveDirection.y *= -1;
            bounced = true;
        }

        if (bounced) {
            rb.linearVelocity = (Vector3)moveDirection * floatSpeed * 0.5f;
        }

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        transform.position = pos;
    }

    private void MakeCollectible() {
        isCollectible = true;
    }

    public float GetRandomValue() {
        return Random.Range(ValueFromTo.x, ValueFromTo.y);
    }

    public float GetRandomDuration() {
        return Random.Range(DurationFromTo.x, DurationFromTo.y);
    }

    private void OnMouseDown() {
        if (!isCollectible) return;

        BonusManager.Instance.ApplyBonus(Type, GetRandomValue(), GetRandomDuration());
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject == Spaceship.Instance.gameObject) {
            Vector3 reflectDir = Vector3.Reflect(rb.linearVelocity.normalized, collision.contacts[0].normal);
            rb.linearVelocity = reflectDir * floatSpeed * 0.7f;
        }
    }
}
