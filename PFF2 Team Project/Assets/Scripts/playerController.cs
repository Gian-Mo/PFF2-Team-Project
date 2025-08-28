using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODOS
// - Implement the player controller
// - Implement the IDamage portion of it
//

public class playerController : MonoBehaviour, IDamage, IForce, IPickUp
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject wand;
   public WandStats wandInfo;
    public List<GameObject> spellTypes;

    [SerializeField] public int HP;
    [SerializeField] public int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] Transform headPos;
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject projectile;

    public int gravity;

    [SerializeField] int meleeDamageMod;
    [SerializeField] float shootRate;
    [SerializeField] int meleeDist;
    [SerializeField] float fallThreshold;
    public float pullRate;


    float shootTimer;
    public float healTimer;
    public float speedTimer;
    public float speedTimerMax;
    public float healTimerMax;
    public bool isHealing;
    public bool isSpeedBoosting;
    public int gravityOrig;
    public int jumpSpeedOrig;
    public int HPOrig;
    int jumpCount;
    int speedOrig;
    Vector3 moveDirection;
    public Vector3 playerVel;
    Vector3 playerScaleOrig;
    bool isSprinting;
    bool isJumping;
    float slowTimer;
    float slowTime;

    bool hasJumped = false;
    float groundY;
    float previousYPosition;
    float currentYPos;
    float peakHeight;

    void Start()
    {
        HPOrig = HP;
        gravityOrig = gravity;
        jumpSpeedOrig = jumpSpeed;
        playerScaleOrig = transform.localScale;
        isJumping = false;
        speedOrig = speed;
        groundY = transform.position.y;

        SetWand();
    }

    void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            Movement();
        }

        Sprint();

        float currentYPosition = transform.position.y; 
        if (Input.GetButtonDown("Jump") && !hasJumped) // Detect jump
        {
            hasJumped = true; 
            peakHeight = currentYPosition;
            GameManager.instance.updateHeightCounter(currentYPosition);
            Debug.Log("Jumped from: " + currentYPosition);
        }
        if (peakHeight < currentYPosition) // Update peak while rising. Peak calculated to prevent frame by frame subtraction
        {
            peakHeight = currentYPosition;
        }
        if (hasJumped && controller.isGrounded) // Landed
        {
            hasJumped = false; 
            peakHeight = 0f;
        }
        previousYPosition = currentYPosition;
    }

    void Movement()
    {
        shootTimer += Time.deltaTime;
        slowTimer += Time.deltaTime;
        if (isSpeedBoosting)
        {
            speedTimer += Time.deltaTime;
        }
        if (isHealing) { healTimer += Time.deltaTime; }


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
        if (Input.GetButton("Melee") && shootTimer >= shootRate)
        {
            Melee();
            shootTimer = 0;
        }
        if (speed <= 0)
        {
            FullSlowScreen();
        }
        if (slowTimer >= slowTime && speed < speedOrig)
        {
            resetSpeed();
            FullSlowScreen();
        }
        if(healTimer >= healTimerMax)
        {
            HP = HPOrig;
        }
        if(speedTimer >= speedTimerMax)
        {
            speed = speedOrig;
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

    public void SetWand()
    {
            
        shootRate = wandInfo.shootRate;      

        wand.GetComponent<MeshFilter>().sharedMesh = wandInfo.model.GetComponent<MeshFilter>().sharedMesh;
        wand.GetComponent<MeshRenderer>().sharedMaterial = wandInfo.model.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void ShootProjectile()
    {
        Vector3 rotation = new Vector3(30, 0, 0);
        StartCoroutine(ShootAttack(rotation));      
        AlternateSpell();      
       GameObject spell = Instantiate(projectile,shootPos.position, Camera.main.transform.rotation);
        spell.GetComponent<Damage>().damageMultiplier = wandInfo.shootDamageMod;      

    }
    void Melee()
    {
        RaycastHit hit;
        IDamage dmg = null;
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward);


        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            dmg = hit.collider.GetComponent<IDamage>();
        }

        Vector3 move = new Vector3(0,0,0.5f);
        StartCoroutine(MeleeAttack(move));

        if (dmg != null)
        {

            dmg.takeDamage(meleeDamageMod);

        }


    }

    void AlternateSpell()
    {
        //Add spell randomly to a list and shoot them in that order

        // The chances will vary per type of wand

        int index = Random.Range(1, 101);

        if (index < 71)
        {
            projectile = spellTypes[0];
        }
        else if (index < 91)
        {
            projectile = spellTypes[1];
        }
        else if (index < 101)
        {
            projectile = spellTypes[2];
        }

    }
    IEnumerator MeleeAttack(Vector3 move)
    {
        wand.transform.localPosition += move;

        yield return new WaitForSeconds(0.1f) ;

        wand.transform.localPosition -= move;
    }
    IEnumerator ShootAttack(Vector3 rotation)
    {
        
        wand.transform.Rotate(rotation) ;

        yield return new WaitForSeconds(0.1f);

        wand.transform.Rotate(-rotation);
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
        if (ammount > 0)
        {
            GameManager.instance.FlashScreen(Color.red); 
        }
        else if (ammount < 0)
        {
            GameManager.instance.FlashScreen(Color.green);
        }
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


    void FullSlowScreen()
    {
        if (speed <= 0)
        {
            GameManager.instance.playerFlashScreen.SetActive(true);
        }
        else
        {
            GameManager.instance.playerFlashScreen.SetActive(false);
        }
    }

    public void takeForce(Vector3 direction)
    {
        playerVel += direction;

    }

    public void takeSlow(int amount, float slowtime)
    {
        slowTimer = 0;
        slowTime = slowtime;
        GameManager.instance.FlashScreen(Color.cyan);
        speed /= amount;
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

    public void getGunStats(WandStats wand)
    {
        wandInfo = wand;
       
        SetWand();
    }

    
}
