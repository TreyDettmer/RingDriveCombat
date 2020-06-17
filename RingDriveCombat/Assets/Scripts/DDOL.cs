using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DDOL : MonoBehaviour {

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //load logo screens
        SceneManager.LoadScene("CalicoGamesSplash");

    }

    public void StartGame()
    {
        
        SceneManager.LoadScene("Scenes/MainGame");
    }
}
