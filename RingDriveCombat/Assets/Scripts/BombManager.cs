using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour {
    public float BlastRadius = 20f;
    public Material lavaMat;
    public LayerMask lavaLayer;
    public GameObject explosionEffect;
	// Use this for initialization
	void Start () {
        StartCoroutine(deathDelay());
	}

    IEnumerator deathDelay()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision _col)
    {
        if (_col.collider.gameObject.tag != "Player" )
        {
            
            Vector3 conPoint = _col.contacts[0].point;
            Collider[] colliders = Physics.OverlapSphere(conPoint, BlastRadius);
            if (colliders.Length >= 1)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    Collider col = colliders[i];
                    if (col.gameObject.tag == "Ground")
                    {
                        col.gameObject.GetComponent<MeshRenderer>().material = lavaMat;
                        col.gameObject.layer = LayerMask.NameToLayer("Lava");// lavaLayer.value;
                        //Destroy(col.gameObject);
                        break;
                    }
                }
            }
        }
        else
        {
            ArcadeCarController car = _col.gameObject.GetComponentInParent<ArcadeCarController>();
            GetComponent<Collider>().enabled = false;
            car.Explode();
        }
        GameObject temp = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        temp.transform.SetParent(FindObjectOfType<RingManager>().transform);
        Destroy(temp, 1f);
        Destroy(this.gameObject);
        
    }
}
