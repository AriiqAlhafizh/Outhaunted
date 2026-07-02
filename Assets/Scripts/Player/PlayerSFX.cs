using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public AudioClip walk;
    public AudioClip jump;
    public AudioClip land;
    public AudioClip combo;
    public AudioClip hurt;
    public AudioClip die;
    public AudioClip attack;
    public AudioClip attackHit;

    AudioSource audioSource;
    PlayerContext context;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        context = GetComponentInParent<PlayerContext>();

        context.Movement.OnMove += () => PlayAudio(walk);
        context.Movement.OnJump += () => PlayAudio(jump);
        context.Movement.OnLand += () => PlayAudio(land);

        PlayerManager.Instance.OnDamaged += () => PlayAudio(hurt);
        PlayerManager.Instance.OnDeath += () => PlayAudio(die);

        context.Attack.OnAttack += () => PlayAudio(attack);
        context.Attack.OnAttackHit += (GameObject) => PlayAudio(attackHit);

    }
    private void OnDisable()
    {
        context.Movement.OnMove -= () => PlayAudio(walk);
        context.Movement.OnJump -= () => PlayAudio(jump);
        context.Movement.OnLand -= () => PlayAudio(land);

        PlayerManager.Instance.OnDamaged -= () => PlayAudio(hurt);
        PlayerManager.Instance.OnDeath -= () => PlayAudio(die);

        context.Attack.OnAttack -= () => PlayAudio(attack);
        context.Attack.OnAttackHit -= (GameObject) => PlayAudio(attackHit);
    }

    public void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
