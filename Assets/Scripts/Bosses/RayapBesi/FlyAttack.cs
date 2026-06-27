using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAttack : BossAttack
{
    [Header("Attack Settings")]
    public List<Platform> platforms;

    [Header("Fly Settings")]
    public float leftPost;
    public float rightPost;
    public float flySpeed = 5f;
    public float direction;
    public bool isFlying = false;

    public override void Start()
    {
        base.Start();
        ActionEvent += ExecuteAttack;
    }

    private void OnDisable()
    {
        ActionEvent -= ExecuteAttack;
    }

    private void ExecuteAttack()
    {
        FlipRandomPlatform();
        Fly();
    }

    private void FlipRandomPlatform()
    {
        int randomIndex = Random.Range(0, platforms.Count);

        platforms[randomIndex].StartAnimation();
    }

    private void FlipAllPlatform()
    {
        foreach (Platform platform in platforms)
        {
            platform.StartAnimation();
        }
    }

    private void Fly()
    {
        isFlying = true;
        direction = transform.position.x > 0 ? -1 : 1;
    }

    private void Update()
    {
        if (isFlying)
        {
            transform.position += new Vector3(flySpeed * Time.deltaTime * direction, 0, 0);

            if (transform.position.x < leftPost || transform.position.x > rightPost)
            {
                isFlying = false;
            }
        }
    }
}
