using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GPGSAuthentificator : MonoBehaviour
{
    private static PlayGamesPlatform _platform;
    
    private void Start()
    {
        if (_platform == null)
        {
            var config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            _platform = PlayGamesPlatform.Activate();
        }
        
        Social.Active.localUser.Authenticate(success =>
        {
            GlobalData.SceneLoading = 1;
            SceneManager.LoadScene($"LoadingScreen");
        });
    }
    
}
