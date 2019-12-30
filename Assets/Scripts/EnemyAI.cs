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
    Controller2D controller;

    void Start() {
        player = GameObject.FindWithTag("Player").transform;
        playerHealth = player.GetComponent<Health>();
        controller = GetComponent<Controller2D>();
    }

    void Update() {
        if (isChasingPlayer) {
            Vector2 deltaMove = (player.position - transform.position).normalized * chaseSpeed * Time.deltaTime;
            controller.Move(deltaMove);

            RaycastHit2D hit = controller.collisions.latestHit;
            if (!hit) {
                timeInContact = damageRepetitionRate;
                return;
            }
            if (hit.transform.CompareTag("Player")) {
                timeInContact += Time.deltaTime;
                if (timeInContact >= damageRepetitionRate) {
                    playerHealth.TakeDamage(damage);
                    timeInContact = 0f;
                }
            }
        } else if (!hasChaseStartedOnce && (player.position - transform.position).sqrMagnitude <=
                  minChaseDistance * minChaseDistance) {
            isChasingPlayer = true;
            hasChaseStartedOnce = true;
        }
    }
}
