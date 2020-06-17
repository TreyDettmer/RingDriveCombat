using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettings : MonoBehaviour {

    // Use this for initialization
    public float verticalMouseSensitivity = 5f;
    public float horizontalMouseSensitivity = 5f;
    public float masterVolume = 5f;
    public float musicVolume = 5f;
    public float effectsVolume = 5f;
    public bool bInputEnabled = true;


	void Start () {
        LoadSettings();
        UpdateMasterVolume(masterVolume);
        UpdateEffectsVolume(effectsVolume);
        UpdateMusicVolume(musicVolume);
	}

    public void UpdatePlayerSettings()
    {
        if (GameObject.Find("Player"))
        {
            //Debug.Log("HorzSens = " + horizontalMouseSensitivity);
            //Debug.Log("VertSens = " + verticalMouseSensitivity);
            GameObject.Find("Player").GetComponent<PlayerController>().horizontalLookSpeed = (horizontalMouseSensitivity / 7.142857f);
            GameObject.Find("Player").GetComponent<PlayerController>().verticalLookSpeed = (verticalMouseSensitivity / 7.142857f);
        }
    }

    public void UpdateMasterVolume(float newVolume)
    {
        masterVolume = newVolume;
        AudioListener.volume = (masterVolume / 11f);
    }

    public void UpdateEffectsVolume(float newVolume)
    {
        effectsVolume = newVolume;
    }
    public void UpdateMusicVolume(float newVolume)
    {
        musicVolume = newVolume;
        FindObjectOfType<AudioManager>().UpdateMusicVolume();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SaveSettings()
    {
        SaveSystem.SaveSettings(this);
    }

    public void LoadSettings()
    {
        GameSave gameSave = SaveSystem.LoadGameSave();
        if (gameSave != null)
        {
            verticalMouseSensitivity = gameSave.vertSens;
            horizontalMouseSensitivity = gameSave.horzSens;
            UpdateMasterVolume(gameSave.masterVolume);
            UpdateEffectsVolume(gameSave.effectsVolume);
            UpdateMusicVolume(gameSave.musicVolume);
            //Debug.Log("Loaded Settings from file");
            //Debug.Log("Vert: " + verticalMouseSensitivity + " Horz: " + horizontalMouseSensitivity + " volume: " + gameSave.masterVolume);
        }
    }
}
