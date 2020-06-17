using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Collider> RagdollParts = new List<Collider>();
    private void Awake()
    {
        SetRagdollParts();
        
    }

    private void SetRagdollParts()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            if (c.gameObject != this.gameObject)
            {
                c.isTrigger = true;
                RagdollParts.Add(c);
            }
        }
    }

    public void TurnOnRagdoll()
    {
        
        foreach(Collider c in RagdollParts)
        {
            c.isTrigger = false;
            //c.attachedRigidbody.velocity = Vector3.zero;
        }
    }
}
