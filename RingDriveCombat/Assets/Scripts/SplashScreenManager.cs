using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SplashScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    VideoPlayer player;
    Coroutine co;
    void Start()
    {
        GetComponent<VideoPlayer>().Play();
        co = StartCoroutine(WaitToStartNextScene());

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)){ 
            if (co != null)
            {
                StopCoroutine(co);
                SceneManager.LoadScene("StartMenu");
                FindObjectOfType<AudioManager>().currentScene = Scene.StartMenu;
                FindObjectOfType<AudioManager>().PlayMusic(0);
            }

        }
    }

    IEnumerator WaitToStartNextScene()
    {
        yield return new WaitForSeconds(9.1f);
        
        SceneManager.LoadScene("StartMenu");
        FindObjectOfType<AudioManager>().currentScene = Scene.StartMenu;
        FindObjectOfType<AudioManager>().PlayMusic(0);


    }


}
