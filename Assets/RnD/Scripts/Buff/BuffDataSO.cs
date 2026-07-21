using UnityEngine;

public enum StatType
{
    MoveSpeed,
    MaxHealth,
    AttackRange,
    AttackCooldown,
    AttackDamage,
    JumpForce,
    IFrameDuration,
}

public enum ModifierType
{
    Flat,   
    Percent 
}

[CreateAssetMenu(fileName = "NewBuff", menuName = "ScriptableObjects/Buff")]
public class BuffDataSO : ScriptableObject
{
    public string buffName;
    public StatType statType;
    public ModifierType modifierType;
    public float value;
    public float duration; 

    public bool IsTemporary => duration > 0f;
}
