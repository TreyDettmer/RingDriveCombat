using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.VFX;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Use this for initialization
    public ArcadeCarController car;
    public CameraController cam;
    public Transform tpcTransform;
    public Transform gunTransform;
    public GunManager gun;
    public GUIManager guiManager;
    public RingManager ring;
    public float horizontalLookSpeed = 1f;
    public float verticalLookSpeed = 1f;
    public float verticalLookDownThreshold = 84f;
    public float verticalLookUpThreshold = 270f;
    public float cameraTransitionTime = 1f;
    public float jumpCooldownTime = 15f;
    public int jumpThrusterEffectSpawnRate;
    float m_horizontalInput = 0f;
    float m_verticalInput = 0f;
    public VisualEffect jumpThrusterEffect;
    public VisualEffect mainThrusterEffect;
    public GameObject jumpThrusterLight;
    public GameObject mainThrusterLight;
    [SerializeField]
    float cameraRotationLimit = 85f;
    float cameraRotationX = 0f;
    float currentCameraRotationX = 0f;
    bool bCanJump = true;
    bool bDriving = true;
    List<GameObject> hitGameObjects;
    public List<Powerup> powerups;
    public bool bPowerupSelected = false;
    public Transform gunUpPosition;
    public Transform gunDownPosition;
    public Transform gunBasePosition;
    public AimingAnimation animationScript;
    GameSettings gameSettings;
    public Animator myAnimator;
    public Rigidbody modelHips;
    public PlayerModelController modelController;
    public bool dead = false;
	void Start () {
        gameSettings = GameObject.Find("_app").GetComponent<GameSettings>();
        hitGameObjects = new List<GameObject>();
        //Debug.Log("Playervert: " + verticalLookSpeed + " playerHorz: " + horizontalLookSpeed);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (LevelManager.bPaused)
        {
            
        }
        else
        {
            if (gameSettings.bInputEnabled && !dead)
            {
                GetInput();

                Move();
            }

        }  
        
	}


    public void Move()
    {
        currentCameraRotationX -= m_verticalInput;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        float blend = 1f - (currentCameraRotationX + cameraRotationLimit) / (cameraRotationLimit * 2f);
        if (blend <= .5f)
        {
            gun.transform.position = Vector3.Lerp(gunDownPosition.position, gunBasePosition.position, blend * 2f);
        }
        else
        {
            gun.transform.position = Vector3.Lerp(gunBasePosition.position, gunUpPosition.position, blend - .5f);
        }
        animationScript.blendValue = blend;
        tpcTransform.localEulerAngles = new Vector3(currentCameraRotationX, tpcTransform.localEulerAngles.y, tpcTransform.localEulerAngles.z);
        //tpcTransform.Rotate(-m_verticalInput, 0f, 0f, Space.Self);

        gunTransform.localEulerAngles = new Vector3(gunTransform.localEulerAngles.x, gunTransform.localEulerAngles.y, -currentCameraRotationX);
        //gunTransform.Rotate(0f, 0f, m_verticalInput, Space.Self);
        
        transform.Rotate(0f, m_horizontalInput, 0f, Space.Self);
        
        

    }
    /*
    IEnumerator LerpCamera(Vector3 finalDestination)
    {
        float i = 0.0f;
        float rate = 1.0f / cameraTransitionTime;
        Vector3 initPosition = cam.transform.position;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            cam.transform.position = Vector3.Lerp(initPosition, finalDestination, i);

            yield return null;
        }
        Debug.Log("Finished");
        if (!car.bDriving)
        {
            bDriving = true;
        }
    }
    */
    private void OnCollisionEnter(Collision collision)
    {
        if (!dead)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                Destroy(collision.gameObject);
                car.Explode();
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Destroy(collision.gameObject);
                car.Explode();
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!dead)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Powerup"))
            {

                Powerup powerup = other.gameObject.GetComponent<Powerup>();
                if (powerups.Count > 0)
                {
                    powerups[0].uses += powerup.uses;
                    if (bPowerupSelected)
                    {
                        guiManager.UpdateAmmoCounter(powerups[0].uses);
                        
                    }
                    Destroy(powerup.gameObject);
                }
                else
                {


                    other.enabled = false;
                    other.gameObject.transform.SetParent(transform);
                    other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    powerup.player = this;
                    powerups.Add(powerup);

                }
                guiManager.UpdateGameFeed("Collected powerup");
                guiManager.DisplayPowerupImg(true);

            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Coin"))
            {
                FindObjectOfType<LevelManager>().PickupCoin();
                Destroy(other.gameObject);
            }
        }
    }

    public void Ragdoll(Vector3 explosionForce)
    {
        //ragdoll code here
        
        GetComponent<Rigidbody>().isKinematic = false;
        myAnimator.enabled = false;
        myAnimator.avatar = null;
        Destroy(gun.gameObject);
        modelController.TurnOnRagdoll();
        modelHips.AddForce(explosionForce);


        Destroy(gameObject, 10f);
    }

    public void SwitchWeapons()
    {
        if (bPowerupSelected)
        {
            FindObjectOfType<AudioManager>().Play("SwitchWeapons");
            bPowerupSelected = false;
            gun.gunModel.GetComponent<Renderer>().materials = gun.myMats;
            guiManager.UpdateAmmoCounter(-1);
            guiManager.SwitchWeapons(0);
        }
        else
        {
            if (powerups.Count > 0)
            {
                FindObjectOfType<AudioManager>().Play("SwitchWeapons");
                bPowerupSelected = true;
                gun.gunModel.GetComponent<Renderer>().materials = gun.myPowerupMats;
                guiManager.UpdateAmmoCounter(powerups[0].uses);
                guiManager.SwitchWeapons(1);

            }
        }
    }

    IEnumerator JumpCooldown()
    {
        while (LevelManager.bPaused)
        {
            yield return null;
        }
        yield return new WaitForSeconds(jumpCooldownTime);
        bCanJump = true;
    }

    IEnumerator JumpThrustEffectDelay()
    {
        if (jumpThrusterEffect)
        {
            
            jumpThrusterEffect.SetInt("MainSpawnRate", 10);
            jumpThrusterLight.SetActive(true);
        }
        if (mainThrusterEffect)
        {
            mainThrusterEffect.SetInt("MainSpawnRate", 0);
            mainThrusterLight.SetActive(false);
        }
        yield return new WaitForSeconds(2f);
        if (jumpThrusterEffect)
        {
            
            jumpThrusterEffect.SetInt("MainSpawnRate", 0);
            jumpThrusterLight.SetActive(false);
        }
        if (mainThrusterEffect)
        {
            mainThrusterEffect.SetInt("MainSpawnRate", 10);
            mainThrusterLight.SetActive(true);
        }

    }

    void GetInput()
    {
        m_horizontalInput = horizontalLookSpeed * Input.GetAxis("Mouse X");
        m_verticalInput = verticalLookSpeed * Input.GetAxis("Mouse Y");
        if (Input.GetButtonDown("Jump") && (car.touchingGroundRLW && car.touchingGroundRRW) && (bCanJump))
        {
            car.rb.AddForce(Vector3.up * car.jumpForce, ForceMode.Impulse);
            FindObjectOfType<AudioManager>().Play("ShipJump");
            bCanJump = false;
            StartCoroutine(JumpCooldown());
            StartCoroutine(JumpThrustEffectDelay());
            StartCoroutine(guiManager.JumpCoolDown(jumpCooldownTime));

        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (bPowerupSelected)
            {
                if (powerups.Count > 0)
                {
                    if (powerups[0].uses > 0)
                    {
                        gun.ShootPowerup();
                        powerups[0].uses -= 1;
                    }
                    guiManager.UpdateAmmoCounter(powerups[0].uses);
                    if (powerups[0].uses <= 0)
                    {
                        powerups.RemoveAt(0);
                        SwitchWeapons();
                        guiManager.DisplayPowerupImg(false);
                    }
                    
                }
            }
            else
            {
                gun.Shoot();

            }

        }
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            SwitchWeapons();
        }
        if (Input.GetKeyDown(KeyCode.G) && car.touchingGroundRLW)
        {

            ring.goFast = true;
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            ring.goFast = false;
        }
    }

    public void GunFeedback(List<GameObject> objects)
    {
        hitGameObjects = objects;
        for (int i = 0; i < hitGameObjects.Count;i++)
        {
            if (hitGameObjects[i] != null)
            {
                EnemyController enemy = hitGameObjects[i].GetComponent<EnemyController>();
                if (enemy)
                {
                    enemy.TakeDamage(gun.damagePerBullet);
                }
                else
                {
                    BombManager bomb = hitGameObjects[i].GetComponent<BombManager>();
                    if (bomb)
                    {
                        Destroy(bomb.gameObject);
                    }
                }
            }
        }
        hitGameObjects.Clear();
    }

}


