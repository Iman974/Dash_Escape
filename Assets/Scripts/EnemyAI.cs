using UnityEngine;

public class EnemyAI : MonoBehaviour {

    [SerializeField] float minChaseDistance = 4.5f;
    [SerializeField] float chaseSpeed = 2f;
    [SerializeField] int damage = 4;
    [SerializeField] float damageRepetitionRate = 0.5f;

    Transform player;
    bool isChasingPlayer;
    Health playerHealth;
    float timeInContact;
    bool hasChaseStartedOnce;

    void Start() {
        player = GameObject.FindWithTag("Player").transform;
        playerHealth = player.GetComponent<Health>();
    }

    void Update() {
        if (isChasingPlayer) {
            transform.position = Vector2.MoveTowards(transform.position, player.position,
                chaseSpeed * Time.deltaTime);
        } else if (!hasChaseStartedOnce && (player.position - transform.position).sqrMagnitude <=
                  minChaseDistance * minChaseDistance) {
            isChasingPlayer = true;
            hasChaseStartedOnce = true;
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider) {
        if (otherCollider.gameObject.CompareTag("Player")) {
            playerHealth.TakeDamage(damage);
            isChasingPlayer = false;
        }
    }

    void OnTriggerStay2D(Collider2D otherCollider) {
        if (otherCollider.gameObject.CompareTag("Player")) {
            timeInContact += Time.deltaTime;
            if (timeInContact >= damageRepetitionRate) {
                playerHealth.TakeDamage(damage);
                timeInContact = 0f;
            }
        }
    }

    void OnTriggerExit2D(Collider2D otherCollider) {
        if (otherCollider.gameObject.CompareTag("Player")) {
            timeInContact = 0f;
            isChasingPlayer = true;
        }
    }
}
