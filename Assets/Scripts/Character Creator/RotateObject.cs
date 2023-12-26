using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    PlayerControls playerControls;

    public float rotationAmount = 1;
    public float rotationSpeed = 5;

    public bool rotate;

    Vector2 cameraInput;

    Vector3 currentRotation;
    Vector3 targetRotation;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        }

        playerControls.Enable();
    }

    private void Start()
    {
        currentRotation = transform.eulerAngles;
        targetRotation = transform.eulerAngles;
    }

    private void Update()
    {
        if (rotate)
        {
            if (cameraInput.x > 0)
            {
                targetRotation.y = targetRotation.y - rotationAmount;
            }
            else if (cameraInput.x < 0)
            {
                targetRotation.y = targetRotation.y + rotationAmount;
            }

            currentRotation = Vector3.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = currentRotation;
        }
        else
        {
            targetRotation.y = 0;

            currentRotation = Vector3.Lerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = currentRotation;
        }
    }

    public void _RotateButtonOn()
    {
        rotate = true;
    }

    public void _RotateButtonOff()
    {
        rotate = false;
    }
}
