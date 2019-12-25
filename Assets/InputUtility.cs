using UnityEngine;

public static class InputUtility {

    static Camera mainCamera;

    public static void Initialize() {
        mainCamera = Camera.main;
    }

    public static Vector2 GetWorldMousePosition() {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
