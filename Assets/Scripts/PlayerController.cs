using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerController : MonoBehaviour {

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 3.5f;
    [SerializeField] private float timeToReachApex = 0.5f;
    [SerializeField] private float jumpDelay = 0.05f;
    [SerializeField] private float accelerationAirborne = 0.06f;
    [SerializeField] private float accelerationGrounded = 0.02f;

    private Vector2 velocity;
    private Controller2D controller;
    private float jumpVelocity;
    private float gravity;
    private float smoothingVelocity;
    private bool jumpTrigger;
    private float jumpTriggeredTime;

    public float PlayerInputX { get; set; }

    private void Start() {
        controller = GetComponent<Controller2D>();

        //PlayerInput.OnJumpKeyDownEvent += OnJumpKeyDown;

        gravity = -(2f * jumpHeight) / (timeToReachApex * timeToReachApex);
        jumpVelocity = Mathf.Abs(gravity) * timeToReachApex;
    }

    private void Update() {
        if (!jumpTrigger || Time.time - jumpTriggeredTime >= jumpDelay) {
            jumpTrigger = Input.GetKeyDown(KeyCode.Space);
            jumpTriggeredTime = Time.time;
        }

        if (controller.IsCollidingBelow || controller.IsCollidingAbove) {
            velocity.y = 0f;
        }

        if (jumpTrigger && controller.IsCollidingBelow) {
            velocity.y = jumpVelocity;
        }

        CalculateVelocity();

        controller.Move(velocity * Time.deltaTime);
    }

    private void CalculateVelocity() {
        float smoothTime = controller.IsCollidingBelow ? accelerationGrounded : accelerationAirborne;
        velocity.x = Mathf.SmoothDamp(velocity.x, PlayerInputX * moveSpeed, ref smoothingVelocity, smoothTime);
        velocity.y += gravity * Time.deltaTime;
    }

    //private void OnJumpKeyDown() {
    //    if (controller.IsCollidingBelow) {
    //        velocity.y = jumpVelocity;
    //    }
    //}

    private void OnDestroy() {
        //PlayerInput.OnJumpKeyDownEvent -= OnJumpKeyDown;
    }
}
