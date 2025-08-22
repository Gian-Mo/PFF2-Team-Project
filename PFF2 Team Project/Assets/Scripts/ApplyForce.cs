using System.Collections;

using UnityEngine;


//TODOS
// - FIgure This out;
public class ApplyForce : MonoBehaviour
{
    public enum typeOfForce { toward, against }
    [SerializeField] typeOfForce type;
    [SerializeField] float timeOfForce;
   public float speed;

    public bool allowFromStart;

    Rigidbody targetRigidBody;
 
    float timer;
   protected Vector3 direction;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        

    }
   virtual public void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        DefineDirection(other);       

    }


    virtual public void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;
        TriggerStayFunc(other);

    }

    public void TriggerStayFunc(Collider other)
    {
      
        IForce force = other.GetComponent<IForce>();
        force.takeForce(direction);
    }
    

   virtual public void DefineDirection(Collider other)
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
