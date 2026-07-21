using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
public enum PlayerType
{
    Kuntilanak,
    Pocong
}
[CreateAssetMenu(fileName = "NewEntity", menuName = "ScriptableObjects/Entities/PlayerCharacter")]
public class CharacterData : ScriptableObject
{
    [Header("Player Settings")]
    public PlayerType playerType;
    public GameObject characterPrefab;
    public Sprite healthIcon;
    public string tutorialSceneName;

    [Header("Stats")]
    public int maxHealth;
    public float moveSpeed;
    public float jumpForce;
    public float attackDamage;
    public float attackCooldown;
    public float attackRange;
    public float pogoCooldown;
    public float iFrameDuration;
}
