using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthBar = null;

    int healthPoints;

    void Start() {
        healthPoints = maxHealth;
        healthBar.value = maxHealth;
    }

    public void TakeDamage(int amount) {
        healthPoints -= amount;
        healthBar.value = healthPoints / (float)maxHealth;

        if (healthPoints <= 0) {
            Debug.Log("You died!");
        }
    }
}
