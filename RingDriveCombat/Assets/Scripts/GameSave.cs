using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameSave
{
    public float horzSens;
    public float vertSens;
    public float effectsVolume;
    public float musicVolume;
    public float masterVolume;

    public GameSave (GameSettings gameSettings)
    {
        horzSens = gameSettings.horizontalMouseSensitivity;
        vertSens = gameSettings.verticalMouseSensitivity;
        effectsVolume = gameSettings.effectsVolume;
        musicVolume = gameSettings.musicVolume;
        masterVolume = gameSettings.masterVolume;
    }
}
