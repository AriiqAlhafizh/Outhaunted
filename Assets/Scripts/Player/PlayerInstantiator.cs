using System;
using UnityEngine;

public class PlayerInstantiator : MonoBehaviour
{
    public Vector2 spawnPos = Vector2.zero;
    private void Start()
    {
        SpawnPlayer();
    }
    public void SpawnPlayer()
    {
        GameObject player = Instantiate(PlayerManager.Instance.CurrentCharacter.characterPrefab, spawnPos, Quaternion.identity);
        foreach (var ability in PlayerManager.Instance.CurrentCharacter.startingAbilities)
        {
            Type abilityType = ability.GetType();
            player.AddComponent(abilityType);
        }
        
        PlayerManager.Instance.ResetStats();
        PlayerManager.Instance.InGame = true;

        Destroy(gameObject);
    }
}
