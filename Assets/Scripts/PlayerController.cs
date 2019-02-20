using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerController : MonoBehaviour {

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;
    //[SerializeField] float accelerationAirborne = 0.06f;
    //[SerializeField] float accelerationGrounded = 0.02f;
    //[SerializeField] float horizontalKnockback = 0.1f;
    [SerializeField] [Range(0f, 1f)] float aboveFriction = 0.1f;
    [SerializeField] float groundFriction = 0.5f;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 3.5f;
    [SerializeField] float timeToReachApex = 0.5f;
    [SerializeField] float jumpDelay = 0.05f;

    [Header("Dash")]
    //[SerializeField] float boxCastOffset = 0.01f;
    //[SerializeField] float dashDistance = 2f;
    [SerializeField] float baseDashImpulse = 30f;
    [SerializeField] float maxDashForce = 25f;
    [SerializeField] [Range(0f, 1f)] float loadingMoveSpeed = 0.05f;
    [SerializeField] Vector2 influence = new Vector2(0.1f, 0f);
    [SerializeField] AnimationCurve dashToInputTransition = AnimationCurve.Linear(0f, 0f, 1f, 1f);

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
    float dashTimer;

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
            velocity.x = -velocity.x * collisions.horizontalCollider.bounciness;
        }

        if (jumpTrigger && collisions.below) {
            velocity.y = jumpVelocity;
        }
        float deltaTime = Time.deltaTime;
        dashTimer += deltaTime;
        CalculateVelocity(collisions.below);

        controller.Move(velocity * deltaTime * movementTimeScale);
    }

    private void CalculateVelocity(bool isCollidingBelow) {
        //float smoothTime = isCollidingBelow ? accelerationGrounded : accelerationAirborne;
        //Mathf.SmoothDamp(velocity.x, PlayerInputX * moveSpeed, ref smoothingVelocity, smoothTime)
        float transitionState = 0f;
        if (Mathf.Abs(PlayerInputX) > 0f) {
            transitionState = dashToInputTransition.Evaluate(dashTimer);
        }
        velocity.x = Mathf.Lerp(velocity.x, PlayerInputX * moveSpeed, transitionState);
        Debug.Log($"transition: {transitionState}, velocityX: {velocity.x}");
        velocity.y += gravity * Time.deltaTime;

        if (Input.GetMouseButton(0)) {
            dashLoadingTime += Time.deltaTime;
            movementTimeScale = loadingMoveSpeed;
        } else if (Input.GetMouseButtonUp(0)) {
            dashTimer = 0f;
            velocity *= influence;
            velocity += Dash(dashLoadingTime * baseDashImpulse);
            dashLoadingTime = 0f;
            movementTimeScale = 1f;
        }
    }

    private Vector2 Dash(float dashDistance) {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dashDirection = (mousePosition - (Vector2)transform.position).normalized;

        return Vector2.ClampMagnitude(dashDirection * dashDistance, maxDashForce);
    }
}
