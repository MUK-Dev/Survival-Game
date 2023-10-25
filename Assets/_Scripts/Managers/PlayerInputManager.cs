using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    //* Handles player input by unity new input system
    public static PlayerInputManager Instance { get; private set; }

    private PlayerInputActions inputActions;

    private void Awake()
    {
        Instance = this;
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
    }

    //* Returns input vector of player
    public Vector3 GetMovementVector()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        Vector3 input = new Vector3(inputVector.x, 0, inputVector.y);

        return input.normalized;
    }

    //* Returns mouse position
    public Vector2 GetMousePosition()
    {
        return inputActions.Player.MousePosition.ReadValue<Vector2>();
    }

    //* Checks if player move or not
    public bool CanMove() => inputActions.Player.Move.inProgress;
}
