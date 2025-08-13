using UnityEngine;
using System.Collections;
using UnityEditor.UIElements;

public class Damage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    enum damageType { moving, stationary, DOT, homing, slow }
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    [SerializeField] int slowAmount;
    [SerializeField] int damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] float slowtime;
  

    bool isdamaging;

    void Start()
    {
        if (type == damageType.moving || type == damageType.homing || type == damageType.slow)
        {
            Destroy(gameObject, destroyTime);
            if (type == damageType.moving || type == damageType.slow)
            {
                rb.linearVelocity = transform.forward * speed;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == damageType.homing)
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
        if (dmg != null && type == damageType.slow)
        {
            dmg.takeSlow(slowAmount, slowtime);
        }
        else if (dmg != null && type != damageType.DOT)

        if (dmg != null && type != damageType.DOT)
        {
            dmg.takeDamage(damageAmount);
        }
        if (type == damageType.moving || type == damageType.homing || type == damageType.slow)
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

        if (dmg != null && type == damageType.DOT)
        {
            if (!isdamaging)
            {
                StartCoroutine(damageOther(dmg));
            }
        }
    }

    IEnumerator damageOther(IDamage d)
    {
        isdamaging = true;
        d.takeDamage(damageAmount);
        yield return new WaitForSeconds(damageRate);
        isdamaging = false;
    }
}
