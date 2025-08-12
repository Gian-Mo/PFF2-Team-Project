using UnityEngine;

public class RotationBall : MonoBehaviour
{
    [SerializeField] int orbitSpeed;

    Vector3 target;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * orbitSpeed * Time.deltaTime);
        
    }
}
