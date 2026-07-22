using System.Collections;
using UnityEngine;

public class ProjectileSpreadAttack : BossAttack
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float projectileDelay; // 0 untuk shotgun, .5f untuk ada delaynya

    private float facingDirection;
    public override void Start()
    {
        base.Start();
        Duration = projectileDelay * 3;
        ActionEvent += ExecuteAttack;
    }
    private void OnDisable()
    {
        ActionEvent -= ExecuteAttack;
    }
    private void ExecuteAttack()
    {
        facingDirection = transform.position.x > 0 ? -1f : 1f;
        StartCoroutine(SpawnProjectile(projectileDelay));
    }

    private IEnumerator SpawnProjectile(float delay)
    {
        Vector3 projSource = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
        for (int i = 0; i < 3; i++) 
        {
            float projectileRotation = i * 15f * facingDirection;
            Quaternion rotation = Quaternion.Euler(0, 0, projectileRotation);
            Instantiate(projectilePrefab, projSource, rotation);
            yield return new WaitForSeconds(delay);
        }
    }
}
