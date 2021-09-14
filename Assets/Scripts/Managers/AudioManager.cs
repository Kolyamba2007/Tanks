using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum TankAudio { Death, Hit, Shoot }

    [SerializeField]
    private AudioClip _deathAudio;
    [SerializeField]
    private AudioClip _hitAudio;
    [SerializeField]
    private AudioClip _shootAudio;

    public void PlayAudioShot(TankAudio audio)
    {
        var source = new GameObject("[Audio Source]");
        AudioSource audioSource = source.AddComponent<AudioSource>();
        switch (audio)
        {
            case TankAudio.Death:
                audioSource.clip = _deathAudio;
                break;
            case TankAudio.Hit:
                audioSource.clip = _hitAudio;
                break;
            case TankAudio.Shoot:
                audioSource.clip = _shootAudio;
                break;
        }
        audioSource.Play();
        StartCoroutine(AudioCoroutine(audioSource));
    }
    private IEnumerator AudioCoroutine(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        Destroy(source.gameObject);
    }
}
