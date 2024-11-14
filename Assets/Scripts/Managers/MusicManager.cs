using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] private AudioSource soundtrackAudioSource;

    [SerializeField] private AudioClip[] menuSoundTracks, levelSoundtracks;
    bool inGame = false;

    public float fadeDuration = 1f;
    private int currentClipIndex = 0;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene")
        {
            inGame = true;
            StopAllMusic();
            StartMenuSoundtracks();
        }
        else
        {
            inGame = false;
        }
    }

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        StartMenuSoundtracks();
    }

    public void StartMenuSoundtracks()
    {
        currentClipIndex = Random.Range(0, inGame ? levelSoundtracks.Length : menuSoundTracks.Length);
        AudioClip currentCLip = inGame ? levelSoundtracks[currentClipIndex] : menuSoundTracks[currentClipIndex];
        soundtrackAudioSource.clip = currentCLip;
        soundtrackAudioSource.Play();

        StartCoroutine(PlayNextClipWhenFinished());
    }

    private IEnumerator PlayNextClipWhenFinished()
    {
        while (true)
        {
            // Wait until the current clip finishes
            yield return new WaitForSeconds(soundtrackAudioSource.clip.length);

            // Fade out
            yield return StartCoroutine(FadeOut(soundtrackAudioSource, fadeDuration));

            // Move to the next clip
            currentClipIndex = inGame ? (currentClipIndex + 1) % levelSoundtracks.Length : (currentClipIndex + 1) % menuSoundTracks.Length;
            soundtrackAudioSource.clip = inGame ? levelSoundtracks[currentClipIndex] : menuSoundTracks[currentClipIndex];
            soundtrackAudioSource.Play();

            // Fade in
            yield return StartCoroutine(FadeIn(soundtrackAudioSource, fadeDuration));
        }
    }

    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        audioSource.volume = 0; // Ensure volume is set to 0
        audioSource.Stop();
    }

    private IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        audioSource.volume = 0; // Start with volume at 0
        audioSource.Play();

        float targetVolume = 1f; // Set to your desired max volume
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, targetVolume, t / duration);
            yield return null;
        }

        audioSource.volume = targetVolume; // Ensure volume is set to max
    }

    public void StopAllMusic()
    {
        soundtrackAudioSource.Stop();
    }


}
