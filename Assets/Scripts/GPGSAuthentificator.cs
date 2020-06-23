using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GPGSAuthentificator : MonoBehaviour
{
    public static PlayGamesPlatform Platform;
    
    private void Start()
    {
        if (Platform == null)
        {
            var config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            Platform = PlayGamesPlatform.Activate();
        }
        
        Social.Active.localUser.Authenticate(success =>
        {
            GlobalData.SceneLoading = 1;
            SceneManager.LoadScene($"LoadingScreen");
        });
    }
    // TODO: Add Static Methods to Update Score etc...
}
