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
    [SerializeField] private GameObject gameCanvas;

    void Start()
    {
        startButton.onClick.AddListener(OnStartPressed);
        buttonText = startButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            gameCanvas.SetActive(true);
        }
    }

    public void OnClosePressed()
    {
        PlayClickSound();
        if (!hasGameStarted)
        {
            buttonText.text = "EXIT TO MENU";
            hasGameStarted = true;
        }
        gameCanvas.SetActive(false);
    }

    private void OnStartPressed()
    {
        PlayClickSound();
        if (!hasGameStarted)
        {
            buttonText.text = "EXIT TO MENU";
            hasGameStarted = true;
            gameCanvas.SetActive(false);
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
