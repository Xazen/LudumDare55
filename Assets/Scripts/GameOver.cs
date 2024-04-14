using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverContainer;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button restartButton;

    [SerializeField] private TMP_Text perfectText;
    [SerializeField] private TMP_Text greatText;
    [SerializeField] private TMP_Text goodText;
    [SerializeField] private TMP_Text badText;
    [SerializeField] private TMP_Text missText;

    public bool IsOpen { get; set; }

    private void Awake()
    {
        Singletons.RegisterGameOverMenu(this);
    }

    private void Start()
    {
        menuButton.onClick.AddListener(OpenMenu);
        restartButton.onClick.AddListener(RestartGame);
        gameOverContainer.SetActive(false);

        EventSystem.current.SetSelectedGameObject(restartButton.gameObject);
    }

    private void RestartGame()
    {
        GlobalSettings.PendingRestart = true;
        SceneManager.LoadScene("Game");
        IsOpen = false;
    }

    private void OpenMenu()
    {
        Singletons.AudioManager.ResumeMusic();
        Singletons.AudioManager.ToMenu();
        SceneManager.LoadScene("MainMenu");
        IsOpen = false;
    }

    public void OpenGameOver()
    {
        gameOverContainer.SetActive(true);

        var gameModelTimingNotesCount = Singletons.GameModel.TimingNotesCount;
        perfectText.text = "x"+gameModelTimingNotesCount[TimingType.Perfect];
        greatText.text = "x"+gameModelTimingNotesCount[TimingType.Great];
        goodText.text = "x"+gameModelTimingNotesCount[TimingType.Good];
        badText.text = "x"+gameModelTimingNotesCount[TimingType.Bad];
        missText.text = "x"+gameModelTimingNotesCount[TimingType.Miss];

        IsOpen = true;
    }

    private void Update()
    {
        if (IsOpen)
        {
            if (!EventSystem.current.currentSelectedGameObject)
            {
                EventSystem.current.SetSelectedGameObject(restartButton.gameObject);
            }
        }
    }
}
