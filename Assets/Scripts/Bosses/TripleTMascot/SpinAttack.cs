using System.Collections;
using UnityEngine;

public class SpinAttack : BossAttack
{
    [Header("Spin Attack Settings")]
    public float spinDuration = 2f;
    public float spinDistance = 5f;
    public AnimationCurve spinCurve;
    public Collider2D col;

    public override void Start()
    {
        animator = GetComponentInChildren<Animator>();
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
        animator.Play("Attack");
        col.enabled = true;
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
        animator.Play("Base");
        col.enabled = false;
    }
}
