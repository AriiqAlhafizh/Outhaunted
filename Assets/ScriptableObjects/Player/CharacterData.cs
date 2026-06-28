using System.Collections.Generic;
using UnityEngine;
public enum PlayerType
{
    Kuntilanak,
    Pocong
}
[CreateAssetMenu(fileName = "NewEntity", menuName = "Entity/PlayerCharacter")]
public class CharacterData : ScriptableObject
{
    [Header("Player Settings")]
    public PlayerType playerType;
    public GameObject characterPrefab;

    [Header("Stats")]
    public int maxHealth;
    public float moveSpeed;
    public float jumpForce;
    public float attackDamage;
    public float attackCooldown;
    public float iFrameDuration;

    [Header("Abilities")]
    public List<Ability> startingAbilities;
}
