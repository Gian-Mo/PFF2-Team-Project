using UnityEngine;
//TODOS
// - Implement the player controller
// - Implement the IDamage portion of it
//

public class playerController : MonoBehaviour, IDamage, IForce
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;


    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] Transform headPos;
    public int gravity;

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;

    float shootTimer;

    public int gravityOrig;
    public int jumpSpeedOrig;
    int HPOrig;
    int jumpCount;

    Vector3 moveDirection;
    public Vector3 playerVel;
    Vector3 playerScaleOrig;

    bool isSprinting;
    bool isJumping;



    void Start()
    {
        HPOrig = HP;
        gravityOrig = gravity;
        jumpSpeedOrig = jumpSpeed;
        playerScaleOrig = transform.localScale;
        isJumping = false;
    }


    void Update()
    {
        Movement();
    }
    void Movement()
    {
        shootTimer += Time.deltaTime;


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

        Sprint();

        Crouch();

        if (Input.GetButton("Fire1") && shootTimer >= shootRate)
        {
            Shoot();
            shootTimer = 0;
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


    void Shoot()
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

            dmg.takeDamage(shootDamage);

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
            transform.localScale = new Vector3(playerScaleOrig.x,(playerScaleOrig.y * 0.7f), playerScaleOrig.z); 
        }
        if (Input.GetButtonUp("Crouch"))
        {
            transform.localScale = playerScaleOrig;
        }


    }

    public void takeDamage(int ammount)
    {
        HP -= ammount;

        if (HP <= 0)
        {
            GameManager.instance.YouLose();
        }


    }
    
    public void takeForce(Vector3 direction)
    {
                     
        
    }

  public bool IsJumping()
    {
        return isJumping;
    }
}
