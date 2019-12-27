using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] float moveSpeed = 2f;

    void Update() {
        transform.position += transform.right * moveSpeed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}
