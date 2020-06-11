using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class GlobalData
{
    public static float Health = 100.0f;
    public static float Stamina = 0.0f;
    public static int NumberOfBricksToWin;
    public static int NumberOfBricksPlaced;
    public static bool IsSmartphone;
    public static int NumberOfBricksOnFloor;
    public static int NumberOfRowsTower = 6;
    public static int SceneLoading;
    
    public static void ScaleAround(Transform target, Transform pivot, Vector3 scale) {
        var pivotParent = pivot.parent;
        var pivotPos = pivot.position;
        pivot.parent = target;        
        target.localScale = scale;
        target.position += pivotPos - pivot.position;
        pivot.parent = pivotParent;
    }
    
}

public class GameLogic : MonoBehaviour
{
    public Slider healthBar;
    public Slider staminaBar;
    public Text scoreText;
    public Text timerText;
    public GameObject joysticks;
    public Button pauseMenuButton;
    public Button resumeGameButton;
    public GameObject pauseMenu;
    
    public GameObject gameFinished;
    public Text gameFinishedScoreText;
    public Text gameFinishedTimeText;
    public Button gameFinishedRetryButton;
    public GameObject gameFinishedWinText;
    public int numberOfBricksToWin;
    public bool isSmartphone;
    public float timer;
    private float _tmpTimer;
    private bool _gameFinished;
    private bool _levelWon;
    private float _timeScale;
    private void Awake()
    {
        GlobalData.NumberOfBricksPlaced = 0;
        GlobalData.NumberOfBricksOnFloor = 0;
        _timeScale = 1.0f;
        SetTimeGame(1.0f);
        pauseMenuButton.onClick.AddListener(() => PauseGame(true));
        resumeGameButton.onClick.AddListener(() => PauseGame(false));
        GlobalData.NumberOfBricksToWin = numberOfBricksToWin;
        GlobalData.IsSmartphone = isSmartphone || Application.platform == RuntimePlatform.Android;
        #if UNITY_ANDROID
        GlobalData.IsSmartphone = true;
        #endif
        joysticks.SetActive(isSmartphone);
        _tmpTimer = 0.0f;
        _gameFinished = false;
        _levelWon = false;
    }
    

    private void Update()
    {
        healthBar.value = GlobalData.Health;
        staminaBar.value = GlobalData.Stamina;
        scoreText.text = $"Score: {GlobalData.NumberOfBricksPlaced} / {GlobalData.NumberOfBricksToWin}";
    }

    private void FixedUpdate()
    {
        _tmpTimer += Time.fixedDeltaTime;
        SetTimer(timer - _tmpTimer);
        if (_tmpTimer >= timer || GlobalData.NumberOfBricksToWin == GlobalData.NumberOfBricksPlaced || GlobalData.Health <= 0.0f)
        {
            EndGame();
        }
    }
    private void PauseGame(bool value)
    {
        if (value)
        {
            SetTimeGame(0.0f);
            pauseMenu.SetActive(true);
        }
        else
        {
            SetTimeGame(_timeScale);
            pauseMenu.SetActive(false);
        }
    }

    private static void SetTimeGame(float value)
    {
        Time.timeScale = value;
    }
    
    private void SetTimer(float value)
    {
        var minutes = Mathf.Floor(value / 60);
        var seconds = Mathf.RoundToInt(value % 60);
        timerText.text = $"{minutes}:{seconds}";
    }
    
    
    
    private void EndGame()
    {
        SetTimeGame(0.0f);
        gameFinished.SetActive(true);
        _gameFinished = true;
        if (GlobalData.NumberOfBricksPlaced == GlobalData.NumberOfBricksToWin)
        {
            _levelWon = true;
            gameFinishedWinText.SetActive(true);
        }
        gameFinishedScoreText.text = $"Score: {GlobalData.NumberOfBricksPlaced} / {GlobalData.NumberOfBricksToWin}";
        var minutes = Mathf.Floor(_tmpTimer / 60);
        var seconds = Mathf.RoundToInt(_tmpTimer % 60);
        gameFinishedTimeText.text = $"Time: {minutes}:{seconds}";
        _tmpTimer = 0.0f;
    }
    
    

    
}
