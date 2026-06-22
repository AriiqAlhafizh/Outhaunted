using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // Attacks
    /*  MoveEvent
     *  JumpAttack
     *  SpinAttack
     *  BounceAttack
     *  StompAttack
     */
    [Header("Boss MoveSet")]
    public List<BossAttack> Attacks = new();

    [Header("Attack Settings")]
    public int currentPhase = 0;
    public int startingAttacks = 3;
    public float delayBetweenAttacks = 1f;
    private void Start()
    {   
        StartCoroutine(AttackCycleCoroutine());
    }

    private IEnumerator AttackCycleCoroutine()
    {
        while (true)
        {
            int randomAttack = UnityEngine.Random.Range(0, startingAttacks + currentPhase);
            Debug.Log("Trying Event No. " + randomAttack);

            yield return StartCoroutine(Attacks[randomAttack].Execute());

            yield return new WaitForSeconds(delayBetweenAttacks);
        }
    }

    public void IncreasePhase()
    {
        currentPhase++;
    }
}