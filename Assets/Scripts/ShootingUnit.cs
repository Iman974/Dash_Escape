using UnityEngine;

public class ShootingUnit : MonoBehaviour, IDashable {

    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] Transform structure = null;
    [SerializeField] Transform firePoint = null;
    [SerializeField] Transform bullet = null;
    [SerializeField] float fireRate = 1f;

    public DashReaction DashReaction => DashReaction.Bounce;
    public bool IsRotationActive { get; set; }

    Transform playerTransform;
    float angularVelocity;
    float lastFireTime = -9f;

    void Start() {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update() {
        if (!IsRotationActive) {
            return;
        }
        float angle = Vector2.SignedAngle(structure.right, playerTransform.position - structure.position);
        angularVelocity = angle * rotationSpeed;
        structure.Rotate(0f, 0f, angularVelocity * Time.deltaTime);
        if (Time.time - lastFireTime >= fireRate) {
            lastFireTime = Time.time;
            Shoot();
        }
    }

    void Shoot() {
        Transform bulletInstance = Instantiate(bullet, firePoint.position, Quaternion.identity);
        bulletInstance.right = firePoint.position - transform.position;
    }
}
