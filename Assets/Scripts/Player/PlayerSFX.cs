using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public AudioClip jump;
    public AudioClip land;
    public AudioClip combo;
    public AudioClip hurt;
    public AudioClip attack;
    public AudioClip attackHit;

    public AudioSource audioSource;
    PlayerContext context;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        context = GetComponentInParent<PlayerContext>();

        context.Movement.OnJump += OnJump;
        context.Movement.OnLand += OnLand;

        PlayerManager.Instance.OnDamaged += OnDamaged;

        context.Attack.OnAttack += OnAttack;
        context.Attack.OnAttackHit += OnAttackHit;
    }
    private void OnDisable()
    {
        context.Movement.OnJump -= OnJump;
        context.Movement.OnLand -= OnLand;

        PlayerManager.Instance.OnDamaged -= OnDamaged;

        context.Attack.OnAttack -= OnAttack;
        context.Attack.OnAttackHit -= OnAttackHit;
    }

    public void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void OnJump()
    {
        PlayAudio(jump);
    }

    private void OnLand()
    {
        PlayAudio(land);
    }
    private void OnAttack()
    {
        PlayAudio(attack);
    }
    private void OnAttackHit(GameObject obj)
    {
        PlayAudio(attackHit);
    }

    private void OnDamaged()
    {
        PlayAudio(hurt);
    }
}
