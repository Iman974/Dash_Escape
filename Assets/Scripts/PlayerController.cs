using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] float moveSpeed = 6f;
    [SerializeField] RaycastController raycastController;

    Vector2 input;
    Rigidbody2D rb2D;
    Controller2D controller2D;
    bool isDashing;
    Camera mainCamera;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        controller2D = GetComponent<Controller2D>();
        mainCamera = Camera.main;
    }

    void Update() {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0)) {
            isDashing = true;
            controller2D.InitDash();
        }

        if (!isDashing) {
            controller2D.Move(input * moveSpeed * Time.deltaTime);
        } else {
            isDashing = controller2D.Dash();
        }
    }
}
