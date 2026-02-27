using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeSceneNext(string nextSceneName)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
