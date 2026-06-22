using System;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;
    
    public CharacterData CurrentCharacter;
    public PlayerContext CurrentPlayerContext;

    public int MaxHealth;
    public int CurrentHealth;
    public Vector3 PlayerPosition;
    public float iFrame;
    public bool inIFrame;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            MaxHealth = CurrentCharacter.maxHealth;
            CurrentHealth = MaxHealth;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (CurrentPlayerContext == null)
        {
            FindPlayerContext();
            Debug.Log("Got Player Context");
        }
        PlayerPosition = CurrentPlayerContext.Position;
    }

    private void FindPlayerContext()
    {
        CurrentPlayerContext = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContext>();
    }
}
