using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemyController : EnemyController
{
    // Start is called before the first frame update
    public PlayerController player;
    public GameObject bulletPrefab;
    public float lookDelayTime = 1f;
    public float lookSpeed = .4f;
    public override void Start()
    {
        player = FindObjectOfType<PlayerController>();
        currentHealth = startingHealth;
        float temp1 = Random.Range(-1f, 1f);
        if (temp1 >= 0f)
        {
            turnRight = 1;
        }
        else
        {
            turnRight = -1;
        }
        float temp2 = Random.Range(-1f, 1f);
        if (temp2 >= 0f)
        {
            moveRight = 1;
        }
        else
        {
            moveRight = -1;
        }

        GetComponent<Renderer>().materials[0].SetColor("_BaseColor", initColor);
        StartCoroutines();

    }

    // Update is called once per frame
    public override void Update()
    {
        if (!LevelManager.bPaused)
        {
            if (Input.GetButtonDown("DebugButton"))
            {
                Shoot();
            }

            if (bActive)
            {
                if (bCanShoot)
                {
                    int random = Random.Range(1, 1000);
                    if (shotFrequency > random)
                    {
                        Shoot();
                    }
                }

            }
        }
    }

    public override void StartCoroutines()
    {

        StartCoroutine(horizontalMoveCoroutine(0f, movementDistance * moveRight, movementTime));
        StartCoroutine(lookDelay());


    }

    IEnumerator lookDelay()
    {
        while (LevelManager.bPaused)
        {
            yield return null;
        }
        yield return new WaitForSeconds(lookDelayTime);
        while (!bActive)
        {
            yield return null;
        }
        if (player == null)
        {
            yield break;
        }
        StartCoroutine(lookTowardsPlayer(lookSpeed));

    }

    IEnumerator lookTowardsPlayer(float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0f)
        {
            while (LevelManager.bPaused)
            {
                yield return null;
            }
            if (player == null)
            {
                yield break;
            }
            //Vector3 goal = player.transform.position;
            //Vector3 direction = player.transform.position - transform.position;
            Vector3 direction = CalculateWhereToShoot();
            Quaternion goalRotation = Quaternion.LookRotation(direction);
            
            i += Time.deltaTime * rate;
            lookCam.rotation = Quaternion.Lerp(lookCam.rotation, goalRotation, i);
            
            yield return null;
        }
        StartCoroutine(lookDelay());

    }


    public Vector3 CalculateWhereToShoot()
    {
        
        float bulletVelocity = shootForce / 50f;
        float timeToReach = (player.transform.position - transform.position).magnitude / bulletVelocity;
        Vector3 myVelocity = GetComponent<Rigidbody>().velocity;
        Vector3 futurePosition = transform.position + myVelocity * timeToReach;
        Vector3 direction = (player.transform.position - futurePosition).normalized;
        return direction;
    }

    public void Shoot()
    {
        bCanShoot = false;
        GameObject bullet = Instantiate(bulletPrefab, lookCam.position + lookCam.forward * 12f, Quaternion.identity);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());


        bulletRB.AddForce(lookCam.forward * shootForce, ForceMode.Force);

        StartCoroutine(shotDelayTimer());
    }
}
