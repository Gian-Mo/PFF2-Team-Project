using UnityEngine;

public class victory : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    bool test;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Moves the lava upward
    void movement()
    {
    }


    //Checks for a hitbox to stop moving, like right before the top, for example
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.YouWin();
    }
}
