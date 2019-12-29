using UnityEngine;

public abstract class BulletBehaviour : ScriptableObject {

    public virtual void Initialize(BulletBehaviour originalBehaviour, Bullet bullet) {

    }

    public virtual void Update() { }

    public abstract Vector2 GetDeltaMove(Bullet bullet);

    public abstract void OnCollision(Bullet bullet, Collider2D otherCollider);
}
