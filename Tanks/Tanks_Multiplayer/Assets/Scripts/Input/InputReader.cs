using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;


[CreateAssetMenu(fileName = "InputReader", menuName = ("Input/New Input Reader"))]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<bool,Vector2> OnFirePressed;
    public event Action<bool> OnFireCancledEvent;

    public event Action<Vector2> OnMovementButtonPressed;
    public event Action<Vector2> OnWeaponRotated;

    public Vector2 aimPosition;

    private PlayerControls inputActions;



    private void OnEnable()
    {
        if(inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.Player.SetCallbacks(this);
        }

        inputActions.Player.Enable();
    }

    public void OnFireWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 screenPos = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
            OnFirePressed?.Invoke(true , screenPos);
        }
       
    }


    public void OnMovement(InputAction.CallbackContext context)
    {
        OnMovementButtonPressed?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnAiming(InputAction.CallbackContext context)
    {
        OnWeaponRotated?.Invoke(context.ReadValue<Vector2>());
        aimPosition = context.ReadValue<Vector2>();
    }
}
