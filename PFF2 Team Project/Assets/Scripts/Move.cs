using UnityEngine;

public class Move : MonoBehaviour
{
    enum typesOfMove {oneDirection, backAndForth }
    enum whenDoesItStop { time, destination}

    [SerializeField] typesOfMove type;
    [SerializeField] whenDoesItStop stoppingCondition;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float accel;
    [SerializeField] Vector3 direction;
    [SerializeField] float duration;
    [SerializeField] Vector3 destination;


    Vector3 posOrig;
    private void Start()
    {       
        posOrig = transform.position;
        if (type == typesOfMove.backAndForth || stoppingCondition ==  whenDoesItStop.destination)
        {
            SetDestination(destination); 
        }
        direction = direction.normalized;

    }

    void Update()
    {
        DirectionChange();
        movement();
        speed += accel * Time.deltaTime;
    }

    
    void movement()
    {
        rb.linearVelocity = direction * speed;
    }

    void DirectionChange()
    {
        if (type == typesOfMove.backAndForth) {

            if (Vector3.Distance(transform.position,destination) < 0.1) {
                direction = posOrig - destination;
                direction = direction.normalized;
            }
            if (Vector3.Distance(transform.position, posOrig) < 0.1) {
                direction = destination - posOrig;
                direction = direction.normalized;
            }
        }
    }

    //Setters fot eh direction and destination.
    public void SetDestination(Vector3 newDestination)
    {
        destination = newDestination;
        direction = newDestination - posOrig;
        direction = direction.normalized;
    }
    public void SetDestination(GameObject other)
    {
        destination = other.transform.position;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }
    public void SetDirection(GameObject other)
    {
        direction =  other.transform.position - transform.position;
        direction = direction.normalized;
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
