using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManeger : MonoBehaviour
{
    InputManeger inputManeger;
    CameraManeger cameraManeger;
    PlayerLocomotion playerLocomotion;

    private void Awake()
    {
        inputManeger = GetComponent<InputManeger>();
        cameraManeger = FindAnyObjectByType<CameraManeger>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        inputManeger.HandleAllInputs();
    }
    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManeger.HandleAllCameraMovement();
        //playerLocomotion.isJumping
    }
}
