using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public AudioClip jump;
    public AudioClip land;
    public AudioClip combo;
    public AudioClip hurt;
    public AudioClip attack;
    public AudioClip attackHit;

    AudioSource audioSource;
    PlayerContext context;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        context = GetComponentInParent<PlayerContext>();

        context.Movement.OnJump += () => PlayAudio(jump);
        context.Movement.OnLand += () => PlayAudio(land);

        PlayerManager.Instance.OnDamaged += () => PlayAudio(hurt);

        context.Attack.OnAttack += () => PlayAudio(attack);
        context.Attack.OnAttackHit += (GameObject) => PlayAudio(attackHit);

    }
    private void OnDisable()
    {
        context.Movement.OnJump -= () => PlayAudio(jump);
        context.Movement.OnLand -= () => PlayAudio(land);

        PlayerManager.Instance.OnDamaged -= () => PlayAudio(hurt);

        context.Attack.OnAttack -= () => PlayAudio(attack);
        context.Attack.OnAttackHit -= (GameObject) => PlayAudio(attackHit);
    }

    public void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
