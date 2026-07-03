using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileSpikeAttack : BossAttack
{
    public float startPosLeftX;
    public float startPosRightX;

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
        if (transform.position.x > 0)
        {
            Instantiate(projectilePrefab, new Vector3(startPosLeftX, transform.position.y, transform.position.z), Quaternion.identity);
        }
        else
        {
            Instantiate(projectilePrefab, new Vector3(startPosRightX, transform.position.y, transform.position.z), Quaternion.identity);
        }
    }
}