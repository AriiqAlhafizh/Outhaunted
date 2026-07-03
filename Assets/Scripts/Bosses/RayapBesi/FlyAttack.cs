using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAttack : BossAttack
{
    private AudioSource _AudioSource;
    public AudioClip flyAttackSound;

    [Header("Fly Settings")]
    public float leftPost;
    public float rightPost;
    public float flySpeed = 5f;
    public float direction;
    public bool isFlying = false;

    public override void Start()
    {
        base.Start();
        _AudioSource = GetComponentInChildren<AudioSource>();
        Duration = Mathf.Abs(leftPost - rightPost) / flySpeed;
        ActionEvent += ExecuteAttack;
    }

    private void OnDisable()
    {
        ActionEvent -= ExecuteAttack;
    }

    private void ExecuteAttack()
    {
        Fly();
    }

    private void Fly()
    {
        _AudioSource.PlayOneShot(flyAttackSound);
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
                _AudioSource.Stop();
                isFlying = false;
            }
        }
    }
}
