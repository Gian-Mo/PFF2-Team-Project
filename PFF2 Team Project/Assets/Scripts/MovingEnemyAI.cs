using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class MovingEnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTimer;

    Color colorOrig;

    float roamTimer;
    float angleToPlayer;
    float stopingDistOrig;

    bool playerInTrigger;

    Vector3 playerDir;
    Vector3 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        GameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stopingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (agent.remainingDistance < 0.01f)
        {
            roamTimer += Time.deltaTime;
        }
        if (playerInTrigger && !canSeePlayer())
        {
            checkRoam();
        }
        else if (!playerInTrigger)
        {
            checkRoam();
        }

    }

    void checkRoam()
    {
        if (roamTimer >= roamPauseTimer && agent.remainingDistance < 0.01f)
        {
            roam();
        }
    }

    void roam()
    {
        roamTimer = 0;

        agent.stoppingDistance = 0;

        Vector3 ranPos = Random.insideUnitSphere * roamDist;
        ranPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);

    }

    bool canSeePlayer()
    {

        playerDir = GameManager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        Debug.DrawRay(transform.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDir, out hit))
        {

            //Hay, I can see you!!
            if (hit.collider.CompareTag("Player") && angleToPlayer <= FOV)
            {

                agent.SetDestination(GameManager.instance.player.transform.position);

                
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTartget();
                }
                agent.stoppingDistance = stopingDistOrig;
                return true;
            }
        }

        agent.stoppingDistance = 0;
        return false;

    }

    void faceTartget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            agent.stoppingDistance = 0;
        }
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
            Destroy(gameObject);
            GameManager.instance.updateGameGoal(-1);
        }
    }

    public void takeSlow(int amount, float slowtime)
    {
        
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
}
