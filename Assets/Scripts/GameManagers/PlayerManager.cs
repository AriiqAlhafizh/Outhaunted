using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Player Settings")]
    public CharacterData CurrentCharacter;
    public PlayerContext CurrentPlayerContext;

    [Header("Stats")]
    public int MaxHealth;
    public int CurrentHealth;
    public Vector3 PlayerPosition;
    public float iFrameDuration;
    public bool inIFrame;
    public float InputDisabledDuration = 0.4f;

    [Header("Game")]
    public bool IsGamePaused;
    public bool InGame;

    // Event
    public event Action OnDamaged;
    public event Action OnDeath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            IsGamePaused = false;
            InGame = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (InGame)
        {
            if (CurrentPlayerContext == null)
            {
                FindPlayerContext();
                Debug.Log("Got Player Context");
            }
            PlayerPosition = CurrentPlayerContext.Position;
        }
    }

    private void FindPlayerContext()
    {
        CurrentPlayerContext = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerContext>();
    }

    public void RemovePlayerContext()
    {
        CurrentPlayerContext = null;
    }

    public void ResetStats()
    {
        MaxHealth = CurrentCharacter.maxHealth;
        CurrentHealth = MaxHealth;
        iFrameDuration = CurrentCharacter.iFrameDuration;
    }

    public void SetCharacterData(CharacterData characterData)
    {
        CurrentCharacter = characterData;
        ResetStats();
    }
    public void GotoScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void TakeDamage()
    {
        CurrentHealth -= 1;
        OnDamaged?.Invoke();

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDeath?.Invoke();

            StartCoroutine(DieCoroutine());
        }

        Debug.Log($"Player took {1}. Current health: {CurrentHealth}");
    }

    public IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(1f);
        InGame = false;
        RemovePlayerContext();
        SceneManager.LoadScene("MainMenu");
    }
}
