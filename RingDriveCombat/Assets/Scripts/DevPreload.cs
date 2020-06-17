using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevPreload : MonoBehaviour {

    void Awake()
    {
        GameObject check = GameObject.Find("__app");
        Debug.Log("PENIS");
        if (check == null)
        { UnityEngine.SceneManagement.SceneManager.LoadScene("_preload"); }
        
    }
}
