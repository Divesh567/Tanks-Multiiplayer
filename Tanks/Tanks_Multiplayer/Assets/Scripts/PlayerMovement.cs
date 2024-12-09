using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField]
    private InputReader inputReader;
    [SerializeField]
    private Transform threadsTransform;
    [SerializeField]
    private Rigidbody2D tankRb;

    [SerializeField]
    private Transform tankGun;


    [Header("Settings")]
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float turningSpeed;

    private Vector2 previousMovementInput;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        inputReader.OnMovementButtonPressed += HandleMovement;
       // inputReader.OnFirePressed += HandleFireRotation;
    }

    private void HandleMovement(Vector2 moveVector)
    {
        previousMovementInput = moveVector;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        inputReader.OnMovementButtonPressed -= HandleMovement;
       // inputReader.OnFirePressed += HandleFireRotation;
    }

    private void HandleFireRotation(bool dofire, Vector2 firePos)
    {
       /* if (!dofire) return;

        Debug.Log($"Fire Direction Vector == {firePos}");

        float angle = Mathf.Atan2(firePos.y, firePos.x) * Mathf.Rad2Deg;

        tankGun.rotation = Quaternion.Euler(0, 0, angle - 90);*/
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (previousMovementInput.y == 0 && previousMovementInput.x == 0)
        {
            return;
        }

       /* float zrotation = previousMovementInput.x * -turningSpeed * Time.deltaTime;*/

       

        float angle = Mathf.Atan2(previousMovementInput.y, previousMovementInput.x) * Mathf.Rad2Deg;

        

        threadsTransform.rotation = Quaternion.Euler(0, 0, angle - 90);

       // threadsTransform.eulerAngles = new Vector3(0,0, zrotation);

        /*  // Get the horizontal and vertical input from the joystick
          float horizontalInput = previousMovementInput.x;
          float verticalInput = previousMovementInput.y;

          // Ensure the joystick has moved far enough to warrant rotation
          if (horizontalInput != 0 || verticalInput != 0)
          {
              // Calculate the angle in radians (between -π and π) and convert to degrees (between -180 and 180)
              float angle = Mathf.Atan2(verticalInput, horizontalInput) * Mathf.Rad2Deg;

              // Create the target rotation based on the angle
              Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);  // Subtract 90 to account for the forward direction of the player

              // Smoothly rotate the player to the target rotation
              transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);
          }*/

    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        if (previousMovementInput.y == 0 && previousMovementInput.x == 0)
        {
            tankRb.velocity = Vector2.zero;
            return;
        }
        
      
        tankRb.velocity = threadsTransform.transform.up * movementSpeed * Time.deltaTime;
    }

}
