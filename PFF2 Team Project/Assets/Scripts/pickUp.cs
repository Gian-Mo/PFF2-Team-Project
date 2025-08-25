using UnityEngine;

public class pickUp : MonoBehaviour
{
    [SerializeField] WandStats wand;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        IPickUp pickable = other.GetComponent<IPickUp>();

        if (pickable != null)
        {
            pickable.getGunStats(wand);            
            Destroy(gameObject);
        }

    }
}
