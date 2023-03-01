using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public Image pausePanel;

    public Skills playerScript;
    private float defaultGameSpeed;

    private bool isActiveMenu = false;

    private void Start()
    {
        pausePanel.gameObject.SetActive(false);
        defaultGameSpeed = playerScript.gameSpeed;    
    }

    public void PauseSet()
    {
        isActiveMenu = !isActiveMenu;

        switch (isActiveMenu)
        {
            case true:
                Time.timeScale = 0f;
                pausePanel.gameObject.SetActive(true);
                break;

            case false:
                Time.timeScale = defaultGameSpeed;
                pausePanel.gameObject.SetActive(false);
                break;
        }
    }
}
