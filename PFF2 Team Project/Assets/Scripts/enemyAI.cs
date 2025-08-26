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
        if (slowTimer >= 2.5f && shootRate < shootRateOrig)
        {
            shootRate = shootRateOrig;
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
        shootRate /= amount;
    }
}
