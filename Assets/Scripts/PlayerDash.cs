using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] float dashSpeed = 1f;
    [SerializeField] float dashDistance = 2f;
    [SerializeField] float circleCastOffset = 0.01f;

    private Rigidbody2D rb2D;
    private Camera mainCamera;
    private float velocity;
    private Vector2 destination;
    private bool doPositionCheck;
    private float colliderRadius;
    private Vector2 remainingMovement;

    private void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        velocity = dashDistance / dashSpeed;
        colliderRadius = GetComponent<CircleCollider2D>().radius * transform.localScale.x;
        destination = rb2D.position;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Dash();
        }
        rb2D.MovePosition(Vector2.MoveTowards(rb2D.position, destination, velocity * Time.deltaTime));
    }

    private void FixedUpdate() {
        if (doPositionCheck) {
            if (rb2D.position != destination) {
                return;
            }

            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.CircleCast(rb2D.position, colliderRadius, remainingMovement, remainingMovement.magnitude);
            if (hit) {
                destination = hit.centroid;
                remainingMovement = rb2D.position - hit.centroid + remainingMovement;
                remainingMovement = Vector2.Reflect(remainingMovement, hit.normal);
            } else {
                destination += remainingMovement;
                doPositionCheck = false;
            }
        }
    }

    private void Dash() {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dashDirection = (mousePosition - rb2D.position).normalized;
        RaycastHit2D hit = Physics2D.CircleCast(rb2D.position - (dashDirection * circleCastOffset),
            colliderRadius, dashDirection, dashDistance + circleCastOffset);
        if (hit) {
            destination = hit.centroid;
            remainingMovement = rb2D.position - hit.centroid + (dashDirection * dashDistance);
            remainingMovement = Vector2.Reflect(remainingMovement, hit.normal);
            doPositionCheck = true;
        } else {
            destination = rb2D.position + (dashDirection * dashDistance);
        }
    }
}
