using UnityEngine;

public class lavaSet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float accel;
    

    bool test;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        speed += accel * Time.deltaTime;
    }

    //Moves the lava upward
    void movement()
    {
        rb.linearVelocity = new Vector3(0f, speed, 0f);
    }


    //Checks for a hitbox to stop moving, like right before the top, for example
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("End"))
        {
            speed = 0;
            accel = 0;
            rb.linearVelocity = Vector3.zero;
        }
    }
}
