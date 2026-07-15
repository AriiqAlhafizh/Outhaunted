using UnityEngine;

public class EffectsHandler : MonoBehaviour
{
    [Header("Child References")]
    [SerializeField] private AttackHitbox2D meleeHitbox;
    [SerializeField] private ParticleSystem genericParticleVFX;
    [SerializeField] private Animator spriteVFXAnimator; 

    public void TriggerHitbox(Vector2 size, Vector2 offset, float damage, float duration, AttackDirection attackDirection)
    {
        meleeHitbox.Activate(size, offset, damage, duration, attackDirection);
    }

    public void PlayParticleVFX(Vector3 localOffset)
    {
        genericParticleVFX.transform.localPosition = localOffset;
        genericParticleVFX.Play();
    }

    public void PlaySpriteVFX(int vfxAnimHash, Vector3 localOffset)
    {
        if (spriteVFXAnimator != null) { Debug.Log("animator not null"); }
        spriteVFXAnimator.transform.localPosition = localOffset;
        spriteVFXAnimator.Play(vfxAnimHash, 0, 0f);
    }
}
