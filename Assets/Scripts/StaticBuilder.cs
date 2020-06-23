using UnityEngine;
using UnityEngine.SceneManagement;
public class StaticBuilder : MonoBehaviour
{
    public static void LoadScene(int lvl)
    {
        GlobalData.SceneLoading = lvl;
        SceneManager.LoadScene($"LoadingScreen");
    }

    public static void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NoStatic_LoadScene(int lvl)
    {
        GlobalData.SceneLoading = lvl;
        SceneManager.LoadScene($"LoadingScreen");
    }
}
