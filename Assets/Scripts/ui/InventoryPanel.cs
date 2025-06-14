using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This class manages the logic for the inventory UI panel.
/// The actual inventory logic is implemented as a player component, this
/// class interfaces with it.
/// </summary>

public class InventoryPanel : MonoBehaviour
{
    private VisualElement panel;
    private Inventory playerInventory;

    private List<Pickup.ItemData> inventoryItems;
    private bool isDragging = false;
    private int draggedCellID = -1;

    public Sprite testSprite;

    public bool IsOpen => panel.enabledSelf;

    private void Awake()
    {
        var uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null)
            Debug.LogError("[UIManager] Could not initialize UIManager required components!");

        var rootElement = uiDocument.rootVisualElement;
        panel = rootElement.Q<VisualElement>("Inventory");

        if (panel == null)
            Debug.LogError("[InventoryPanel] Could not find required UI panels in the UI document");
    }

    /// <summary>
    /// Notify this object that the player is now available/spawned.
    /// Initialize the inner components with player data.
    /// </summary>
    /// <param name="playerObj"></param>
    public void PlayerAvailable(GameObject playerObj)
    {
        playerInventory = playerObj.GetComponent<Inventory>();
        SetupInventoryPanel();
    }

    /// <summary>
    /// Enable this panel in the viewport.
    /// </summary>
    public void Show()
    {
        panel.style.display = DisplayStyle.Flex;
        panel.SetEnabled(true);

        // This panel requires a visible cursor
        UnityEngine.Cursor.visible = true;

        // Always load player items on show
        LoadPlayerItems();
    }

    /// <summary>
    /// Hide this panel from the viewport.
    /// </summary>
    public void Hide()
    {
        panel.style.display = DisplayStyle.None;
        panel.SetEnabled(false);
    }

    /// <summary>
    /// Load items from the player inventory and populate the UI with the icons/stack count.
    /// </summary>
    private void LoadPlayerItems()
    {
        inventoryItems = playerInventory.GetItems();
        var iconSlots = panel.Query<VisualElement>(name: "Icon").ToList();

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            iconSlots[i].style.backgroundImage = new StyleBackground(inventoryItems[i].Icon);
            var stackCount = inventoryItems[i].Count;
            if (stackCount > 1)
                iconSlots[i].Q<Label>(name: "StackCount").text = stackCount.ToString();
            else
                iconSlots[i].Q<Label>(name: "StackCount").text = "";
        }
    }

    /// <summary>
    /// UI click event. Either start dragging or consume the item.
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="cellID"></param>
    /// <param name="element"></param>
    private void OnCellClick(PointerDownEvent evt, int cellID, VisualElement element)
    {
        if (isDragging || inventoryItems[cellID].Count == 0)
            return;

        if (evt.button == (int)MouseButton.RightMouse)
        {
            playerInventory.UseItem(cellID);
            LoadPlayerItems();
        }

        else if (evt.button == (int)MouseButton.LeftMouse)
        {
            isDragging = true;
            draggedCellID = cellID;
        }
    }

    /// <summary>
    /// Simple hovering event.
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="element"></param>
    private void OnStartHover(PointerEnterEvent evt, VisualElement element)
    {
        element.style.unityBackgroundImageTintColor = Color.green;
    }

    /// <summary>
    /// Simple hovering event.
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="element"></param>
    private void OnStopHover(PointerLeaveEvent evt, VisualElement element)
    {
        element.style.unityBackgroundImageTintColor = Color.white;
    }

    /// <summary>
    /// Click event. If an item was being dragged, drop it in the underlying cell.
    /// </summary>
    /// <param name="evt"></param>
    /// <param name="cellID"></param>
    private void OnDrop(PointerUpEvent evt, int cellID)
    {
        if (!isDragging) return;

        playerInventory.MoveItem(draggedCellID, cellID);

        isDragging = false;
        draggedCellID = -1;

        LoadPlayerItems();
    }

    /// <summary>
    /// Setup the UI callbacks.
    /// </summary>
    private void SetupInventoryPanel()
    {
        if (panel == null)
            return;

        var cells = panel.Query<VisualElement>(name: "Icon").ToList();
        for (int i = 0; i < cells.Count; i++)
        {
            int index = i; // C# closure issue, we need a local copy of i.
            cells[index].RegisterCallback<PointerDownEvent>((evt) => { OnCellClick(evt, index, cells[index]); });
            cells[index].RegisterCallback<PointerUpEvent>((evt) => { OnDrop(evt, index); });
        }

        var borders = panel.Query<VisualElement>(name: "Border").ToList();
        for (int i = 0; i < borders.Count; i++)
        {
            var border = borders[i];
            border.RegisterCallback<PointerEnterEvent>((evt) => { OnStartHover(evt, border); });
            border.RegisterCallback<PointerLeaveEvent>((evt) => { OnStopHover(evt, border); });
        }
    }
}