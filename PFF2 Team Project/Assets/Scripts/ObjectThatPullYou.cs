using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;


public class ObjectThatPullYou : ApplyForce
{
    [SerializeField] GameObject keyToPress;
    [SerializeField] Image coolDown;
    [SerializeField] float pullRate;

    float pullTimer;

    bool onTriger;
    bool getPulled;

    private void Start()
    {
       pullTimer = 0;
        getPulled = false;
    }

    private void Update()
    {
        FillFunctionality();
        pullTimer += Time.deltaTime;

        if (onTriger)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
        if (pullTimer >= pullRate)
        {
            if (Input.GetButtonDown("GetPulled"))
            {
                getPulled = true;
            }
        }
    }

    public override void OnTriggerEnter(Collider other)
     {
        if (other.isTrigger || !other.CompareTag("Player")) return;
        keyToPress.SetActive(true);
        onTriger = true;      
       

    }

    public override void OnTriggerStay(Collider other)
    {
        if (other.isTrigger || !other.CompareTag("Player")) return;

        if (getPulled)
        {
          pullTimer = 0;
          DefineDirection();
          TriggerStayFunc(other);
          getPulled = false;
        }
      
    }
    public void OnTriggerExit(Collider other)
   {
        
        if (other.isTrigger || !other.CompareTag("Player")) return;
        keyToPress.SetActive(false);
        onTriger= false;
       

    }

    public void DefineDirection()
    {
       direction = transform.position - GameManager.instance.player.transform.position;
        direction = direction.normalized * speed;
    }
    void FillFunctionality()
    {
        if (pullTimer == 0)
        {
            coolDown.fillAmount = 1;
           
        }
        else if(pullTimer <= pullRate)
        {
            coolDown.fillAmount -= (float) 1/ pullRate * Time.deltaTime;
        }
        
    }
}
