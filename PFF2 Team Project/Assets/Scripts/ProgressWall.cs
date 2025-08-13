using UnityEngine;


public class ProgressWall : MonoBehaviour
{
    [SerializeField] int progressGoal;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (progressGoal <= GameManager.instance.gameGoalCount)
        {
            Destroy(gameObject);
        }
    }
}
