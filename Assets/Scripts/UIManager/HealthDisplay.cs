using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public Image[] health;

    [Header("Heart Colors")]
    public Color fullColor = Color.white;
    public Color emptyColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    private Sprite healthIcon;

    void Start()
    {
        // Ambil icon health dari karakter yang dipilih
        healthIcon = PlayerManager.Instance.CurrentCharacter.healthIcon;

        // Semua heart menggunakan sprite yang sama
        foreach (Image heart in health)
        {
            heart.sprite = healthIcon;
        }

        ResetHealthDisplay();
        PlayerManager.Instance.OnDamaged += OnHealthChange;
    }

    void OnDestroy()
    {
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.OnDamaged -= OnHealthChange;
    }

    void OnHealthChange()
    {
        int currentHealth = PlayerManager.Instance.CurrentHealth;

        for (int i = 0; i < health.Length; i++)
        {
            health[i].sprite = healthIcon;

            if (i < currentHealth)
            {
                health[i].color = fullColor;      // Heart penuh
            }
            else
            {
                health[i].color = emptyColor;     // Heart kosong (lebih gelap)
            }
        }
    }

    public void ResetHealthDisplay()
    {
        foreach (Image heart in health)
        {
            heart.sprite = healthIcon;
            heart.color = fullColor;
        }
    }
}