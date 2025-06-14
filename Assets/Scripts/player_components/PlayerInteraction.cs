using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class responsible for player interactions with items in the scene.
/// </summary>

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private float maxInteractDistance = 10f;

    private PlayerInput playerInput;
    private Inventory inventory;

    private Pickup selectedPickup;
    private bool isPickupSelected = false;

    // EVENTS
    public delegate void InteractInfoTextDelegate(string message);
    /// <summary>
    /// This event is invoked when an item is directly in front of the character. The info text contains a message telling the player to press a button to pickup the item.
    /// </summary>
    public event InteractInfoTextDelegate OnInteractInfoText;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inventory = GetComponent<Inventory>();

        if (playerInput == null || inventory == null)
            Debug.LogError("[PlayerInteraction] Could not initialize PlayerInteraction required components!");
    }

    private void Update()
    {
        CheckForInteractable();
    }

    /// <summary>
    /// Check if there is an interactable item in front of the player.
    /// </summary>
    private void CheckForInteractable()
    {
        // Check if the camera is pointing an interactable item, if so try to obtain it
        Transform cameraXForm = Camera.main.transform;
        if (Physics.Raycast(cameraXForm.position, cameraXForm.forward, out RaycastHit hit, maxInteractDistance))
        {
            Pickup pickup = hit.transform.GetComponentInParent<Pickup>();
            if (pickup)
            {
                // Weird unity stuff, we need the key name. We just take the first that should be the keyboard key, we should instead iterate over all bindings, check for the currently used input device and get that key name.
                var interactBinding = playerInput.actions["Interact"].bindings[0];
                string interactKeyName = InputControlPath.ToHumanReadableString(interactBinding.path, InputControlPath.HumanReadableStringOptions.OmitDevice);

                OnInteractInfoText("Press [" + interactKeyName + "] for " + pickup.GetItemName());
                isPickupSelected = true;
                selectedPickup = pickup;
            }
        }
        else
        {
            OnInteractInfoText("");
            isPickupSelected = false;
            selectedPickup = null;
        }
    }

    /// <summary>
    /// Interact input event.
    /// Used to pick and item up.
    /// </summary>
    private void OnInteract()
    {
        if (!isPickupSelected || selectedPickup == null)
            return;

        bool success = selectedPickup.TryObtain(ref inventory);
        if (success)
        {
            Debug.Log("Picked up something");
        }
    }
}