using System;
using System.Collections.Generic;
using GooglePlayGames.BasicApi;
using UI;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class MenusLogic : MonoBehaviour
{
    public GameObject shopMenu;
    public GameObject mainMenu;
    public GameObject levelsMenu;
    public GameObject leaderboard;
    public GameObject leaderboardContent;
    public GameObject leaderboardObject;
    public GameObject levelsContent;
    public GameObject levelButtonPrefab;

    private void Awake()
    {
        var jsonContent = JsonHelper.GetJsonArray<LevelData>(GlobalData.LevelsJson);
        try
        {
            GPGSAuthentificator.Platform.Events.FetchEvent(DataSource.ReadNetworkOnly,
                GPGSIds.event_levels_finished,
                (responseStatus, typeEvent) =>
                {
                    if (responseStatus != ResponseStatus.Success) return;
                    UpdateLevelsMenu(typeEvent.CurrentCount, jsonContent);
                });  
        }
        catch (NullReferenceException)
        {
            UpdateLevelsMenu((ulong)jsonContent.Length, jsonContent);
        }
    }

    private void UpdateLevelsMenu(ulong currentLevelsWon, IReadOnlyCollection<LevelData> jsonContent)
    {
        for (var i = 0; i < jsonContent.Count; i++)
        {
            var bt = Instantiate(levelButtonPrefab, levelsContent.transform);
            bt.GetComponentInChildren<Text>().text = i.ToString();
            var button = bt.GetComponent<Button>();
            var i1 = i;
            button.onClick.AddListener(() =>
            {
                GlobalData.ActualLevel = i1;
                StaticBuilder.LoadScene(3);
            });
            if ((ulong) i <=  currentLevelsWon)
            {
                button.interactable = true;
            }
        }
    }
    
    private void Start()
    {
        ShowLeaderboard();
    }

    public void MainMenu(bool pEnabled)
    {
        mainMenu.SetActive(pEnabled);
    }

    public void LevelsMenu(bool pEnabled)
    {
        if (pEnabled)
        {
            shopMenu.SetActive(false);
            mainMenu.SetActive(false);
            levelsMenu.SetActive(true);
        }
        else
        {
            shopMenu.SetActive(false);
            mainMenu.SetActive(true);
            levelsMenu.SetActive(false);
        }
    }

    public void ShopMenu(bool pEnabled)
    {
        shopMenu.SetActive(pEnabled);
        mainMenu.SetActive(!pEnabled);
    }

    private void ShowLeaderboard()
    {

        try
        {
            if (!GPGSAuthentificator.Platform.IsAuthenticated())
            {
                leaderboard.SetActive(false);
                return;
            }

            leaderboard.SetActive(true);
            GPGSAuthentificator.Platform.LoadScores(GPGSIds.leaderboard_best_players, callback =>
            {
                var scores = callback;
                var userIds = new string[scores.Length];
            
                for (var i = 0; i < scores.Length; i++)
                {
                    userIds[i] = scores[i].userID;
                }
                Social.LoadUsers(userIds, profiles => DisplayLeaderBoard(scores, profiles));
            });
        }
        catch (NullReferenceException)
        {
            leaderboard.SetActive(false);
        }
    }
    private void DisplayLeaderBoard(IReadOnlyList<IScore> scores, IReadOnlyList<IUserProfile> profiles)
    {
        for (var i = 0; i < scores.Count; i++)
        {
            var elementLeaderboard = Instantiate(leaderboardObject, leaderboardContent.transform.position + Vector3.down * 16,
                Quaternion.identity, leaderboardContent.transform).GetComponent<Text>();
            elementLeaderboard.text = $"{profiles[i].userName}        {scores[i].value}";
        }
    }
}
