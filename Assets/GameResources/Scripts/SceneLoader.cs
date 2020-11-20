using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private int nextSceneToLoad;

    private void Start()
    {
        nextSceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneToLoad);
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
