using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerController : MonoBehaviour {

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float accelerationAirborne = 0.06f;
    [SerializeField] float accelerationGrounded = 0.02f;
    [SerializeField] float horizontalKnockback = 0.1f;
    [SerializeField] [Range(0f, 1f)] float aboveFriction = 0.1f;
    [SerializeField] float groundFriction = 0.02f;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 3.5f;
    [SerializeField] float timeToReachApex = 0.5f;
    [SerializeField] float jumpDelay = 0.05f;

    [Header("Dash")]
    //[SerializeField] float boxCastOffset = 0.01f;
    //[SerializeField] float dashDistance = 2f;
    [SerializeField] float baseDashImpulse = 30f;
    [SerializeField] float maxDashForce = 25f;
    [SerializeField] [Range(0f, 1f)] float dashSlowDown = 0.1f;
    [SerializeField] [Range(0f, 1f)] float xInfluence = 0.1f;
    [SerializeField] [Range(0f, 1f)] float yInfluence = 0f;

    Vector2 velocity;
    Controller2D controller;
    float jumpVelocity;
    float gravity;
    float smoothingVelocity;
    bool jumpTrigger;
    float jumpTriggeredTime;
    Camera mainCamera;
    Vector2 remainingMovement;
    Vector2 boxColliderSize;
    float dashLoadingTime;
    float movementTimeScale = 1f;
    float oppositeFriction;

    public float PlayerInputX { get; set; }

    private void Start() {
        controller = GetComponent<Controller2D>();
        mainCamera = Camera.main;

        gravity = -(2f * jumpHeight) / (timeToReachApex * timeToReachApex);
        jumpVelocity = Mathf.Abs(gravity) * timeToReachApex;
        boxColliderSize = GetComponent<BoxCollider2D>().size;
    }

    private void Update() {
        if (!jumpTrigger || Time.time - jumpTriggeredTime >= jumpDelay) {
            jumpTrigger = Input.GetKeyDown(KeyCode.Space);
            jumpTriggeredTime = Time.time;
        }

        Controller2D.CollisionsInfo collisions = controller.Collisions;
        if (collisions.below) {
            velocity.y = 0f;

            oppositeFriction = Mathf.Sign(-velocity.x) * groundFriction * Mathf.Abs(velocity.x);
            float result = velocity.x + oppositeFriction;
            velocity.x = (int)Mathf.Sign(velocity.x) != (int)Mathf.Sign(result) ? 0f : result;
        } else if (collisions.above) {
            velocity.y = 0f;
            velocity.x *= (1f - aboveFriction);
        }
        if (collisions.right || collisions.left) {
            velocity.x = -velocity.x * horizontalKnockback;
        }

        if (jumpTrigger && collisions.below) {
            velocity.y = jumpVelocity;
        }

        CalculateVelocity(collisions.below);

        controller.Move(velocity * Time.deltaTime * movementTimeScale);
    }

    private void CalculateVelocity(bool isCollidingBelow) {
        float smoothTime = isCollidingBelow ? accelerationGrounded : accelerationAirborne;
        //velocity.x = Mathf.SmoothDamp(velocity.x, PlayerInputX * moveSpeed, ref smoothingVelocity, smoothTime);
        velocity.y += gravity * Time.deltaTime;

        if (Input.GetMouseButton(0)) {
            dashLoadingTime += Time.deltaTime;
            movementTimeScale = dashSlowDown;
        } else if (Input.GetMouseButtonUp(0)) {
            velocity.x *= xInfluence;
            velocity.y *= yInfluence;
            velocity += Dash(dashLoadingTime * baseDashImpulse);
            dashLoadingTime = 0f;
            movementTimeScale = 1f;
        }
    }

    private Vector2 Dash(float dashDistance) {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 position = transform.position;
        Vector2 dashDirection = (mousePosition - position).normalized;
        //RaycastHit2D hit = Physics2D.BoxCast(position - (dashDirection * boxCastOffset),
        //    boxColliderSize, 0f, dashDirection, this.dashDistance + boxCastOffset);
        if (false) {
            //destination = hit.centroid;
            //remainingMovement = position - hit.centroid + (dashDirection * dashDistance);
            //remainingMovement = Vector2.Reflect(remainingMovement, hit.normal);
        } else {
            return Vector2.ClampMagnitude(dashDirection * dashDistance, maxDashForce);
        }
    }
}
