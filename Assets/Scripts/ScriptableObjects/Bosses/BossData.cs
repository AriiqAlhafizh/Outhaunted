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
    public GameObject bossPrefab;
    public Vector2 spawnPos;

    [Header("Stats")]
    public int maxHealth;
    public int startingAttacks;
}
