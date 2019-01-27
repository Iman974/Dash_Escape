using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField] int maxHealth = 100;

    private int health;

    private void Start() {
        health = maxHealth;
    }

    public void TakeDamage(int amount) {
        health -= amount;

        if (health <= 0) {
            // Die
        }
    }
}
