using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DummySettings : MonoBehaviour {
    public float vertSensSlider;
    public float horzSensSlider;
    float tempVertSens;
    float tempHorzSens;
    float tempMasterVolume;
    float tempMusicVolume;
    float tempEffectsVolume;
    public Slider horzSlider;
    public Slider vertSlider;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider effectsVolumeSlider;
    public bool hasApplied = false;
    // Use this for initialization
    void Start () {
        LoadedSettings();
        tempHorzSens = GameObject.Find("_app").GetComponent<GameSettings>().horizontalMouseSensitivity;
        horzSlider.value = tempHorzSens;
        tempMasterVolume = GameObject.Find("_app").GetComponent<GameSettings>().masterVolume;
        masterVolumeSlider.value = tempMasterVolume;
        tempEffectsVolume = GameObject.Find("_app").GetComponent<GameSettings>().effectsVolume;
        effectsVolumeSlider.value = tempEffectsVolume;
        tempMusicVolume = GameObject.Find("_app").GetComponent<GameSettings>().musicVolume;
        musicVolumeSlider.value = tempMusicVolume;

        //Debug.Log("Dummy started");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ButtonHover()
    {
        FindObjectOfType<AudioManager>().Play("ButtonHover");
    }
    public void ButtonClick()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }

    public void LoadPreviousScene()
    {
        GameObject.Find("_app").GetComponent<SettingsScript>().LoadPreviousScene();
    }

    public void SetVertandHorzSens()
    {
        tempVertSens = vertSlider.value;
        tempHorzSens = horzSlider.value;
        tempMasterVolume = masterVolumeSlider.value;
        tempEffectsVolume = effectsVolumeSlider.value;
        tempMusicVolume = musicVolumeSlider.value;
        GameObject.Find("_app").GetComponent<SettingsScript>().SetSettings(tempVertSens, tempHorzSens,tempEffectsVolume,tempMusicVolume,tempMasterVolume);
    }

    public void LoadedSettings()
    {
        tempVertSens = GameObject.Find("_app").GetComponent<GameSettings>().verticalMouseSensitivity;
        tempHorzSens = GameObject.Find("_app").GetComponent<GameSettings>().horizontalMouseSensitivity;
        tempMasterVolume = GameObject.Find("_app").GetComponent<GameSettings>().masterVolume;
        tempEffectsVolume = GameObject.Find("_app").GetComponent<GameSettings>().effectsVolume;
        tempMusicVolume = GameObject.Find("_app").GetComponent<GameSettings>().musicVolume;
        GameObject.Find("_app").GetComponent<GameSettings>().UpdatePlayerSettings();
        vertSlider.value = tempVertSens;
        horzSlider.value = tempHorzSens;
        masterVolumeSlider.value = tempMasterVolume;
        musicVolumeSlider.value = tempMusicVolume;
        effectsVolumeSlider.value = tempEffectsVolume;

    }

    public void ApplyChanges()
    {
        SetVertandHorzSens();
        GameObject.Find("_app").GetComponent<SettingsScript>().ApplyChanges();
    }
}
