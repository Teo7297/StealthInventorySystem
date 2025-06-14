using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class responsible for catching input related to UI.
/// The player input component is set to SendMessage so the Player game object will 
/// receive the input commands
/// </summary>

public class UIInput : MonoBehaviour
{
    UIManager uiManager;
    PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
    }

    /// <summary>
    /// Input event for when the inventory button is pressed. Hide the HUD and show the inventory panel.
    /// </summary>
    private void OnInventory()
    {
        uiManager.ToggleInventory();
        playerInput.SwitchCurrentActionMap("UI");
    }

    /// <summary>
    /// Input event for when the cancel button is pressed. Close every panel and bring back the HUD.
    /// </summary>
    private void OnCancel()
    {
        uiManager.BackToHUD();
        playerInput.SwitchCurrentActionMap("Player");
    }
}