using UnityEngine.SceneManagement;
public static class StaticBuilder
{
    public static void LoadLevel(int lvl)
    {
        GlobalData.SceneLoading = lvl;
        SceneManager.LoadScene($"LoadingScreen");
    }

    public static void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
