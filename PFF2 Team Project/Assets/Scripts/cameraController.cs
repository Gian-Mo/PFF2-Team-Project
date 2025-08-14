using UnityEngine;

public class cameraController : MonoBehaviour
{

    [SerializeField] int sens;
    [SerializeField] float lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    float rotX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;


        //Use invert Y option

        if (invertY)
        {
            rotX += mouseY;
        }
        else { 
            rotX -= mouseY;
        }


        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        //Rotate camera to look up and down 
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);


        //Rotate the player to look left and right 
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
