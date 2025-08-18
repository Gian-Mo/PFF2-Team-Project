using System.Collections;

using UnityEngine;


//TODOS
// - FIgure This out;
public class ApplyForce : MonoBehaviour
{
    public enum typeOfForce { toward, against }
    [SerializeField] typeOfForce type;
    [SerializeField] float timeOfForce;
    [SerializeField] float speed;

    public bool allowFromStart;

    Rigidbody targetRigidBody;
 
    float timer;
    Vector3 direction;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        DefineDirection(other);       

    }


    public void OnTriggerStay(Collider other)
    {
        
      TriggerStayFunc(other);

    }

    public void TriggerStayFunc(Collider other)
    {
        if (other.isTrigger) return;
        IForce force = other.GetComponent<IForce>();


        force.takeForce(direction);
    }
    

    public void DefineDirection(Collider other)
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

  public void SetType(typeOfForce newType)
    {
        type = newType;
    }

}
