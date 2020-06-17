using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour {

    public int totalAmmo = 100;
    public int shotsPerBurst = 3;
    public float burstDelay = 1.2f;
    public float shotDelay = .1f;
    public float damagePerBullet = 34f;
    public bool bFiring = false;
    public bool bInfiniteAmmo = true;
    public Transform muzzlePoint;
    public PlayerController player;
    public CameraController cam;
    public Material[] myMats;
    public Material[] myPowerupMats;
    public ParticleSystem muzzleFlash;
    public GameObject flashLight;
    public GameObject gunModel;
    public GameObject bulletPrefab;
    public GameObject powerupBulletPrefab;
    public Material groundMat;
    public LayerMask powerupLayers;
    int ammoInMag;
    bool bCanShoot = true;
	// Use this for initialization
	void Start () {
        myMats = gunModel.GetComponent<Renderer>().materials;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void Shoot()
    {
        if (bFiring == false && bCanShoot == true)
        {
            StartCoroutine(Shooty());
        }
        


        
    }

    IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(burstDelay);
        bCanShoot = true;
    }

    IEnumerator Shooty()
    {
        List<GameObject> hitObjects = new List<GameObject>();
        bFiring = true;
        bCanShoot = false;
        int shotsToFire = shotsPerBurst;
        if (totalAmmo < shotsToFire)
        {
            shotsToFire = totalAmmo;
        }
        player.animationScript.SetShootingValue(bFiring);
        for (int i = 0; i < shotsToFire; i++)
        {
            Vector3 shotDestination = Vector3.zero;
            RaycastHit hit;
            MuzzleFlash();
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 2000f))
            {
                shotDestination = hit.point;
                hitObjects.Add(hit.collider.gameObject);
                
            }
            else
            {

                shotDestination = cam.transform.position + cam.transform.forward * 2000f;
                hitObjects.Add(null);
                

            }
            SpawnBullet(muzzlePoint.position, shotDestination, .2f);
            FindObjectOfType<AudioManager>().Play("Gunshot");
            yield return new WaitForSeconds(shotDelay);
        }
        if (!bInfiniteAmmo)
        {
            totalAmmo -= shotsToFire;
        }
        bFiring = false;
        player.animationScript.SetShootingValue(bFiring);
        StartCoroutine(FireDelay());
        player.GunFeedback(hitObjects);
    }

    void SpawnBullet(Vector3 start, Vector3 end, float time)
    {
        GameObject bulletObject = Instantiate(bulletPrefab, start, Quaternion.identity);
        BulletSimulator bulletComp = bulletObject.GetComponent<BulletSimulator>();
        bulletComp.SetValues(start, end, time);
        
    }


    void SpawnPowerupBullet(Vector3 start, Vector3 end, float time)
    {
        GameObject bulletObject = Instantiate(powerupBulletPrefab, start, Quaternion.identity);
        BulletSimulator bulletComp = bulletObject.GetComponent<BulletSimulator>();
        bulletComp.SetValues(start, end, time);
    }
    public void ShootPowerup()
    {
        Vector3 shotDestination = Vector3.zero;
        RaycastHit hit;
        MuzzleFlash();
        FindObjectOfType<AudioManager>().Play("Powerupshot");
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 2000f,powerupLayers))
        {
            shotDestination = hit.point;
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Lava"))
            {
                hit.collider.gameObject.GetComponent<MeshRenderer>().material = groundMat;
                hit.collider.gameObject.layer = 9;
            }

        }
        else
        {

            shotDestination = cam.transform.position + cam.transform.forward * 2000f;


        }
        SpawnPowerupBullet(muzzlePoint.position, shotDestination, .2f);
    }

    void MuzzleFlash()
    {
        //flashLight.SetActive(true);
        muzzleFlash.Play();      
        
        //flashLight.SetActive(false);
    }





    
}
