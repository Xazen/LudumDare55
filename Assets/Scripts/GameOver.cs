using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite lostSprite;
    [SerializeField] private Sprite wonSprite;

    [SerializeField] private GameObject gameOverContainer;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button restartButton;

    [SerializeField] private TMP_Text totalScoreText;
    [SerializeField] private TMP_Text maxComboText;
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

    public void OpenGameOver(bool didWin)
    {
        gameOverContainer.SetActive(true);

        image.sprite = didWin ? wonSprite : lostSprite;

        totalScoreText.text = Singletons.GameModel.Score.ToString("N0");
        maxComboText.text = "x"+Singletons.GameModel.MaxCombo;
        UpdateText(TimingType.Perfect, perfectText);
        UpdateText(TimingType.Great, greatText);
        UpdateText(TimingType.Good, goodText);
        UpdateText(TimingType.Bad, badText);
        UpdateText(TimingType.Miss, missText);

        IsOpen = true;
    }

    private void UpdateText(TimingType timingType, TMP_Text text)
    {
        var gameModelTimingNotesCount = Singletons.GameModel.TimingNotesCount;
        if (gameModelTimingNotesCount.TryGetValue(timingType, out var value))
        {
            text.text = "x"+value;
        }
        else
        {
            text.text = "x0";
        }
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
