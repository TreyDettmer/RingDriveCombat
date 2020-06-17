using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxCamera : MonoBehaviour
{
    public RingManager ring;
    public int axis = 3;
    void FixedUpdate()
    {
        if (axis == 3)
        {
            transform.Rotate(0f, 0f, ring.currentSpeed, Space.Self);
        }
        else if (axis == 1)
        {
            transform.Rotate(ring.currentSpeed,0f, 0f, Space.Self);
        }
        else if (axis == 2)
        {
            transform.Rotate( 0f, -ring.currentSpeed, 0f, Space.Self);
        }
    }
}
