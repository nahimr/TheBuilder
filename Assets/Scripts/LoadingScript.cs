using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    public Slider loaderScreen;
    private void Start()
    {
        StartCoroutine(LoadAsyncOperation());
    }

    private IEnumerator LoadAsyncOperation()
    {
        var gameLevel = SceneManager.LoadSceneAsync(GlobalData.SceneLoading);

        while (gameLevel.progress < 1)
        {
            loaderScreen.value = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }
    }
}
