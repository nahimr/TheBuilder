using UnityEngine;
using UnityEngine.SceneManagement;
public class StaticBuilder : MonoBehaviour
{
    public void LoadLevel(int lvl)
    {
        GlobalData.SceneLoading = lvl;
        SceneManager.LoadScene($"LoadingScreen");
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
