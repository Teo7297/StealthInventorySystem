using UnityEngine;

/// <summary>
/// Simple UI manager responsible for the initialization of all the other UI panels.
/// This class is also responsible for the selection of the active UI panels.
/// This has been modeled as a singleton for convenience.
/// </summary>

[RequireComponent(typeof(HUDPanel))]
[RequireComponent(typeof(InventoryPanel))]
public class UIManager : MonoBehaviour
{
    private InventoryPanel inventoryPanel;
    private HUDPanel hudPanel;

    // Singleton instance initialization
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<UIManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("UIManager");
                    _instance = go.AddComponent<UIManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        hudPanel = GetComponent<HUDPanel>();
        inventoryPanel = GetComponent<InventoryPanel>();
    }

    private void Start()
    {
        inventoryPanel.Hide();
        hudPanel.Show();
    }

    /// <summary>
    /// Notify all UI panels that the player is now available/spawned.
    /// </summary>
    /// <param name="playerObj"></param>
    public void PlayerAvailable(GameObject playerObj)
    {
        hudPanel.PlayerAvailable(playerObj);
        inventoryPanel.PlayerAvailable(playerObj);
    }

    /// <summary>
    /// Toggle between HUD and Inventory
    /// </summary>
    public void ToggleInventory()
    {
        if (inventoryPanel.IsOpen)
        {
            inventoryPanel.Hide();
            hudPanel.Show();
        }
        else
        {
            inventoryPanel.Show();
            hudPanel.Hide();
        }
    }

    /// <summary>
    /// Close all panels and enable HUD
    /// </summary>
    public void BackToHUD()
    {
        inventoryPanel.Hide();
        hudPanel.Show();
    }
}