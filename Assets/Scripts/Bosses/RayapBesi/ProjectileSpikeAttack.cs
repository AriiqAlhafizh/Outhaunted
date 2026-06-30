using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileSpikeAttack : BossAttack
{
    [Header("Spike Settings")]
    public GameObject projectilePrefab;

    public override void Start()
    {
        base.Start();
        Duration = .2f;
        ActionEvent += ExecuteAttack;
    }

    private void OnDisable()
    {
        ActionEvent -= ExecuteAttack;
    }

    private void ExecuteAttack()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }
}