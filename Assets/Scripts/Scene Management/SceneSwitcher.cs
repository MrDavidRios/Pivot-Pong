using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void FadeToLevel(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }
}