using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbitScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float speed;
    bool bOrbit = false;

    private void Start()
    {
        bOrbit = true; 
    }

    void Update()
    {
        if (bOrbit)
        {
            transform.LookAt(target);
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        
    }
}
