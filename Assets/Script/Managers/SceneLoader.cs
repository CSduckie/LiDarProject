using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    
    // 同步加载场景
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 异步加载场景
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }

    private System.Collections.IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        
        yield return new WaitForSeconds(1);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        
        while (!asyncLoad.isDone)
        {
            Debug.Log("加载进度：" + asyncLoad.progress);
            yield return null;
        }

    }

    // 重新加载当前场景
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        LoadScene(currentScene.name);
    }

    // 返回主菜
    public void ReturnToMainMenu()
    {
        LoadScene("MainMenu");
    }

    // 退出游戏
    public void QuitGame()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }
}
