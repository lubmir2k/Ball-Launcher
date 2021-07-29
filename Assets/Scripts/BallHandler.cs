using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Rigidbody2D pivot;
    [SerializeField] float detachDelay = 0.15f;
    [SerializeField] float respawnDelay = 1f;

    Rigidbody2D currentBallRigidbody;
    SpringJoint2D currentBallSpringjoint;

    Camera mainCamera;
    bool isDragging;

    void Start()
    {
        mainCamera = Camera.main;
        SpawnBall();
    }

    void Update()
    {
        if (currentBallRigidbody == null)
            return;

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if(isDragging)
            {
                LaunchBall();
            }

            isDragging = false;

            return;
        }

        isDragging = true;

        currentBallRigidbody.isKinematic = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidbody.position = worldPosition;

    }

    void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;

        Invoke(nameof(DetachBall), detachDelay);
    }

    void DetachBall()
    {
        currentBallSpringjoint.enabled = false;
        currentBallSpringjoint = null;

        Invoke(nameof(SpawnBall), respawnDelay);
    }

    void SpawnBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringjoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringjoint.connectedBody = pivot;
    }
}
