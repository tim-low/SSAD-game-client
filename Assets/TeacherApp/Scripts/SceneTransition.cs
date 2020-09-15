using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneTransition : MonoBehaviour {

    [SerializeField]
    private string SceneName;
    public static string prev;
    public string currentSceneName;

    public void SceneLoader(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    public string GetSceneName()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log(currentSceneName);
        return currentSceneName;
    }

    public void SceneLoader(string SceneName)
    {
        prev = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(SceneName);
    }

    public void ConditionalSceneLoader()
    {
        if (prev == "CreateNewSessionScene2")
            SceneManager.LoadScene("CreateNewSessionScene2");
        else if (prev == "AddqnScene10")
            SceneManager.LoadScene("AddqnScene10");
        else if (prev == "MainMenuScene1")
            SceneManager.LoadScene("MainMenuScene1");

    }
}
