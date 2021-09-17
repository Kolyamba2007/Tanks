using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum TankAudio { Death, Hit, Shoot }
    public enum GameAudio { LevelStart, GameOver }

    [SerializeField]
    private AudioClip _deathAudio;
    [SerializeField]
    private AudioClip _hitAudio;
    [SerializeField]
    private AudioClip _shootAudio;
    [SerializeField]
    private AudioClip _levelStartAudio;
    [SerializeField]
    private AudioClip _gameOverAudio;

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
    public void PlayAudioShot(GameAudio audio)
    {
        var source = new GameObject("[Audio Source]");
        AudioSource audioSource = source.AddComponent<AudioSource>();
        switch (audio)
        {
            case GameAudio.LevelStart:
                audioSource.clip = _levelStartAudio;
                break;
            case GameAudio.GameOver:
                audioSource.clip = _gameOverAudio;
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
