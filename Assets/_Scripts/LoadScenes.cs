using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public Text bestScoreTimeText;
    private float bestScoreTime;

    public Text bestScoreKillsText;
    private int bestScoreKills;

    public GameObject StartGame;

    private void Update()
    {
        if (bestScoreTimeText != null)
        {
            bestScoreTime = PlayerPrefs.GetFloat("BestScore");
            bestScoreTimeText.text = $"best score: " + bestScoreTime.ToString("0") + " seconds";
        }
        if (bestScoreKillsText != null)
        {
            bestScoreKills = PlayerPrefs.GetInt("BestScoreKills");
            bestScoreKillsText.text = $"kills: " + bestScoreKills.ToString("0");
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
