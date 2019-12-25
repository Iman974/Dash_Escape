using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float dashDistance = 4f;
    [SerializeField] float dashDuration = 0.14f;
    [SerializeField] float minDashDistance = 0.05f;

    Rigidbody2D rb2D;
    Controller2D controller2D;
    Camera mainCamera;

    Vector2 input;
    bool isDashing;
    Vector2 dashDirection;
    Vector2 dashStartPosition;
    float remainingDashDistance;
    float dashVelocity;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        controller2D = GetComponent<Controller2D>();
        mainCamera = Camera.main;

        dashVelocity = dashDistance / dashDuration;
    }

    void Update() {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0)) {
            InitDash();
        }

        if (!isDashing) {
            controller2D.Move(input * moveSpeed * Time.deltaTime);
        } else {
            controller2D.Move(dashDirection * dashVelocity * Time.deltaTime);
            HandleDash();
        }
    }

    void InitDash() {
        dashStartPosition = transform.position;
        remainingDashDistance = dashDistance;
        dashDirection = (InputUtility.GetWorldMousePosition() - dashStartPosition).normalized;
        controller2D.CastAllRays(dashDirection * minDashDistance);
        var collisions = controller2D.collisions;
        isDashing = !(collisions.hitX || collisions.hitY);
    }

    void HandleDash() {
        if ((dashStartPosition - (Vector2)transform.position).sqrMagnitude >=
                remainingDashDistance * remainingDashDistance) {
            isDashing = false;
        }

        RaycastHit2D hit = controller2D.collisions.GetAnyHit();
        if (hit) {
            Wall wall = hit.collider.GetComponent<Wall>();
            if (wall.DashCollision == Wall.DashReaction.Bounce) {
                dashDirection = Vector2.Reflect(dashDirection, hit.normal);
                remainingDashDistance -= (dashStartPosition - (Vector2)transform.position).magnitude;
                dashStartPosition = transform.position;
            } else if (wall.DashCollision == Wall.DashReaction.Stop) {
                isDashing = false;
            }
        }
    }
}
