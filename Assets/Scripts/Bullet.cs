using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] float moveSpeed = 2f;
    [SerializeField] int damage = 10;

    Rigidbody2D rb2D;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update() {
        rb2D.MovePosition(rb2D.position + (Vector2)transform.right * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D otherCollider) {
        Health health = otherCollider.GetComponent<Health>();
        if (health != null) {
            health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
