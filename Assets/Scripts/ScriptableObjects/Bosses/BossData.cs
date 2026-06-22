using UnityEngine;
public enum BossType
{
    TripleTMascot,
    RayapBesi
}
[CreateAssetMenu(fileName = "NewEntity", menuName = "Entity/Boss")]
public class BossData : ScriptableObject
{
    [Header("Boss Settings")]
    public BossType bossType;

    [Header("Stats")]
    public int maxHealth;
    public int baseAttack;
}
