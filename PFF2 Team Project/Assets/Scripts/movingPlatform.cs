using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    [SerializeField] int speed;

    [SerializeField] Transform platform;
    [SerializeField] Transform destination;

    bool arrived = false;

    Vector3 startingPos;

    private void Start()
    {
        startingPos = transform.position;
    }

    private void Update()
    {
        if (platform.position == destination.position || platform.position == startingPos)
        {
            arrived = !arrived;
        }
        if (arrived)
        {
            platform.position = Vector3.MoveTowards(platform.position, startingPos, speed * Time.deltaTime);
        }
        else
        {
            platform.position = Vector3.MoveTowards(platform.position, destination.position, speed * Time.deltaTime);

        }

    }
}
