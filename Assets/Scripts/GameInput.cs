using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameInput : MonoBehaviour
{

    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerDash;
    public static GameInput Instance { get; private set; }
     private PlayerInputActions    _playerInputActions;
    private void Awake()
    {
        Instance = this;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();

        _playerInputActions.Combat.Attack.started+= PlayerAttack_started; // при нажатии started
        _playerInputActions.Player.Dash.performed += PlayerDash_performed; // после того как отпустили performed
    }

    private void PlayerDash_performed(InputAction.CallbackContext obj)
    {
        OnPlayerDash?.Invoke(this, EventArgs.Empty);
    }
        public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }
    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        Debug.Log("Attack input detected");
   
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
    public Vector2 GetMousePosition() {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }

    public void DisableMovement()
    {
        _playerInputActions.Disable();
    }
}
