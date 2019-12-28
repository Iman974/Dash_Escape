using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float dashDistance = 4f;
    [SerializeField] float dashDuration = 0.14f;
    [SerializeField] float minDashDistance = 0.05f;
    [SerializeField] AnimationCurve dashCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

    Rigidbody2D rb2D;
    Controller2D controller2D;
    Camera mainCamera;

    Vector2 input;
    bool isDashing;
    Vector2 dashDirection;
    Vector2 dashStartPosition;
    float remainingDashDistance;
    float dashVelocity;
    float dashStartTime;

    public bool IsDashing => isDashing;

    public int vsync = 0;
    public int fps = 60;
    public bool setSettings;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        controller2D = GetComponent<Controller2D>();
        mainCamera = Camera.main;

        dashVelocity = dashDistance / dashDuration;
        if (setSettings) {
            QualitySettings.vSyncCount = vsync;
            Application.targetFrameRate = fps;
        }
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
            float traveledDashSqrDistance = (dashStartPosition - (Vector2)transform.position).sqrMagnitude;
            float t = traveledDashSqrDistance / (dashDistance * dashDistance);
            controller2D.Move(dashCurve.Evaluate(t) * dashDirection * dashVelocity * Time.deltaTime);
            HandleDash();
        }
    }

    void InitDash() {
        if (!Stamina.Instance.CanDoDash()) {
            return;
        }
        Stamina.Instance.OnDashStart();
        dashStartPosition = transform.position;
        remainingDashDistance = dashDistance;
        dashDirection = (InputUtility.GetWorldMousePosition() - dashStartPosition).normalized;
        bool hit = controller2D.CastAllRays(dashDirection * minDashDistance);
        if (!hit) {
            isDashing = true;
            dashStartTime = Time.time;
        }
    }

    void HandleDash() {
        float traveledDashSqrDistance = (dashStartPosition - (Vector2)transform.position).sqrMagnitude;
        if (traveledDashSqrDistance >= remainingDashDistance * remainingDashDistance) {
            isDashing = false;
        }

        RaycastHit2D hit = controller2D.collisions.latestHit;
        if (hit) {
            IDashable dashable = hit.transform.GetComponentInParent<IDashable>();
            if (dashable.DashReaction == DashReaction.Bounce) {
                dashDirection = Vector2.Reflect(dashDirection, hit.normal);
                remainingDashDistance -= (dashStartPosition - (Vector2)transform.position).magnitude;
                dashStartPosition = transform.position;
            } else if (dashable.DashReaction == DashReaction.Stop) {
                isDashing = false;
            }
        }
    }
}
