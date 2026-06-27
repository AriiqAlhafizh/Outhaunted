using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int maximum;
    public int current;
    public Image mask;

    void Start()
    {
        BossStatsManager.Instance.OnDamaged += GetCurrentFill;
    }

    private void OnDisable()
    {
        BossStatsManager.Instance.OnDamaged -= GetCurrentFill;
    }

    void GetCurrentFill()
    {
        int currHealth = BossStatsManager.Instance.CurrentHealth;
        float fillAmount = (float)currHealth / maximum;
        mask.fillAmount = fillAmount;
    }   
}
