using UnityEngine;
using System.Collections;

//TODOS
// - Implement the player controller
// - Implement the IDamage portion of it
//

public class playerController : MonoBehaviour, IDamage, IForce
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject wand;
    [SerializeField] WandStats wandInfo;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] Transform headPos;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject projectile;
    
    public int gravity;

    [SerializeField] int shootDamageMod;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
   public float pullRate;

    
    float shootTimer;

    public int gravityOrig;
    public int jumpSpeedOrig;
    int HPOrig;
    int jumpCount;
    int speedOrig;

    Vector3 moveDirection;
    public Vector3 playerVel;
    Vector3 playerScaleOrig;

    bool isSprinting;
    bool isJumping;
    float slowTimer;

   


    void Start()
    {
        HPOrig = HP;
        gravityOrig = gravity;
        jumpSpeedOrig = jumpSpeed;
        playerScaleOrig = transform.localScale;
        isJumping = false;        
        speedOrig = speed;

        SetWand();
    }


    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            Movement();
        }

        Sprint();
    }
    void Movement()
    {
        shootTimer += Time.deltaTime;
        slowTimer += Time.deltaTime;
        


        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
            isJumping = false;
        }
        else
            playerVel.y -= gravity * Time.deltaTime;


        moveDirection = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);

        controller.Move(moveDirection * speed * Time.deltaTime);


        Jump();

        WallRunning();

        controller.Move(playerVel * Time.deltaTime);      

        Crouch();

        if (Input.GetButton("Fire1") && shootTimer >= shootRate)
        {
            ShootProjectile();
            shootTimer = 0;
        }
        if (speed <= 0)
        {
            FullSlowScreen();
        }
        if (slowTimer >= 2.5f && speed < speedOrig)
        {
            resetSpeed();
            FullSlowScreen();
        }
               
    }

    void Jump()
    {

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
            isJumping = true;
        }
    }

    void Sprint()
    {

        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }

    }

    void SetWand()
    {
        shootDamageMod = wandInfo.shootDamageMod;       
        shootRate = wandInfo.shootRate;
       projectile = wandInfo.bulletTypes[0];

        wand.GetComponent<MeshFilter>().sharedMesh = wandInfo.model.GetComponent<MeshFilter>().sharedMesh;
        wand.GetComponent<MeshRenderer>().sharedMaterial = wandInfo.model.GetComponent<MeshRenderer>().sharedMaterial;
    }
    void ShootProjectile()
    {
       

       Instantiate(projectile,shootPos.position, Camera.main.transform.rotation);


    }
    void ShootRay()
    {
        RaycastHit hit;
        IDamage dmg = null;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            dmg = hit.collider.GetComponent<IDamage>();
        }



        if (dmg != null)
        {

            dmg.takeDamage(shootDamageMod);

        }

    }
   

  
    void WallRunning()
    {
        RaycastHit left;
        RaycastHit right;


        if (Physics.Raycast(headPos.position, transform.right, out right, 1, ~ignoreLayer))
        {
            if (right.collider.CompareTag("CanWallRun"))
            {
                jumpCount = 0;
                playerVel = Vector3.zero; 
            }
           
        }
        if (Physics.Raycast(headPos.position, -transform.right, out left, 1, ~ignoreLayer))
        {
            if (left.collider.CompareTag("CanWallRun"))
            {
                jumpCount = 0;
                playerVel = Vector3.zero;
            }
        }
    }



    void Crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            controller.height *= 0.5f;
        }
        if (Input.GetButtonUp("Crouch"))
        {
            controller.height /= 0.5f;
        }


    }

    public void takeDamage(int ammount)
    {
        HP -= ammount;
        updatePlayerUI();
        StartCoroutine(flashDamageScreen());
        if (HP <= 0)
        {
            GameManager.instance.YouLose();
        }
        if (HP >= HPOrig)
        {
            HP = HPOrig;
        }
    }
    public void updatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }

    IEnumerator flashDamageScreen()
    {
        GameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerDamageScreen.SetActive(false);
    }
    IEnumerator flashSlowScreen()
    {
        GameManager.instance.playerSlowScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerSlowScreen.SetActive(false);
    }
    void FullSlowScreen()
    {
        if (speed <= 0)
        {
            GameManager.instance.playerSlowScreen.SetActive(true);
        }
        else
        {
            GameManager.instance.playerSlowScreen.SetActive(false);
        }
    }
    
    public void takeForce(Vector3 direction)
    {
        playerVel += direction;        
        
    } 

    public void takeSlow(int amount, float slowtime)
    {
        slowTimer = 0;
        StartCoroutine(flashSlowScreen());
        speed /= amount;
        if (slowTimer >= slowtime)
        {
            speed *= amount;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Win"))
        {
            GameManager.instance.YouWin();
        }
    }
    

    void resetSpeed()
    {
        speed = speedOrig;
    }
}
