using Unity.VisualScripting;
using UnityEngine;


//TODOS
// - FIgure This out;
public class NewMonoBehaviourScript : MonoBehaviour
{
    public enum typeOfForce { toward, against }
    [SerializeField] typeOfForce type;
    [SerializeField] float timeOfForce;
    [SerializeField] float speed;

    Rigidbody targetRigidBody;
 
    float timer;
    Vector3 direction;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == typeOfForce.toward) { 
         direction = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        

    }
    public void OnTriggerEnter(Collider other)
    {
        DefineDirection(other);
        if (other.GetComponent<playerController>() != null)
        {
            GameManager.instance.playerScript.playerVel = Vector3.forward;
            GameManager.instance.playerScript.gravity = 0;
        }

    }


    public void OnTriggerStay(Collider other)

    {
 
        IForce force = other.GetComponent<IForce>();

      

        if (force != null && type == typeOfForce.toward)
        {

            force.takeForce(direction * Time.deltaTime);
        }

    }

    public void OnTriggerExit(Collider other)
    {
       
       if (other.GetComponent<playerController>() != null)
       {
            GameManager.instance.playerScript.gravity = GameManager.instance.playerScript.gravityOrig;
       }
           
        
    }

    void DefineDirection(Collider other)
    {
        if (type == typeOfForce.toward)
        {
            direction = -transform.forward * speed;
        }
        else
        {
            direction = transform.forward * speed;
        }


    }

}
