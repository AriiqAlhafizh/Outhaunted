using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[CreateAssetMenu(fileName = "GlideSO", menuName = "ScriptableObjects/Ability/Glide")]
public class GlideSO : AbilitySO
{
    [Header("References")]
    private InputReader input;
    private EnvironmentSensor2D envSensor;
    private Rigidbody2D rb;

    [Header("Glide Settings")]
    [SerializeField] private float glideFallSpeed = 2f;
    private bool isGliding = false;

    [Header("VFX Configuration")]
    [SerializeField] protected Vector3 vfxOffset = new Vector3(0f, 0.5f, 0f); 
    [SerializeField] protected string vfxAnimationName;
    protected int _vfxAnimHash;

    public override void Initialize(GameObject player, InputReader _input)
    {
        base.Initialize(player, input);

        envSensor = player.GetComponent<EnvironmentSensor2D>();
        rb = player.GetComponent<Rigidbody2D>();

        _vfxAnimHash = Animator.StringToHash(vfxAnimationName);

        input = _input;

        input.JumpPressed += StartGlide;
        input.JumpReleased += StopGlide;
    }
    public override void OnDestroyAbility()
    {
        input.JumpPressed -= StartGlide;
        input.JumpReleased -= StopGlide;
    }

    private void StartGlide()
    {
        if (envSensor != null && !envSensor.isGrounded)
        {
            isGliding = true;
            //effectsHandler.PlaySpriteVFX(_vfxAnimHash, vfxOffset);
            //effectsHandler.PlaySound(abilitySound);
            //TriggerAnimation(animationHash);
        }
    }
    private void StopGlide()
    {
        isGliding = false;
    }

    public override void FixedTick(float fixedDeltaTime)
    {
        if (isGliding && envSensor != null && envSensor.isGrounded)
        {
            StopGlide();
            return;
        }

        if (isGliding)
        {
            if (rb.linearVelocity.y < -glideFallSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -glideFallSpeed);
            }
        }
    }
}
