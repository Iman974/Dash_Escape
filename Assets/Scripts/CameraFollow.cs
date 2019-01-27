using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] float smoothTime = 0.1f;

    private Rigidbody2D player;
    private Vector3 startOffset;
    private Vector2 smoothVelocity;

    private void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        startOffset = new Vector3(0f, 0f, -10f);
    }

    private void LateUpdate() {
        Vector3 newPosition = Vector2.SmoothDamp(transform.position, player.position, ref smoothVelocity, smoothTime);
        transform.position = newPosition + startOffset;
    }
}
