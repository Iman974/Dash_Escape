using UnityEngine;
using System;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour {

    private PlayerController playerController;

    public static event Action OnJumpKeyDownEvent;

    private void Start() {
        playerController = GetComponent<PlayerController>();
    }

    private void Update() {
        playerController.PlayerInputX = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && OnJumpKeyDownEvent != null) {
            OnJumpKeyDownEvent();
        }
    }
}
