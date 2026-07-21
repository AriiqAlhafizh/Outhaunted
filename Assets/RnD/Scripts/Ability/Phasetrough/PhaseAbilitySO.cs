using UnityEngine;

[CreateAssetMenu(fileName = "PhaseSO", menuName = "ScriptableObjects/Ability/Phasethrough")]
public class PhaseAbilitySO : AbilitySO
{
    [Header("References")]
    [SerializeField] private CharacterDataAnchorSO characterDataAnchor;
    [SerializeField] BuffDataSO phaseBuff;
    private NewPlayerAnimation playerAnimation;
    private CharacterDataManager characterDataManager;
    private NewPlayerMovement playerMovement;
    private AbilityHandler abilityHandler;
    private InputReader input;
    private Rigidbody2D rb;

    [Header("VFX Configuration")]
    [SerializeField] protected Vector3 vfxOffset = new Vector3(0f, 0.5f, 0f); 
    [SerializeField] protected string vfxAnimationName;
    protected int _vfxAnimHash;

    [Header("Phase Settings")]
    [SerializeField] private string enemyLayerName = "Enemy";
    [SerializeField] private string playerLayerName = "Player";
    private int enemyLayer;
    private int playerLayer;

    [Header("Debug")]
    [SerializeField] private bool inPhaseMode = false;
    [SerializeField] private float lastPhaseTime = -Mathf.Infinity;
    [SerializeField] private float defaultMoveSpeed;
    [SerializeField] private float defaultGravity;

    public override void Initialize(GameObject player, InputReader _input)
    {
        base.Initialize(player, input);

        _vfxAnimHash = Animator.StringToHash(vfxAnimationName);

        input = _input;

        playerMovement = player.GetComponent<NewPlayerMovement>();
        characterDataManager = player.GetComponent<CharacterDataManager>();
        abilityHandler = player.GetComponent<AbilityHandler>();
        playerAnimation = player.GetComponent<NewPlayerAnimation>();
        rb = player.GetComponent<Rigidbody2D>();

        enemyLayer = LayerMask.NameToLayer(enemyLayerName);
        playerLayer = LayerMask.NameToLayer(playerLayerName);

        defaultMoveSpeed = characterDataAnchor.baseMoveSpeed;

        input.DashPressed += StartPhase;

        characterDataManager.OnTemporaryBuffEnded += StopPhase;
    }

    public override void OnDestroyAbility()
    {
        input.DashPressed -= StartPhase;
        characterDataManager.OnTemporaryBuffEnded -= StopPhase;
    }

    private void StartPhase()
    {
        float currentCooldown = cooldownDuration;
        float lastTime = lastPhaseTime;

        if (Time.time >= lastTime + currentCooldown)
        {
            lastPhaseTime = Time.time;

            StartCooldown();
            Phase();
        }
    }

    private void Phase()
    {
        inPhaseMode = true;

        characterDataManager.ApplyBuff(phaseBuff);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        playerMovement.canJump = false;
        abilityHandler.canUseAbilities = false;
        playerAnimation.isInAbility = true;

        effectsHandler.PlaySpriteVFX(_vfxAnimHash, vfxOffset);
        effectsHandler.PlaySound(abilitySound);
        TriggerAnimation(animationHash);

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
    }

    private void StopPhase()
    {
        inPhaseMode = false;

        playerMovement.canJump = true;
        abilityHandler.canUseAbilities = true;
        playerAnimation.isInAbility = false;

        effectsHandler.PlaySpriteVFX(_vfxAnimHash, vfxOffset);

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }

    public override void FixedTick(float fixedDeltaTime)
    {
        if (inPhaseMode)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
    }
}
