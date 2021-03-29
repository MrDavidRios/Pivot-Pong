using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private void Awake()
    {

    }

    public void FadeToLevel(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }
}