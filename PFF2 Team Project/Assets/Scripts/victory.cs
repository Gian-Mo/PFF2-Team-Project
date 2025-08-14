using UnityEngine;

public class victory : MonoBehaviour
{ 
        
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.YouWin();
    }

}
