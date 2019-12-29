using UnityEngine;

[CreateAssetMenu(fileName = "New simple bullet", menuName = "Bullets/Simple")]
public class SimpleBullet : BulletBehaviour {

    public override Vector2 GetDeltaMove(Bullet bullet) {
        return bullet.transform.right * bullet.MoveSpeed * Time.deltaTime;
    }

    public override void OnCollision(Bullet bullet, Collider2D otherCollider) {
        Health health = otherCollider.GetComponent<Health>();
        if (health != null) {
            health.TakeDamage(bullet.Damage);
        }
        Destroy(bullet.gameObject);
    }
}
