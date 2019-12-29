using UnityEngine;

[CreateAssetMenu(fileName = "New cosine bullet", menuName = "Bullets/Cosine")]
public class CosineBullet : BulletBehaviour {

    [SerializeField] float waveLength = 0.5f;
    [SerializeField] [Range(0f, 359.99f)] float maxAngle = 45f;

    float timeSinceSpawn;
    Vector2 startForwardDirection;
    float randomTimeOffset;

    public override void Initialize(BulletBehaviour originalBehaviour, Bullet bullet) {
        CosineBullet original = (CosineBullet)originalBehaviour;
        waveLength = original.waveLength;
        maxAngle = original.maxAngle;
        startForwardDirection = bullet.transform.right;
        randomTimeOffset = Random.Range(0f, 2f * Mathf.PI / (waveLength * bullet.MoveSpeed));
    }

    public override void Update() {
        base.Update();
        timeSinceSpawn += Time.deltaTime;
    }

    public override Vector2 GetDeltaMove(Bullet bullet) {
        Transform transform = bullet.transform;
        float angle = Mathf.Cos((timeSinceSpawn + randomTimeOffset) * waveLength * bullet.MoveSpeed) * maxAngle;
        Vector2 direction = Matrix2x2.CreateRotation(angle * Mathf.Deg2Rad) * startForwardDirection;
        transform.right = direction;
        return direction * bullet.MoveSpeed * Time.deltaTime;
    }

    public override void OnCollision(Bullet bullet, Collider2D otherCollider) {
        Health health = otherCollider.GetComponent<Health>();
        if (health != null) {
            health.TakeDamage(bullet.Damage);
        }
        Destroy(bullet.gameObject);
    }
}
