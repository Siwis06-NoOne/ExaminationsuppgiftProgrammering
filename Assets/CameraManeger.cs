using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManeger : MonoBehaviour
{
    InputManeger inputManeger;

    public Transform targetTransform;   // the object the camera will follow
    public Transform cameraPivot;      // the object the caera will focut on (Up and Down)
    public Transform cameraTransform; // the transform of the actual camrea object in the scene
    public LayerMask collisionLayers;// the layers we want our camera to colide with
    private float defaultPosision;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    public float camreaCollisionOffSet = 0.2f; // how mutch the camera will jump of object it's colliding with
    public float minimumCollisionOffSet = 0.2f;
    public float camreaCollisionRadius = 2;
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;

    public float lookAngle; // Camera look Up and Down
    public float pivotAngle; // Camera look Left and Right
    public float minimumPivotAngle = -35;
    public float maximumPivotAngle = 35;

    private void Awake()
    {
        inputManeger = FindObjectOfType<InputManeger>();
        targetTransform = FindObjectOfType<PlayerManeger>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosision = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowPlayer();
        RotateCamera();
        HandleCameraCollisions();
    }

    private void FollowPlayer()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle = lookAngle + (inputManeger.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManeger.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions()
    {
        float targetPosision = defaultPosision;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, camreaCollisionRadius, direction, out hit, Mathf.Abs(targetPosision), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosision =- (distance - camreaCollisionOffSet);
        }

        if(Mathf.Abs(targetPosision) < minimumCollisionOffSet)
        {
            targetPosision = targetPosision - minimumCollisionOffSet;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosision, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
