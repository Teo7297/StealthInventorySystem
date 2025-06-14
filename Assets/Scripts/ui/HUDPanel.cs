using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This class manages the logic for the HUD UI panel.
/// </summary>

public class HUDPanel : MonoBehaviour
{
    private VisualElement panel;
    private VisualElement healthBarFront;
    private VisualElement healthBarBack;
    private PlayerInteraction playerInteraction;
    private Health playerHealth;

    private void Awake()
    {
        var uiDocument = GetComponent<UIDocument>();

        if (uiDocument == null)
            Debug.LogError("[UIManager] Could not initialize UIManager required components!");

        var rootElement = uiDocument.rootVisualElement;
        panel = rootElement.Q<VisualElement>("HUD");

        if (panel == null)
            Debug.LogError("[HUDPanel] Could not find required UI panels in the UI document");

        var hbarCont = panel.Query<VisualElement>(name: "HealthBar").First();
        var children = hbarCont.Query<VisualElement>().ToList(); // -> unity-progress-bar -> container -> progress + background
        healthBarBack = children[2];
        healthBarFront = children[3];

        healthBarBack.style.backgroundColor = Color.white;
        healthBarFront.style.backgroundColor = Color.red;
    }

    /// <summary>
    /// Notify this object that the player is now available/spawned.
    /// Initialize the inner components with player data.
    /// </summary>
    /// <param name="playerObj"></param>
    public void PlayerAvailable(GameObject playerObj)
    {
        playerInteraction = playerObj.GetComponent<PlayerInteraction>();
        playerHealth = playerObj.GetComponent<Health>();

        playerInteraction.OnInteractInfoText += SetInteractInfoText;
        playerHealth.OnHealthChanged += UpdateHealthBar;

        UpdateHealthBar(playerHealth.GetHealth(), playerHealth.GetMaxHealth());
    }

    /// <summary>
    /// Sets the interaction info text on the ui panel
    /// </summary>
    /// <param name="message"></param>
    public void SetInteractInfoText(string message)
    {
        var infoText = panel.Q<Label>("InfoText");
        infoText.text = message;
    }

    /// <summary>
    /// Enable this panel in the viewport
    /// </summary>
    public void Show()
    {
        panel.style.display = DisplayStyle.Flex;
        panel.SetEnabled(true);

        // This panel requires a hidden cursor
        UnityEngine.Cursor.visible = false;
    }

    /// <summary>
    /// Hide this panel from the viewport
    /// </summary>
    public void Hide()
    {
        panel.style.display = DisplayStyle.None;
        panel.SetEnabled(false);
    }

    /// <summary>
    /// Update the health bar ui.
    /// </summary>
    /// <param name="curr"></param>
    /// <param name="max"></param>
    private void UpdateHealthBar(float curr, float max)
    {
        var perc = (curr / max) * 100;
        healthBarFront.style.width = new StyleLength(Length.Percent(perc));
        Debug.Log(healthBarFront.style.width);
    }
}