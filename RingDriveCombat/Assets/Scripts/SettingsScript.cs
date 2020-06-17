using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SettingsScript : MonoBehaviour {
    public float vertSensSlider = .7f;
    public float horzSensSlider = .7f;
    public float masterVolume = .5f;
    public float effectsVolume = .5f;
    public float musicVolume = .5f;
    float tempVertSens = 1.0f;
    float tempHorzSens = 1.0f;
    float tempMasterVolume = .5f;
    float tempEffectsVolume = .5f;
    float tempMusicVolume = .5f;
    public bool hasApplied = false;

    public void LoadPreviousScene()
    {

        string prevScene = GameObject.Find("_app").GetComponent<GameData>().previousSceneName;
        UnityEngine.SceneManagement.SceneManager.LoadScene(prevScene);
    }


    /*
    public void LoadedSettings()
    {
        tempHorzSens = horzSensSlider;
        tempVertSens = vertSensSlider;
        vertSlider.value = tempVertSens;
        horzSlider.value = tempHorzSens;

    }
    */

    public void SetSettings(float vert, float horz,float effectsVol,float musicVol,float mastervol)
    {
        tempVertSens = vert;
        tempHorzSens = horz;
        tempEffectsVolume = effectsVol;
        tempMusicVolume = musicVol;
        tempMasterVolume = mastervol;
    }

    /*
    public void SetVertandHorzSens()
    {
        tempVertSens = vertSlider.value;
        tempHorzSens = horzSlider.value;
    }
    */


    public void ApplyChanges()
    {
        hasApplied = true;
        horzSensSlider = tempHorzSens;
        vertSensSlider = tempVertSens;
        effectsVolume = tempEffectsVolume;
        musicVolume = tempMusicVolume;
        masterVolume = tempMasterVolume;
        
        GameObject.Find("_app").GetComponent<GameSettings>().horizontalMouseSensitivity = horzSensSlider;
        GameObject.Find("_app").GetComponent<GameSettings>().verticalMouseSensitivity = vertSensSlider;
        GameObject.Find("_app").GetComponent<GameSettings>().UpdateMasterVolume(masterVolume);
        GameObject.Find("_app").GetComponent<GameSettings>().UpdateMusicVolume(musicVolume);
        GameObject.Find("_app").GetComponent<GameSettings>().UpdateEffectsVolume(effectsVolume);
        GameObject.Find("_app").GetComponent<GameSettings>().UpdatePlayerSettings();
        GameObject.Find("_app").GetComponent<GameSettings>().SaveSettings();
    }



}
