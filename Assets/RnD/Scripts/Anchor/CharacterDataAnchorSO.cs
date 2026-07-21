using System;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterDataAnchor", menuName = "ScriptableObjects/Anchors/CharacterDataAnchor")]
public class CharacterDataAnchorSO : ScriptableObject
{
    [Header("Reference")]
    private CharacterData baseTemplate;

    [Header("Runtime Base Values")]
    public int baseMaxHealth;
    public float baseMoveSpeed;
    public float baseJumpForce;
    public float baseAttackDamage;
    public float baseAttackCooldown;
    public float baseAttackRange;
    public float baseIFrameDuration;


    [Header("Runtime Status")]
    public float currentHealth;

    public List<BuffDataSO> activeBuffs = new List<BuffDataSO>();

    public event Action<StatType, float> OnStatChanged;
    public event Action<float, float> OnHealthChanged;

    public void ResetData(CharacterData baseTemplate)
    {
        baseMaxHealth = baseTemplate.maxHealth;
        baseMoveSpeed = baseTemplate.moveSpeed;
        baseJumpForce = baseTemplate.jumpForce;
        baseAttackDamage = baseTemplate.attackDamage;
        baseAttackCooldown = baseTemplate.attackCooldown;
        baseAttackRange = baseTemplate.attackRange;
        baseIFrameDuration = baseTemplate.iFrameDuration;

        currentHealth = baseMaxHealth;

        activeBuffs.Clear();
    }

    public float GetStat(StatType type)
    {
        float baseVal = GetBaseValue(type);
        float flatSum = 0f;
        float percentSum = 0f;

        foreach (var buff in activeBuffs)
        {
            if (buff.statType != type) continue;

            if (buff.modifierType == ModifierType.Flat)
                flatSum += buff.value;
            else if (buff.modifierType == ModifierType.Percent)
                percentSum += buff.value;
        }

        float finalValue = (baseVal + flatSum) * (1f + percentSum);
        return Mathf.Max(0f, finalValue);
    }

    private float GetBaseValue(StatType type)
    {
        return type switch
        {
            StatType.MoveSpeed => baseMoveSpeed,
            StatType.MaxHealth => baseMaxHealth,
            StatType.AttackDamage => baseAttackDamage,
            StatType.JumpForce => baseJumpForce,
            StatType.AttackCooldown => baseAttackCooldown,
            StatType.AttackRange => baseAttackRange,
            StatType.IFrameDuration => baseIFrameDuration,
            _ => 0f
        };
    }

    public void AddBuff(BuffDataSO buff)
    {
        activeBuffs.Add(buff);
        OnStatChanged?.Invoke(buff.statType, GetStat(buff.statType));
    }

    public void RemoveBuff(BuffDataSO buff)
    {
        if (activeBuffs.Remove(buff))
        {
            OnStatChanged?.Invoke(buff.statType, GetStat(buff.statType));
        }
    }

    public void ModifyHealth(float amount)
    {
        float maxHP = GetStat(StatType.MaxHealth);
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHP);
        OnHealthChanged?.Invoke(currentHealth, maxHP);
    }
}
