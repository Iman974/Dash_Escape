using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField] int maxHealth = 100;

    int health;

    void Start() {
        health = maxHealth;
    }

    public void TakeDamage(int amount) {
        health -= amount;

        if (health <= 0) {
            // Die
        }
    }
}
