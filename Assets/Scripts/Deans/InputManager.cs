using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static GameControls _gameControls;

    public static void Init(Player myPlayer)
    {
        _gameControls = new GameControls();

        _gameControls.Permanent.Enable();

        _gameControls.InGame.Movement.performed += ctx =>
        {
            myPlayer.SetMovementDirection(ctx.ReadValue<Vector2>());
        };

        _gameControls.InGame.Jump.performed += ctx =>
        {
            myPlayer.PlayerJump();
        };

        _gameControls.InGame.Crouch.performed += ctx =>
        {
            myPlayer.PlayerCrouch();
        };

        _gameControls.InGame.Block.started += ctx =>
        {
            myPlayer.ToggleBlock();
        };

        _gameControls.InGame.Block.canceled += ctx =>
        {
            myPlayer.StopBlocking();
        };

        _gameControls.InGame.Inhale.started += ctx =>
        {
            myPlayer.Inhale();
        };

        _gameControls.InGame.Inhale.canceled += ctx =>
        {
            myPlayer.StopInhaling();
        };

        _gameControls.InGame.SpitOut.performed += ctx =>
        {
            myPlayer.SpitOut();
        };
        _gameControls.Permanent.Enable();
    }

    public static void SetGameControls()
    {
        _gameControls.InGame.Enable();
        _gameControls.UI.Disable();
    }

    public static void SetUIControls()
    {
        _gameControls.InGame.Enable();
        _gameControls.UI.Disable();
    }
}
