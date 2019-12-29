using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] float moveSpeed = 2f;
    [SerializeField] int damage = 10;
    [SerializeField] BulletBehaviour bulletBehaviour = null;

    Rigidbody2D rb2D;
    public float MoveSpeed => moveSpeed;
    public int Damage => damage;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        BulletBehaviour original = bulletBehaviour;
        bulletBehaviour = (BulletBehaviour)ScriptableObject.CreateInstance(bulletBehaviour.GetType());
        bulletBehaviour.Initialize(original, this);
    }

    void Update() {
        bulletBehaviour.Update();
        Vector2 deltaMove = bulletBehaviour.GetDeltaMove(this);
        Debug.DrawLine(rb2D.position, rb2D.position + deltaMove, Color.white, 2f);
        rb2D.MovePosition(rb2D.position + deltaMove);
    }

    void OnTriggerEnter2D(Collider2D otherCollider) {
        bulletBehaviour.OnCollision(this, otherCollider);
    }
}
