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
        ResetHealthDisplay();
        PlayerManager.Instance.OnDamaged += OnHealthChange;
    }

    void OnDestroy()
    {
        PlayerManager.Instance.OnDamaged -= OnHealthChange;
    }

    void OnHealthChange()
    {
        int currentHealth = PlayerManager.Instance.CurrentHealth;
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
    public void ResetHealthDisplay()
    {
        for (int i = 0; i < health.Length; i++)
        {
            health[i].sprite = fullHealth;
        }
    }

}
