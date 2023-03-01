using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;

public class LoadScenes : MonoBehaviour
{
    public GameObject StartGame;

    public void LoadScene(string SceneName)
    {
        Time.timeScale = 1f;
        if (StartGame != null)
        {
            StartGame.GetComponent<Animator>().SetTrigger("End");
            StartCoroutine(LoadMenu(SceneName));
        }
        else if (StartGame == null)
        {
            SceneManager.LoadScene(sceneName: SceneName);
        }
    }

    private IEnumerator LoadMenu(string SceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName: SceneName);
    }

}
