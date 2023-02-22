using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public GameObject StartGame;

    public void LoadScene(string SceneName)
    {
        if (StartGame != null)
        {
            StartGame.GetComponent<Animator>().SetTrigger("End");
        }
        StartCoroutine(LoadMenu(SceneName));
    }

    private IEnumerator LoadMenu(string SceneName)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName: SceneName);
    }

}
