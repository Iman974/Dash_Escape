using UnityEngine;
using Slider = UnityEngine.UI.Slider;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] int maxStamina = 50;
    [SerializeField] LayerMask collisionLayer = default;
    [SerializeField] Slider staminaBar = null;
    [SerializeField] float staminaDecreaseInterval = 0.06f;

    private int stamina;
    private Rigidbody2D rb2D;
    private bool isFlying;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spRenderer;

    private void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        stamina = maxStamina;
        spRenderer = GetComponent<SpriteRenderer>();
        staminaBar.maxValue = maxStamina;
    }

    private void Update() {
        if (stamina >= 0) {
            staminaBar.value = stamina;
            if (Input.GetKey(KeyCode.Space)) {
                stamina -= 1;
                isFlying = true;
            } else if (Input.GetKeyUp(KeyCode.Space)) {
                isFlying = false;
                CancelInvoke();
            }
        }
        if (stamina <= 0) {
            isFlying = false;
        }

        if (!isFlying) {
            Bounds bounds = boxCollider.bounds;
            if (!Physics2D.OverlapBox(bounds.center, bounds.size, 0f, collisionLayer.value)) {
                // Falling in lava
                spRenderer.color = Color.red;
            } else {
                spRenderer.color = Color.cyan;
            }
        } else if (!IsInvoking()) {
            InvokeRepeating("DecreaseStamina", 0f, staminaDecreaseInterval);
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb2D.MovePosition(rb2D.position + (input * moveSpeed * Time.deltaTime));
    }
}
