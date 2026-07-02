using System.Collections;
using UnityEngine;

public class PhaseAbility : Ability
{
    private static readonly int PhaseHash = Animator.StringToHash("Phase");
    private static readonly int PhaseThroughHash = Animator.StringToHash("PhaseThrough");
    PlayerAnimations pAnimation;
    Animator animator;
    public AudioClip phaseSound;

    [Header("Phase Settings")]
    [SerializeField] private float phaseCooldown = 1f;
    [SerializeField] private float phaseDuration = 1f;
    [SerializeField] private string enemyLayerName = "Enemy";
    [SerializeField] private string playerLayerName = "Player";
    public float phaseMoveSpeedMultiplier = 1.4f;

    [Header("Debug")]
    [SerializeField] private bool inPhaseMode = false;
    [SerializeField] private float lastPhaseTime = -Mathf.Infinity;
    public float defaultMoveSpeed;
    public float defaultGravity;

    private int enemyLayer;
    private int playerLayer;
    PlayerSFX playerSFX;
    private void Start()
    {
        pAnimation = GetComponent<PlayerAnimations>();
        playerSFX = GetComponentInChildren<PlayerSFX>();

        animator = GameObject.FindGameObjectWithTag("VFX").GetComponent<Animator>();

        context.Input.DashPressed += StartPhase;
        enemyLayer = LayerMask.NameToLayer(enemyLayerName);
        playerLayer = LayerMask.NameToLayer(playerLayerName);

        defaultMoveSpeed = PlayerManager.Instance.CurrentCharacter.moveSpeed;
        defaultGravity = context.Rigidbody.gravityScale;
    }

    private IEnumerator PhaseCoroutine()
    {
        if (inPhaseMode)
            yield break;

        inPhaseMode = true;
        context.Attack.canAttack = false;
        pAnimation.SetInAbility(true);

        pAnimation.animator.Play(PhaseThroughHash);
        playerSFX.PlayAudio(phaseSound);
        animator.SetTrigger(PhaseHash);
        context.Movement.moveSpeed = defaultMoveSpeed * phaseMoveSpeedMultiplier; // Increase move speed during phase
        context.Movement.canJump = false;
        context.Rigidbody.linearVelocityY = 0f;
        context.Rigidbody.gravityScale = 0f; // Disable gravity during phase

        // Ignore collisions between player and enemy layers
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        yield return new WaitForSeconds(phaseDuration);

        // Re-enable collisions between player and enemy layers
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        inPhaseMode = false;
        context.Attack.canAttack = true;
        pAnimation.SetInAbility(false);
        context.Movement.moveSpeed = defaultMoveSpeed; // Reset move speed after phase
        context.Movement.canJump = true;
        context.Rigidbody.gravityScale = defaultGravity; // Reset gravity after phase
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
