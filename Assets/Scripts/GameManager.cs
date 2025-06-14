using UnityEngine;

/// <summary>
/// Simple game manager class. For this simple projects it simply spawns the player in a preselected location.
/// Spawning the player this way allows for better control of game initialization flow.
/// Modeled as a singleton for convenience.
/// </summary>

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player playerPrefab;

    // Singleton instance initialization
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    private void Start()
    {
        // Spawn the player on the level
        var spawn = FindObjectsByType<PlayerSpawn>(FindObjectsSortMode.None)[0];
        var playerInstance = Instantiate(playerPrefab, spawn.transform);
        UIManager.Instance.PlayerAvailable(playerInstance.gameObject);
    }


}