using UnityEngine;

public class NewPlayerSFX : MonoBehaviour
{
    private NewPlayerMovement playerMovement;

    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip land;
    [SerializeField] private AudioClip hurt;

    private AudioSource audioSource;
    void Start()
    {
        playerMovement = GetComponent<NewPlayerMovement>();
        audioSource = GetComponentInChildren<AudioSource>();

        playerMovement.OnJump += OnJump;
        playerMovement.OnLand += OnLand;
    }

    private void OnDisable()
    {
        playerMovement.OnJump -= OnJump;
        playerMovement.OnLand -= OnLand;
    }

    private void PlayAudio(AudioClip clip)
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
    private void OnDamaged()
    {
        PlayAudio(hurt);
    }
}
