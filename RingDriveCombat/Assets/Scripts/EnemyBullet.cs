using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        StartCoroutine(deathDelay());
    }

    IEnumerator deathDelay()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision _col)
    {
        if (_col.collider.gameObject.tag == "Player")
        {
            ArcadeCarController car = _col.gameObject.GetComponentInParent<ArcadeCarController>();
            GetComponent<Collider>().enabled = false;
            car.Explode();
            
        }
        Destroy(this.gameObject);

    }
}
