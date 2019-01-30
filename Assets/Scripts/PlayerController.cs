using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 3.5f;
    [SerializeField] float timeToReachApex = 0.5f;
    //[SerializeField] float accelerationAirborne = 0.06f;
    [SerializeField] float accelerationGrounded = 0.02f;
    [SerializeField] LayerMask collisionLayer = default;

    private Vector2 velocity;
    private float jumpVelocity;
    private float gravity;
    private float smoothingVelocity;
    private float height;
    private bool isGrounded = true;
    private float yBeforeJump;
    private BoxCollider2D boxCollider;

    public float PlayerInputX { get; set; }

    private void Start() {
        PlayerInput.OnJumpKeyDownEvent += OnJumpKeyDown;
        boxCollider = GetComponent<BoxCollider2D>();

        gravity = -(2f * jumpHeight) / (timeToReachApex * timeToReachApex);
        jumpVelocity = Mathf.Abs(gravity) * timeToReachApex;
    }

    private void Update() {
        CalculateVelocity();

        Vector3 deltaMove = velocity * Time.deltaTime;
        if (!isGrounded) {
            height = transform.position.y + deltaMove.y - yBeforeJump;
            Bounds bounds = boxCollider.bounds;
            //bounds.center += Vector3.up * deltaMove.y;
            if (height < 0f && Physics2D.OverlapBox(bounds.center, bounds.size, 0f, collisionLayer.value) != null) {
                isGrounded = true;
            }
        }

        if (isGrounded) {
            velocity.y = 0f;
            //height = 0f;
        }

        transform.position += deltaMove;
    }

    private void CalculateVelocity() {
        velocity.x = Mathf.SmoothDamp(velocity.x, PlayerInputX * moveSpeed, ref smoothingVelocity, accelerationGrounded);
        velocity.y += gravity * Time.deltaTime;
    }

    private void OnJumpKeyDown() {
        if (isGrounded) {
            velocity.y = jumpVelocity;
            isGrounded = false;
            yBeforeJump = transform.position.y;
        }
    }

    private void OnDestroy() {
        PlayerInput.OnJumpKeyDownEvent -= OnJumpKeyDown;
    }
}
