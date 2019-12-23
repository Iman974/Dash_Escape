using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] RaycastController raycastController;

    Vector2 input;
    Rigidbody2D rb2D;
    Controller2D controller2D;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        controller2D = GetComponent<Controller2D>();
    }

    void Update() {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input *= moveSpeed * Time.deltaTime;
        controller2D.Move(input);
    }
}
