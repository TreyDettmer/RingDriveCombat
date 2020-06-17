using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour {

    // Use this for initialization

    private void Start()
    {
        FindObjectOfType<AudioManager>().currentScene = Scene.StartMenu;
        FindObjectOfType<GameSettings>().LoadSettings();
    }
    // Update is called once per frame
    void Update () {
		
	}

    public void StartGame()
    {
        FindObjectOfType<AudioManager>().currentScene = Scene.Loading;
        FindObjectOfType<AudioManager>().StopMusic();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameLoadingScene");
    }

    public void GoToSettings()
    {
        GameObject.Find("_app").GetComponent<GameData>().previousSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameSettings");
    }

    public void ButtonHover()
    {
        FindObjectOfType<AudioManager>().Play("ButtonHover");
    }
    public void ButtonClick()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
