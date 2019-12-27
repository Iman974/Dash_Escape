using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] float moveSpeed = 2f;

    void Update() {
        transform.position += transform.right * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D otherCollider) {
        Destroy(gameObject);
    }
}
