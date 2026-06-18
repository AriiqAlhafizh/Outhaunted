using UnityEngine;
public enum PlayerType
{
    Kuntilanak,
    Pocong
}

public enum PlayerDirection
{
    Up,
    Right, 
    Down, 
    Left
}
public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Player Settings")]
    public PlayerType playerType;

    [Header("Stats")]
    public int maxHealth = 100;
    public int maxJumps = 2;
    public int maxDash = 1;
}
