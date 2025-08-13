using System.Collections;
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
        if (other.isTrigger) return;
        IForce force = other.GetComponent<IForce>();
             

        force.takeForce(direction);
     

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
