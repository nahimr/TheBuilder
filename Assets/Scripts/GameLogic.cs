using System;
using GooglePlayGames.BasicApi;
using UI;
using UnityEngine;

public static class GlobalData
{
    public static int NumberOfBricksToWin;
    public static bool IsSmartphone;
    public static int NumberOfBricksOnFloor;
    public static int SceneLoading;
    public static int ActualLevel;
    public static string LevelsJson = Resources.Load<TextAsset>("Levels").text;
    
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
    public int levelId;
    public int numberOfBricksToWin;
    public int numberOfRowsTower;
    public bool isSmartphone;
    public float timer;
    private float _tmpTimer;
    private bool _gameFinished;
    private bool _levelWon;
    private float _timeScale;
    public Player player;
    private int _maxLevels;
    public static GameLogic Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        GlobalData.NumberOfBricksOnFloor = 0;
        _timeScale = 1.0f;
        SetTimeGame(1.0f);
        GlobalData.IsSmartphone = isSmartphone;
        var levelsDatas = JsonHelper.GetJsonArray<LevelData>(GlobalData.LevelsJson);
        var levelData = levelsDatas[GlobalData.ActualLevel];
        levelId = levelData.id;
        numberOfBricksToWin = levelData.numberOfBricksToWin;
        timer = levelData.timeToComplete;
        GlobalData.NumberOfBricksToWin = numberOfBricksToWin;
        _maxLevels = levelsDatas.Length;
        
    }

    private void Start()
    {
        _tmpTimer = 0.0f;
        _gameFinished = false;
        _levelWon = false;
        GameEvents.Current.OnEndGame += AchievementEvent;
        UI_InGame.Instance.pauseButton.onClick.AddListener(() => PauseGame(true));
        UI_InGame.Instance.resumeButton.onClick.AddListener(() => PauseGame(false));
        UI_InGame.Instance.joysticks.SetActive(isSmartphone);
        UI_InGame.Instance.pauseButton.interactable = true;
    }


    private void AchievementEvent(int lvl, bool haveWon)
    {
        switch (lvl)
        {
            case 0 when haveWon:
                try
                {
                    GPGSAuthentificator.Platform.ReportProgress(GPGSIds.achievement_youre_beginner, 100.0f,
                        success =>
                    {
                        if(success)
                            Social.ShowAchievementsUI();
                    });
                }
                catch (NullReferenceException)
                {
                    
                }
                break;
        }
    }
    
    private void Update()
    {
        UI_InGame.Instance.healthBar.value = player.health;
        UI_InGame.Instance.staminaBar.value = player.stamina;
        UI_InGame.Instance.scoreText.text = $"Score: {Tower.Instance.NumberOfBricksPlaced} / {GlobalData.NumberOfBricksToWin}";
    }

    private void FixedUpdate()
    {
        _tmpTimer += Time.fixedDeltaTime;
        SetTimer(timer - _tmpTimer);
        if (_tmpTimer >= timer || GlobalData.NumberOfBricksToWin == Tower.Instance.NumberOfBricksPlaced || player.health <= 0.0f)
        {
            EndGame();
        }
    }
    private void PauseGame(bool value)
    {
        if (value)
        {
            SetTimeGame(0.0f);
            UI_InGame.Instance.pauseMenu.SetActive(true);
        }
        else
        {
            SetTimeGame(_timeScale);
            UI_InGame.Instance.pauseMenu.SetActive(false);
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
        UI_InGame.Instance.timerText.text = $"{minutes}:{seconds}";
    }
    private void EndGame()
    {
        SetTimeGame(0.0f);
        UI_InGame.Instance.gameFinished.SetActive(true);
        _gameFinished = true;
        if (Tower.Instance.NumberOfBricksPlaced == GlobalData.NumberOfBricksToWin)
        {
            
            _levelWon = true;
            UI_InGame.Instance.resultText.text = "Win !!";

            if (GlobalData.ActualLevel + 1 < _maxLevels)
            {
                UI_InGame.Instance.finalNextLevelButton.gameObject.SetActive(true);
                UI_InGame.Instance.finalNextLevelButton.onClick.AddListener(() =>
                {
                    GlobalData.ActualLevel++;
                    StaticBuilder.ReloadLevel();
                });
            }
            GPGSAuthentificator.Platform.Events.FetchEvent(DataSource.ReadNetworkOnly,
                GPGSIds.event_levels_finished,
                (responseStatus, typeEvent) =>
                {
                    Debug.Log($"Lvl Event Count:{typeEvent.CurrentCount}");
                    if (responseStatus != ResponseStatus.Success) return;
                    if (typeEvent.CurrentCount < (ulong) GlobalData.ActualLevel + 1)
                    {
                        GPGSAuthentificator.Platform.Events.IncrementEvent(GPGSIds.event_levels_finished, 1);
                    }
                });
        }
        else
        {
            UI_InGame.Instance.finalNextLevelButton.gameObject.SetActive(false);
            UI_InGame.Instance.resultText.text = "Loose !!";
        }
        GameEvents.Current.EndGame(GlobalData.ActualLevel, _levelWon);
        UI_InGame.Instance.finalScoreText.text = $"Score: {Tower.Instance.NumberOfBricksPlaced} / {GlobalData.NumberOfBricksToWin}";
        var minutes = Mathf.Floor(_tmpTimer / 60);
        var seconds = Mathf.RoundToInt(_tmpTimer % 60);
        UI_InGame.Instance.finalTimeText.text = $"Time: {minutes}:{seconds}";
        _tmpTimer = 0.0f;
        GPGSAuthentificator.Platform.ReportScore(Tower.Instance.NumberOfBricksPlaced, GPGSIds.leaderboard_best_players, null);
    }
}
