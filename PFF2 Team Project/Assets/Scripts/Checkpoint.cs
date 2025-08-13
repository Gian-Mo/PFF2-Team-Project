using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    enum healingType { moving, stationary, HOT, homing }
    [SerializeField] healingType type;
    [SerializeField] Rigidbody rb;
    [SerializeField] int healAmount;
    [SerializeField] float healRate;
    [SerializeField] int speed;
    [SerializeField] int destoryTime;
    bool isHealing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == healingType.moving || type == healingType.homing)
        {
            Destroy(gameObject, destoryTime);
            if (type == healingType.moving)
            {
                rb.linearVelocity = transform.forward * speed;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == healingType.homing)
        {
            rb.linearVelocity = (GameManager.instance.player.transform.position - transform.position).normalized * speed * Time.deltaTime;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && type != healingType.HOT) {
            dmg.takeDamage(-healAmount);
        }
        if (type == healingType.moving || type == healingType.homing)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        
        IDamage dmg = other.GetComponent<IDamage>();
        if(dmg != null && type == healingType.HOT)
        {
            if (!isHealing)
            {
                StartCoroutine(damageOther(dmg));

            }
        }
    }

    IEnumerator damageOther(IDamage d)
    {
        isHealing = true;
        d.takeDamage(-healAmount);
        yield return new WaitForSeconds(healRate);
        isHealing = false;
    }
}
