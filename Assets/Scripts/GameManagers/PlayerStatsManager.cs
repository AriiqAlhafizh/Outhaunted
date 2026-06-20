using System;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;
    
    public CharacterData CurrentCharacter;

    public int MaxHealth;
    public int CurrentHealth;

    // Event
    public event Action<int> OnDamaged;
    public event Action OnDeath;

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

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        OnDamaged?.Invoke(damage);

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnDeath?.Invoke();
        }
    }
}
