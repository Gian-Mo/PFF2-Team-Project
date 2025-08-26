using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] Transform shootPos;

    Color colorOrig;
    float shootRateOrig;

    float shootTimer;
    float slowTimer;
    float slowTime;

    bool PlayerinTrigger;

    Vector3 playerDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        shootRateOrig = shootRate;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        slowTimer += Time.deltaTime;
        if (PlayerinTrigger)
        {
            playerDir = GameManager.instance.player.transform.position - transform.position;

            if (shootTimer >= shootRate)
            {
                shoot();
            }
            faceTarget();
        }
        if (shootRate >= 10)
        {
            FullSlowScreen();
        }
        if (slowTimer >= slowTime && shootRate >= shootRateOrig)
        {
            shootRate = shootRateOrig;
            FullSlowScreen();
        }
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerinTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerinTrigger = false;
        }
    }

    void shoot()
    {
        shootTimer = 0;
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public void takeDamage(int amount)
    {

        if (HP > 0)
        {
            HP -= amount;
            StartCoroutine(flashRed());
        }
        if (HP <= 0)
        {
            GameManager.instance.updateGameGoal(1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
        {
            model.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            model.material.color = colorOrig;
        }

    public void takeSlow(int amount, float slowtime)
    {
        StartCoroutine(flashBlue());
        slowTimer = 0;
        shootRate *= amount;
        slowTime = slowtime;
    }
    IEnumerator flashBlue()
    {
        model.material.color = Color.blue;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
    void FullSlowScreen()
    {
        if (shootRate > shootRateOrig)
        {
            model.material.color = Color.blue;
        }
        else
        {
            model.material.color = colorOrig;
        }
    }
}
