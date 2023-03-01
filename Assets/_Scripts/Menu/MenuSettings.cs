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
    public Slider masterTextureLimitSlider;
    public Slider AntiAliasingSlider;
    public Slider LodBiasSlider;
    public Slider VSyncSlider;

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        qualitySlider.value = PlayerPrefs.GetInt("Quality");
        masterTextureLimitSlider.value = PlayerPrefs.GetInt("MasterTextureLimit");
        AntiAliasingSlider.value = PlayerPrefs.GetInt("AntiAliasing");
        VSyncSlider.value = PlayerPrefs.GetInt("VSync");
        LodBiasSlider.value = PlayerPrefs.GetFloat("LodBias");
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

    public void MasterTextureLimitSet()
    {
        int masterTextureLimitValue = (int)masterTextureLimitSlider.value;
        PlayerPrefs.SetInt("MasterTextureLimit", masterTextureLimitValue);
        QualitySettings.masterTextureLimit = masterTextureLimitValue;
    }

    public void AntiAliasingSet()
    {
        int antiAliasingValue = (int)AntiAliasingSlider.value;
        PlayerPrefs.SetInt("AntiAliasing", antiAliasingValue);
        QualitySettings.antiAliasing = antiAliasingValue;
    }
    
    public void VSyncSet()
    {
        int VSyncValue = (int)VSyncSlider.value;
        PlayerPrefs.SetInt("VSync", VSyncValue);
        QualitySettings.vSyncCount = VSyncValue;
    }

    public void LodBiasSet()
    {
        float LodBiasValue = LodBiasSlider.value;
        PlayerPrefs.SetFloat("LodBias", LodBiasValue);
        QualitySettings.lodBias = LodBiasValue;
    }


}
