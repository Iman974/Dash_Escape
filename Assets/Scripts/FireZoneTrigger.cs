using UnityEngine;

public class FireZoneTrigger : MonoBehaviour {

    ShootingUnit shootingUnit;

    void Start() {
        shootingUnit = GetComponentInParent<ShootingUnit>();
        UnityEngine.Assertions.Assert.IsNotNull(shootingUnit, "Shooting Unit was not found");
    }

    void OnTriggerEnter2D(Collider2D otherCollider) {
        shootingUnit.IsRotationActive = true;
    }

    void OnTriggerExit2D(Collider2D collision) {
        shootingUnit.IsRotationActive = false;
    }
}
