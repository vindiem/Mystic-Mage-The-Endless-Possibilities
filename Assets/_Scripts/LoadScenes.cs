using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public Text bestScoreText;
    private float bestScore;
    public GameObject StartGame;

    private void Update()
    {
        if (bestScoreText != null)
        {
            bestScore = PlayerPrefs.GetFloat("BestScore");
            bestScoreText.text = $"your best score (secs): " + bestScore;
        }
    }

    public void LoadScene(string SceneName)
    {
        if (StartGame != null)
        {
            StartGame.GetComponent<Animator>().SetTrigger("End");
            StartCoroutine(LoadMenu());
        }
        else
        {
            SceneManager.LoadScene(sceneName: SceneName);
        }
    }

    private IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }

}
