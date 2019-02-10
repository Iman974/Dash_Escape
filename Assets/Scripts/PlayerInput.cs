using UnityEngine;
using System;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour {

    private PlayerController playerController;

    private void Start() {
        playerController = GetComponent<PlayerController>();
    }

    private void Update() {
        playerController.PlayerInputX = Input.GetAxisRaw("Horizontal");
    }
}
