using UnityEngine;

public class Stamina : MonoBehaviour {

    [SerializeField] int maxStamina = 100;
    [SerializeField] int dashStamina = 33;
    [SerializeField] int restoreRate = 33;
    [SerializeField] float timeBeforeRestore = 0.5f;
    [SerializeField] UnityEngine.UI.Slider staminaBar = null;

    bool dashTrigger;
    float currentStamina;
    float lastDashTime;

    public static Stamina Instance { get; private set; }

    void Start() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
            return;
        }
        #endregion
        currentStamina = maxStamina;
    }

    void Update() {
        if (currentStamina < maxStamina && Time.time - lastDashTime >= timeBeforeRestore) {
            currentStamina = Mathf.Min(currentStamina + Time.deltaTime * restoreRate, maxStamina);
        }
        UpdateUi();
    }

    void UpdateUi() {
        staminaBar.value = currentStamina / maxStamina;
    }

    public void OnDashStart() {
        currentStamina -= dashStamina;
        lastDashTime = Time.time;
    }

    public bool CanDoDash() {
        return currentStamina - dashStamina >= 0f;
    }
}
