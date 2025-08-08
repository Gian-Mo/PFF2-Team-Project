using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager instance;

    public GameObject player;
    public playerController playerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
