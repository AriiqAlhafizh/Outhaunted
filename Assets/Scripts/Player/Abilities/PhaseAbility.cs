using System.Collections;
using UnityEngine;

public class PhaseAbility : Ability
{
    private static readonly int PhaseThroughHash = Animator.StringToHash("PhaseThrough");
    [SerializeField] PlayerAnimations pAnimation;

    [Header("Phase Settings")]
    [SerializeField] private float phaseCooldown = 1f;
    [SerializeField] private float phaseDuration = 1f;
    [SerializeField] private string enemyLayerName = "Enemy";
    [SerializeField] private string playerLayerName = "Player";

    [Header("Debug")]
    [SerializeField] private bool inPhaseMode = false;
    [SerializeField] private float lastPhaseTime = -Mathf.Infinity;

    private int enemyLayer;
    private int playerLayer;
    private void Start()
    {
        pAnimation = GetComponent<PlayerAnimations>();

        context.Input.DashPressed += StartPhase;
        enemyLayer = LayerMask.NameToLayer(enemyLayerName);
        playerLayer = LayerMask.NameToLayer(playerLayerName);
    }

    private IEnumerator PhaseCoroutine()
    {
        if (inPhaseMode)
            yield break;

        inPhaseMode = true;
        context.Attack.canAttack = false;
        pAnimation.SetInAbility(true);
        pAnimation.animator.Play(PhaseThroughHash);

        // Ignore collisions between player and enemy layers
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        yield return new WaitForSeconds(phaseDuration);

        // Re-enable collisions between player and enemy layers
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        inPhaseMode = false;
        context.Attack.canAttack = true;
        pAnimation.SetInAbility(false);
    }

    private void StartPhase()
    {
        if (Time.time >= lastPhaseTime + phaseCooldown)
        {
            StartCoroutine(PhaseCoroutine());
            lastPhaseTime = Time.time;
        }
    }
}
