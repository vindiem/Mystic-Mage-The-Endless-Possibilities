using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour
{
    public Image selectIndicatorButton;
    public Image selectIndicatorJoystick;
    private string movementType;

    public Slider volumeSlider;
    public Slider qualitySlider;

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        qualitySlider.value = PlayerPrefs.GetInt("Quality");

    }

    private void Update()
    {
        movementType = PlayerPrefs.GetString("MovementType");

        if (movementType == "buttons")
        {
            selectIndicatorButton.gameObject.SetActive(true);
            selectIndicatorJoystick.gameObject.SetActive(false);
        }
        else if (movementType == "joystick")
        {
            selectIndicatorButton.gameObject.SetActive(false);
            selectIndicatorJoystick.gameObject.SetActive(true);
        }
    }

    public void MovementSet(bool type)
    {
        // type == true >> buttons
        // type == false >> joystick

        if (type == true)
        {
            PlayerPrefs.SetString("MovementType", "buttons");

            selectIndicatorButton.gameObject.SetActive(true);
            selectIndicatorJoystick.gameObject.SetActive(false);
        }
        else if (type == false)
        {
            PlayerPrefs.SetString("MovementType", "joystick");

            selectIndicatorButton.gameObject.SetActive(false);
            selectIndicatorJoystick.gameObject.SetActive(true);
        }
    }

    public void VolumeSet()
    {
        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volumeValue);
    }

    public void QualitySet()
    {
        int qualityValue = (int)qualitySlider.value;
        PlayerPrefs.SetInt("Quality", qualityValue);
        QualitySettings.SetQualityLevel(qualityValue, true);
    }

}
