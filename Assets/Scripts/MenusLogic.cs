using System.Collections.Generic;
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
        if (!GPGSAuthentificator.Platform.IsAuthenticated())
        {
            leaderboard.SetActive(false);
            return;
        }

        leaderboard.SetActive(true);
        GPGSAuthentificator.Platform.LoadScores(GPGSIds.leaderboard_best_players, callback =>
        {
            var scores = callback;
            var userids = new string[scores.Length];
            
            for (var i = 0; i < scores.Length; i++)
            {
                userids[i] = scores[i].userID;
            }
            
            Social.LoadUsers(userids, profiles => DisplayLeaderBoard(scores, profiles));
        });
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
