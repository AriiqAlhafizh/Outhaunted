using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpinAttack : BossAttack
{
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    [Header("Spin Attack Settings")]
    public float spinDuration = 2f;
    public float spinDistance = 5f;
    public AnimationCurve spinCurve;
    public Collider2D col;

    List<Animator> animators;

    public override void Start()
    {
        base.Start();
        animators = GetComponentsInChildren<Animator>().ToList();
        Duration = spinDuration;
        ActionEvent += ExecuteAttack;
    }

    private void OnDisable()
    {
        ActionEvent -= ExecuteAttack;
    }

    private void ExecuteAttack()
    {
        
        StartCoroutine(SpinCoroutine());
    }

    private IEnumerator SpinCoroutine() 
    {
        animators[1].Play(AttackHash);
        StartCoroutine(HitboxDuration());
        Vector3 startPos = transform.position;
        Vector3 playerPos = PlayerManager.Instance.PlayerPosition;

        // Only calculate direction on the X axis
        Vector3 direction = new Vector3(playerPos.x - startPos.x, 0f, 0f).normalized;
        Vector3 targetPos = startPos + direction * spinDistance;

        float timeElapsed = 0f;

        while (timeElapsed < spinDuration)
        {
            float t = timeElapsed / spinDuration;
            float evaluatedT = spinCurve.Evaluate(t);

            transform.position = Vector3.LerpUnclamped(startPos, targetPos, evaluatedT);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        animators[1].Play("Base");
    }

    public IEnumerator HitboxDuration()
    {
        col.enabled = true;
        yield return new WaitForSeconds(Duration - 1f);
        col.enabled = false;
    }
}
