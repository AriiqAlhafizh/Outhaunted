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
        GameObject player = Instantiate(PlayerStatsManager.Instance.CurrentCharacter.characterPrefab, spawnPos, Quaternion.identity);
        foreach (var ability in PlayerStatsManager.Instance.CurrentCharacter.startingAbilities)
        {
            Type abilityType = ability.GetType();
            player.AddComponent(abilityType);
        }
        
        PlayerStatsManager.Instance.ResetStats();
        PlayerStatsManager.Instance.InGame = true;

        Destroy(gameObject);
    }
}
