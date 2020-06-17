using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float startingHealth;
    protected float currentHealth;
    public float shootForce;
    public float horizontalAngleThreshold = 45f;
    public float verticalAngleThreshold = 30f;
    public float horizontalTurnTime = 0f;
    public float verticalTurnTime = 0f;
    public GameObject explosionEffect;   
    public float movementTime = 1f;
    public float movementDistance = 0f;
    public GameObject bombPrefab;
    public Transform lookCam;
    public Transform muzzlePoint;
    public float shotFrequency;
    protected float verticalValue = 0f;
    protected bool bActive = false;
    bool dead = false;
    protected bool bCanShoot = true;
    public float shotDelay = 0.8f;
    protected int turnRight = 1;
    protected int moveRight = 1;
    int lookUp = 1;
    protected Color initColor = new Color(1f, 1f, 1f);
    Color finalColor = new Color(1f, 0f, 0f);
    Color disabledColor = new Color(0f, 1f, 0f);
    public GameObject audioObjectPrefab;
    public Material[] disabledMaterials;
    public Material[] regularMaterials;


    

	// Use this for initialization
	public virtual void Start () {
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
        regularMaterials[0].SetColor("_BaseColor", initColor);
        StartCoroutines();
    }
	

    public virtual void StartCoroutines()
    {

        StartCoroutine(horizontalTurnCoroutine(lookCam.localEulerAngles.y, horizontalAngleThreshold, horizontalTurnTime));
        StartCoroutine(verticalTurnCoroutine(lookCam.localEulerAngles.x, -verticalAngleThreshold, verticalTurnTime));
        StartCoroutine(horizontalMoveCoroutine(0f, movementDistance * moveRight, movementTime));

    }
	// Update is called once per frame
	public virtual void Update () {
        if (!LevelManager.bPaused)
        {
            if (Input.GetButtonUp("DebugButton"))
            {
                LaunchBomb();
            }
            
            if (bActive)
            {
                if (bCanShoot)
                {
                    int random = Random.Range(1, 1000);
                    if (shotFrequency > random)
                    {
                        LaunchBomb();
                    }
                }

            }
        }
        //Debug.Log(lookCam.localRotation);
        
	}



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyTrigger")
        {
            bActive = true;
            GetComponent<Renderer>().materials = regularMaterials;
            //GetComponent<Renderer>().materials[0].SetColor("_BaseColor", Color.Lerp(finalColor, initColor, currentHealth / startingHealth));

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "EnemyTrigger")
        {
            bActive = false;
            GetComponent<Renderer>().materials = disabledMaterials;
            //GetComponent<Renderer>().materials[0].SetColor("_BaseColor", disabledColor);


        }
    }

    protected IEnumerator horizontalMoveCoroutine(float initialHorizontalValue, float finalHorizontalValue, float time)
    {


        /*
        float counter = 0f;

        while (counter < time)
        {

            counter += Time.deltaTime;

            float val = Mathf.Lerp(initialHorizontalValue, finalHorizontalValue, counter / time);
            transform.position += new Vector3(0f, 0f,val);
            yield return null;
        }
        */
        /*
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / time;
            float offset = Mathf.Lerp(initialHorizontalValue, finalHorizontalValue, t);
            transform.position = new Vector3(transform.position.x, transform.position.y, initialHorizontalValue + offset);
            yield return null;
        }
        */
        float goalXValue = transform.localPosition.y + finalHorizontalValue;
        float startXValue = transform.localPosition.y;
        float i = 0.0f;
        float rate = 1.0f / time;
        bool imTheOne = false;
        EnemyController[] controllers = FindObjectsOfType<EnemyController>();
        if (controllers[0] == this)
        {
            imTheOne = true;
        }
        while (i < 1.0f)
        {
            while (LevelManager.bPaused)
            {
                yield return null;
            }

            i += Time.deltaTime * rate;


            float whereIAmGoing = (goalXValue + startXValue) * i;
            float howFarShouldIGo = whereIAmGoing - transform.localPosition.y;
            float val = Mathf.Lerp(startXValue, goalXValue, i);
            //my z value should be my starting z value plus val
            float ogXValuePlusVal = startXValue + val;
            float howFarToGo = val - transform.localPosition.y;
            
            if (imTheOne)
            {

                //Debug.Log("name: " + this.name + " i: " + i + " goal: " + goalXValue + " init: " + startXValue + " current: " + transform.position.z + " val: " + val + " howFarToGo: " + howFarToGo);
            }
            transform.localPosition += new Vector3(0f, howFarToGo,0f);

            yield return null;
        }
        
        moveRight *= -1;
        StartCoroutine(horizontalMoveCoroutine(0f, moveRight * movementDistance * 2f, movementTime));
        

    }




    protected IEnumerator horizontalTurnCoroutine(float initialHorizontalValue,float finalHorizontalValue,float time)
    {

        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0f)
        {
            while (LevelManager.bPaused)
            {
                yield return null;
            }
            i += Time.deltaTime * rate;
            float val = Mathf.Lerp(initialHorizontalValue, finalHorizontalValue, i);
            //Debug.Log(val);
            lookCam.localEulerAngles = new Vector3(lookCam.localEulerAngles.x, val -90, lookCam.localEulerAngles.z);

            yield return null;
        }
        turnRight *= -1;
        StartCoroutine(horizontalTurnCoroutine(turnRight * -1 * horizontalAngleThreshold, turnRight * horizontalAngleThreshold, horizontalTurnTime));
        
    }

    protected IEnumerator verticalTurnCoroutine(float initialVerticalValue, float finalVerticalValue, float time)
    {
        
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0f)
        {
            while (LevelManager.bPaused)
            {
                yield return null;
            }
            i += Time.deltaTime * rate;
            verticalValue = Mathf.Lerp(initialVerticalValue, finalVerticalValue, i);
            //Debug.Log(val);
            lookCam.localEulerAngles = new Vector3(verticalValue, lookCam.localEulerAngles.y, lookCam.localEulerAngles.z);

            yield return null;
        }
        lookUp *= -1;
        StartCoroutine(verticalTurnCoroutine(lookUp * -1 * verticalAngleThreshold, lookUp * verticalAngleThreshold, verticalTurnTime));
       
    }


    void LaunchBomb()
    {
        bCanShoot = false;
        GameObject bomb = Instantiate(bombPrefab, lookCam.position + lookCam.forward * 12f, Quaternion.identity);
        Rigidbody bombRB = bomb.GetComponent<Rigidbody>();
        Physics.IgnoreCollision(bomb.GetComponent<Collider>(), GetComponent<Collider>());


        bombRB.AddForce(lookCam.forward * shootForce, ForceMode.Force);
        
        StartCoroutine(shotDelayTimer());
    }

    protected IEnumerator shotDelayTimer()
    {
        yield return new WaitForSeconds(shotDelay);
        bCanShoot = true;
    }

    public void TakeDamage(float Damage)
    {
        if (bActive)
        {
            currentHealth -= Damage;
            regularMaterials[0].SetColor("_BaseColor",Color.Lerp(finalColor, initColor, currentHealth / startingHealth));
            //Debug.Log("Took Damage, health: " + currentHealth);
            
            if (currentHealth <= 0f)
            {
                FindObjectOfType<LevelManager>().EnemyKilled();
                bActive = false;
                GameObject temp = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                temp.transform.SetParent(FindObjectOfType<RingManager>().transform);
                FindObjectOfType<AudioManager>().Play("EnemyDeath");


                Destroy(temp, 1.8f);
                Destroy(gameObject);
                
            }
        }
    }
}
