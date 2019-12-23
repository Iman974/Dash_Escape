using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] float smoothTime = 0.25f;

    Rigidbody2D player;
    Vector3 startOffset;
    Vector2 smoothVelocity;

    void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        startOffset = new Vector3(0f, 0f, -10f);
    }

    void LateUpdate() {
        Vector3 newPosition = Vector2.SmoothDamp(transform.position, player.position, ref smoothVelocity, smoothTime);
        transform.position = newPosition + startOffset;
    }
}
