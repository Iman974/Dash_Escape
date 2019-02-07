using UnityEngine;
using Slider = UnityEngine.UI.Slider;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    [SerializeField] float moveSpeed = 5f;
    //[SerializeField] int maxStamina = 50;
    [SerializeField] LayerMask collisionLayer = default;
    [SerializeField] Slider staminaBar = null;
    [SerializeField] float staminaDecreaseSpeed = 0.4f;
    [SerializeField] float staminaIncreaseSpeed = 0.6f;
    [SerializeField] float flightHeight = 0.5f;
    //[SerializeField] float takeoffSpeed = 1f;

    private float stamina = 1f;
    private Rigidbody2D rb2D;
    private bool isFlying;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spRenderer;
    //private float timeSinceFlightStart;
    //private Vector2 flightOffset;

    private void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (stamina >= 0) {
            staminaBar.value = stamina;
            if (Input.GetKeyDown(KeyCode.Space)) {
                isFlying = true;
                rb2D.position += Vector2.up * flightHeight;
            } else if (Input.GetKeyUp(KeyCode.Space)) {
                isFlying = false;
                rb2D.position -= Vector2.up * flightHeight;
                //timeSinceFlightStart = 0f;
            }
        }
        if (stamina <= 0) {
            isFlying = false;
        }

        if (!isFlying) {
            stamina = Mathf.Min(stamina + (staminaIncreaseSpeed * Time.deltaTime), 1f);
            Bounds bounds = boxCollider.bounds;
            if (!Physics2D.OverlapBox(bounds.center, bounds.size, 0f, collisionLayer.value)) {
                // Falling in lava
                spRenderer.color = Color.red;
            } else {
                spRenderer.color = Color.cyan;
            }
        } else {
            stamina = Mathf.Max(stamina - (staminaDecreaseSpeed * Time.deltaTime), 0f);
            //timeSinceFlightStart = Mathf.Max(timeSinceFlightStart + (Time.deltaTime * takeoffSpeed), 1f);
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb2D.MovePosition(rb2D.position + (input * moveSpeed * Time.deltaTime));
    }
}
