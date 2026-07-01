using System;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;

    public BossData CurrentBoss;
    public int MaxHealth;
    public int CurrentHealth;

    // Event
    public event Action OnDamaged;
    public event Action OnDeath;

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

    public void SetBossGameObject(BossData boss)
    {
        CurrentBoss = boss;
    }
    public void ResetBossStats()
    {
        MaxHealth = CurrentBoss.maxHealth;
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        OnDamaged?.Invoke();
        if (CurrentHealth <= 0)
        {
            PlayerManager.Instance.InGame = false;
            PlayerManager.Instance.RemovePlayerContext();

            CurrentHealth = 0;
            OnDeath?.Invoke();
        }
    }
}
