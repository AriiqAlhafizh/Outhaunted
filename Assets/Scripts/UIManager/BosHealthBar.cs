using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BosHealthBar : MonoBehaviour
{
    public Slider HealthBarSlider;
    public TextMeshProUGUI HealthBarText;

    public int CurrentHealth;
    public int maxHealth;
    void Start()
    {
        CurrentHealth = maxHealth;
        HealthBarSlider.value = CurrentHealth;
    }

    void Update()
    {
        UpdateHealthBar();
    }
    public void UpdateHealthBar()
    {
        //HealthBarSlider.value = (float)CurrentHealth / maxHealth;
        HealthBarText.text = $"{CurrentHealth} / {maxHealth}";
        HealthBarSlider .value = CurrentHealth;
        HealthBarSlider.maxValue = maxHealth;

    }
}
