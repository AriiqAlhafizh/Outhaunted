using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    // Attacks
    /*  MoveEvent
     *  JumpAttack
     *  SpinAttack
     *  BounceAttack
     *  StompAttack
     */

    [Header("Boss Stats")]
    public int MaxHealth;
    public int currentHealth;

    [Header("Boss MoveSet")]
    public List<BossAttack> Attacks = new();

    [Header("Attack Settings")]
    public int currentPhase = 0;
    public int startingAttacks = 3;
    public float delayBetweenAttacks = 1f;
    private void Start()
    {   
        BossManager.Instance.OnDamaged += OnDamaged;
        BossManager.Instance.OnDeath += Die;
        StartCoroutine(AttackCycleCoroutine());
    }
    private void OnDisable()
    {
        BossManager.Instance.OnDamaged -= OnDamaged;
        BossManager.Instance.OnDeath -= Die;
    }

    private void Update()
    {
        if (PlayerManager.Instance.PlayerPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
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

    public void InitializeStats(BossData bossData)
    {
        MaxHealth = bossData.maxHealth;
        currentHealth = MaxHealth;
        startingAttacks = bossData.startingAttacks;
    }

    public void OnDamaged()
    {
        if (currentPhase == 0 && currentHealth <= MaxHealth * 0.67f)
        {
            IncreasePhase();
        } 
        else if (currentPhase == 1 && currentHealth <= MaxHealth * 0.33f)
        {
            IncreasePhase();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Boss has been defeated!");
    }
}