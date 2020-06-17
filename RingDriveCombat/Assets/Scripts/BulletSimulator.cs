using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSimulator : MonoBehaviour {

    // Use this for initialization
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float travelTime;
    private float timer;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        transform.position = Vector3.Lerp(startPosition, endPosition, timer / travelTime);
        if (timer >= travelTime)
        {
            
            Destroy(gameObject);
        }
	}

    public void SetValues(Vector3 start,Vector3 end,float time)
    {
        startPosition = start;
        endPosition = end;
        travelTime = time;
        timer = 0f;
    }
}
