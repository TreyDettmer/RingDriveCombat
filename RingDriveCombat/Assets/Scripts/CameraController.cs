using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform carTransform;
    public Transform lowTransform;
    public Transform highTransform;
    public PlayerController player;
    public Transform playerModel;
    public float lowLookThreshold = .4f;
    public float highLookThreshold = -.4f;
    public float carLerpSpeed = 10f;
    public float carSlerpSpeed = 10f;
    Vector3 basePosition;
    public float orbitSpeed = 1f;
    bool bOrbitView = false;
    bool deathCam = false;
    Vector3 offsetX;
    Vector3 offsetY;
    public float height;
    public float distance;
    public float deathHeight;
    public float deathDistance;
    GameSettings gameSettings;
    
	// Use this for initialization
	void Start () {
        basePosition = carTransform.localPosition;
        offsetX = new Vector3(0, height, distance);
        offsetY = new Vector3(0, 0, distance);
        gameSettings = GameObject.Find("_app").GetComponent<GameSettings>();
    }

    public void PlayerDied()
    {
        deathCam = true;
        bOrbitView = true;
        offsetX = new Vector3(0, deathHeight, deathDistance);
        offsetY = new Vector3(0, 0, deathDistance);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!bOrbitView)
            {
                gameSettings.bInputEnabled = false;
                bOrbitView = true;
            }
            else
            {
                if (!player.dead)
                {
                    gameSettings.bInputEnabled = true;
                    bOrbitView = false;
                }
            }

        }
    }

    // Update is called once per frame
    void FixedUpdate() {


        if (!bOrbitView)
        {
            if (carTransform)
            {
                if (transform.rotation.x > 0)
                {
                    float percent = transform.rotation.x / lowLookThreshold;
                    carTransform.localPosition = Vector3.Lerp(basePosition, highTransform.localPosition, percent);
                }
                else
                {
                    float percent = transform.rotation.x / highLookThreshold;
                    carTransform.localPosition = Vector3.Lerp(basePosition, lowTransform.localPosition, percent);
                }
                transform.position = Vector3.Lerp(transform.position, carTransform.position, Time.deltaTime * carLerpSpeed);
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(carTransform.rotation.eulerAngles.x, carTransform.rotation.eulerAngles.y, 0f), Time.deltaTime * carSlerpSpeed);
                transform.rotation = Quaternion.Euler(carTransform.rotation.eulerAngles.x, carTransform.rotation.eulerAngles.y, 0f);
            }
        }
        else if (playerModel)
        {
            if (!deathCam)
            {
                offsetX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * orbitSpeed, Vector3.up) * offsetX;
                offsetY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * orbitSpeed, Vector3.right) * offsetY;
                transform.position = playerModel.position + offsetX + offsetY;
                transform.LookAt(playerModel.position);
            }
            else
            {
                

                offsetX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * orbitSpeed, Vector3.up) * offsetX;
                offsetY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * orbitSpeed, Vector3.right) * offsetY;
                transform.position = playerModel.position + offsetX + offsetY;
                transform.LookAt(playerModel.position);
            }
        }


	}
}
