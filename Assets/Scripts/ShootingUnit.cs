using UnityEngine;

public class ShootingUnit : MonoBehaviour, IDashable {

    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] Transform structure = null;

    public DashReaction DashReaction => DashReaction.Bounce;

    Transform playerTransform;
    float angularVelocity;
    bool rotationActive;

    void Start() {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void OnTriggerEnter2D(Collider2D otherCollider) {
        rotationActive = true;
    }

    void Update() {
        if (!rotationActive) {
            return;
        }
        float angle = Vector2.SignedAngle(structure.right, playerTransform.position - structure.position);
        angularVelocity = angle * rotationSpeed;
        structure.Rotate(0f, 0f, angularVelocity * Time.deltaTime);
    }

    void OnTriggerExit2D(Collider2D collision) {
        rotationActive = false;
    }
}
