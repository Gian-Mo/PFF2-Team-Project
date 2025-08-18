using UnityEditor.SearchService;
using UnityEngine;

public class ObjectThatPullYou : MonoBehaviour
{
    [SerializeField] GameObject keyToPress;
    [SerializeField] int speed;

    bool onTriger;
    private void Update()
    {
        if (onTriger)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position); 
        }
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


}
