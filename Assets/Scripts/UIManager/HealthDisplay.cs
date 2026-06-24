using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public Sprite emptyHealth;
    public Sprite fullHealth;
    public Image[] health;


    void Start()
    {
        OnHealthChange();
        PlayerStatsManager.instance.OnDamage += OnHealthChange;
    }

    void OnDestroy()
    {
        PlayerStatsManager.instance.OnDamage -= OnHealthChange;
    }

    void OnHealthChange()
    {
        int currentHealth = PlayerStatsManager.Instance.CurrentHealth;
        Debug.Log("Health changed: " + currentHealth);
        for (int i = 0; i < health.Length; i++)
        {
            if (i < currentHealth)
            {
                health[i].sprite = fullHealth;
            }
            else
            {
                health[i].sprite = emptyHealth;
            }
        }
    }
}
