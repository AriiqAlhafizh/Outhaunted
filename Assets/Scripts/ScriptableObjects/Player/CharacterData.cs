using System.Collections.Generic;
using UnityEditor.Animations;
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

    [Header("Stats")]
    public int maxHealth;
    public float moveSpeed;
    public float jumpForce;
    public float attackDamage;
    public float attackCooldown;

    [Header("Abilities")]
    public List<Ability> startingAbilities;

    [Header("Animator Settings")]
    public AnimatorController attackAnimator;
}
