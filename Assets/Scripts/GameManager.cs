using UnityEngine;

public class GameManager : MonoBehaviour {

    static GameManager instance;

    void Awake() {
        #region Singleton
        if (instance == null) {
            instance = this;
        } else {
            Destroy(this);
            return;
        }
        #endregion

        InputUtility.Initialize();
    }
}
