using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This is an empty class that simplifies the components required by the player game object.
/// </summary>

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInteraction))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(UIInput))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{

}