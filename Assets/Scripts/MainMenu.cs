using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);

        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
