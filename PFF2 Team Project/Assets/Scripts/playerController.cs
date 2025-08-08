using UnityEngine;
//TODOS
// - Implement the player controller
// - Implement the IDamage portion of it
//

public class playerController : MonoBehaviour,IDamage,IForce
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;
    

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
   [SerializeField] int jumpSpeed;
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

    bool isSprinting;


    
    void Start()
    {
        HPOrig = HP;
       gravityOrig = gravity;
       jumpSpeedOrig = jumpSpeed;
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
        }
        else
            playerVel.y -= gravity * Time.deltaTime;


        moveDirection = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);

        controller.Move(moveDirection * speed * Time.deltaTime);

        Jump();

        controller.Move(playerVel * Time.deltaTime);

        Sprint();


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

    public void takeDamage(int ammount)
    {
        HP -= ammount;

        //if (HP <= 0)
        //{
        //    GameManager.instance.YouLose();
        //}

      
    }
    
    public void takeForce(Vector3 direction)
    {
     controller.Move(direction);

              
        
    }
}
