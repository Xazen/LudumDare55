using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseContainer;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button restartButton;

        private void Start()
        {
            continueButton.onClick.AddListener(ContinueGame);
            menuButton.onClick.AddListener(OpenMenu);
            restartButton.onClick.AddListener(RestartGame);
            pauseContainer.SetActive(false);

            EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
        }

        private void RestartGame()
        {
            SceneManager.LoadScene("Game");
        }

        private void OpenMenu()
        {
            SceneManager.LoadScene("Boot");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseContainer.SetActive(!pauseContainer.activeSelf);
                if (pauseContainer.activeSelf)
                {
                    EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
                }
            }
        }

        private void ContinueGame()
        {
            pauseContainer.SetActive(false);
        }
    }
}