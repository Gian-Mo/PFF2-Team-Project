using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;


public class ObjectThatPullYou : MonoBehaviour
{
    [SerializeField] GameObject keyToPress;
    [SerializeField] int speed;
    [SerializeField] Image coolDown;

    bool onTriger;
    private void Update()
    {
        if (onTriger)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position); 
        }
        FillFunctionality();
    }

    public void OnTriggerEnter(Collider other)
   {
        if (other.isTrigger || !other.CompareTag("Player")) return;
        keyToPress.SetActive(true);
        onTriger = true;
        GameManager.instance.playerScript.SetPullVariables(true,transform.position, speed);

   }
   public void OnTriggerExit(Collider other)
   {
        
        if (other.isTrigger || !other.CompareTag("Player")) return;
        keyToPress.SetActive(false);
        onTriger= false;
        GameManager.instance.playerScript.SetPullVariables(false, transform.position, speed);

    }


    void FillFunctionality()
    {
        if (GameManager.instance.playerScript.pullTimer == 0)
        {
            coolDown.fillAmount = 1;
           
        }
        else if(GameManager.instance.playerScript.pullTimer <= GameManager.instance.playerScript.pullRate)
        {
            coolDown.fillAmount -= (float) 1/ GameManager.instance.playerScript.pullRate * Time.deltaTime;
        }
        
    }
}
