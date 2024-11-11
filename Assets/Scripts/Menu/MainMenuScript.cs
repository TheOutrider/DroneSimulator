using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Button startButton, quitButton, controlsButton, backButton;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    void Start()
    {
        startButton.onClick.AddListener(OnStartPressed);
        quitButton.onClick.AddListener(OnQuitPressed);
        controlsButton.onClick.AddListener(OnControlsPressed);
        backButton.onClick.AddListener(OnBackPressed);
    }

    private void OnBackPressed()
    {
        PlayClickSound();
    }

    private void OnControlsPressed()
    {
        PlayClickSound();
    }

    private void OnStartPressed()
    {
        PlayClickSound();
        SceneManager.LoadScene("SampleScene");
    }

    private void OnQuitPressed()
    {
        PlayClickSound();
        Application.Quit();
    }

    private void PlayClickSound()
    {
        audioSource.PlayOneShot(clip);
    }
}
