using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuScript : MonoBehaviour
{
    [SerializeField] private Button startButton;
    private TextMeshProUGUI buttonText;
    [SerializeField] private AudioSource audioSource;
    private bool hasGameStarted = false;

    void Start()
    {
        startButton.onClick.AddListener(OnStartPressed);
        buttonText = startButton.GetComponentInChildren<TextMeshProUGUI>();

    }

    private void OnStartPressed()
    {
        PlayClickSound();
        if (!hasGameStarted)
        {
            buttonText.text = "EXIT TO MENU";
            hasGameStarted = true;
        }
        else
        {
            SceneManager.LoadScene("MenuScene");
        }
    }

    private void PlayClickSound()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
}
