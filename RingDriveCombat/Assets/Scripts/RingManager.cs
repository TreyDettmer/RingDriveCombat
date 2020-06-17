using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviour {
    public float rotationSpeed = 5f;
    public float fastSpeedMultiplier = 1.1f;
    public float maxSpeed = -20f;
    public float currentSpeed;
    public bool goFast = false;
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!LevelManager.bPaused)
        {
            if (goFast == false)
            {
                currentSpeed = rotationSpeed;
                transform.Rotate(0f, 0f, currentSpeed, Space.Self);
            }
            else
            {
                
                if (currentSpeed <= maxSpeed)
                {
                    currentSpeed = maxSpeed;
                }
                else
                {
                    currentSpeed *= fastSpeedMultiplier;
                }
                transform.Rotate(0f, 0f, currentSpeed, Space.Self);
            }
        }
	}
}
