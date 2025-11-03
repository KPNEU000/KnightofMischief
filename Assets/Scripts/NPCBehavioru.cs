using System.Collections;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCBehavioru : MonoBehaviour
{
    public enum npcState { Wander, Chase, Faint, Investigate }
    public enum npcType { Elderly, Adult, Teen, Child, Pet}
    public npcState currentState = npcState.Wander;
    public npcType thisType = npcType.Adult;
    public NavMeshAgent npcAgent;
   
    public GameObject player;
    public Rigidbody2D rb;
    float awarenessDistance;
    public int scareMeterMax;
    public float currentScareMeterValue;
    public Transform investigationTarget;
    [Header("Type-Specific")]
    public float speed = 1;
    public float baseAwarenessDistance = 5; //The maximum distance an NPC can hear a normal noise
    public static float awarenessBoostPercentage = 1; //When the player is sprinting, the awareness of all NPCs is boosted to simulate the louder walking
    [Header("Wander Settings")]
    public float wanderFrequency = 1;
    public float wanderRange = 3;
    public float hearingRange = 10f;
    public static float hearingRangeBuff = 0;
    public float seeingRange = 20f;
    [Range(1, 360)]
    public float viewAngle = 180;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    [Header("Chase Settings")]
    public bool canSeeTarget;
    public Transform target;
    public float chasetime = 1; //How many seconds they'll keep chasing after losing sight of you 

    [Header("Investigate Settings")]


    [Header("Faint Settings")]
    public Sprite faintSprite;
    
    [Header("Misc")]
    public float baseSpeed;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        npcAgent = GetComponent<NavMeshAgent>();
        npcAgent.updateUpAxis = false;
        npcAgent.updateRotation = false;
        npcAgent.SetDestination(new Vector2(Random.Range(transform.position.x, transform.position.x + 10), Random.Range(transform.position.y, transform.position.y + 10)));
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, player.transform.position);
        Gizmos.DrawSphere(transform.position, awarenessDistance);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.forward * seeingRange));
    }

    public IEnumerator FOV()
    {
        yield return new WaitForSeconds(0.2f);
        FOVCheck();
    }

    public void FOVCheck()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < seeingRange)
        {
            Vector2 direction = player.transform.position - transform.forward;
            print(direction);

            if (Physics2D.Raycast(transform.position, direction, seeingRange).collider.gameObject.CompareTag("Player"))
            {
                print(Vector3.Angle(transform.position, direction));
                if (Vector3.Angle(transform.position, direction) < viewAngle / 2)
                {
                    canSeeTarget = true;
                }
                else
                {
                    canSeeTarget = false;
                }
            }
            else
            {
                canSeeTarget = false;
            }

        }
        else
        {
            canSeeTarget = false;
        }
    }

    void FixedUpdate()
    {
        awarenessDistance = baseAwarenessDistance * awarenessBoostPercentage;
        switch (currentState)
        {
            case npcState.Wander:
                Wandering();
                break;

            case npcState.Chase:
                Chase();
                break;

            case npcState.Investigate:
                Investigate();
                break;

            case npcState.Faint:
                Faint();
                break;
        }

        if(npcAgent.destination != null) //Look at your destination
        {
            Vector3 dir = npcAgent.destination - transform.position;
            float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void Investigate()
    {
        if (thisType != npcType.Adult)
        {
            currentScareMeterValue += 5;
        }

        target = investigationTarget;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (currentState == npcState.Chase && collision.gameObject.CompareTag("Player"))
        {
            //GameManager.GameOver
        }
    }

    void Wandering()
    {
        StartCoroutine("Wander");
        //Detect enemies
        LookForEnemy();
    }

    private IEnumerator Wander()
    {
        yield return new WaitForSeconds(wanderFrequency); //
        if (!npcAgent.pathPending && !npcAgent.hasPath)
        {
            //Set NPC's destination to somewhere in a 10 unit range 
            npcAgent.SetDestination(new Vector2(Random.Range(transform.position.x - wanderRange, transform.position.x + wanderRange), Random.Range(transform.position.y - wanderRange, transform.position.y + wanderRange)));
        }
    }

    public void LookForEnemy()
    {
        //Hearing
        //When the player enters a small sphere around the NPC, start chasing them
        //Someone would hear you if you walked right behind them
        Collider[] collidersArrayHearing = Physics.OverlapSphere(transform.position, awarenessDistance);
        foreach (Collider collider in collidersArrayHearing)
        {
            if (collider.CompareTag("Player"))
            {
                target = player.transform;
                currentState = npcState.Chase;
            }
        }

        //Sight
        StartCoroutine("FOV");
        if (canSeeTarget)
        {
            target = player.transform;
            currentState = npcState.Chase;
        }
    }

    void Chase()
    {
        npcAgent.SetDestination(target.transform.position);
        
        LookForEnemy();
        if (!canSeeTarget)
        {
            Invoke("LooseInterest", chasetime);
        }
    }

    void LooseInterest()
    {
        target = null;
        currentState = npcState.Wander;
    }

    void Faint()
    {
        Instantiate(faintSprite, transform.position, Quaternion.identity);
        //LevelManager.completionScore++;
        Destroy(gameObject);
    }

    public void StartStun()
    {
        StartCoroutine("Stun");
    }
    public IEnumerator Stun() //stops movement for some time
    {
        baseSpeed = npcAgent.speed;
        npcAgent.speed = 0;
        yield return new WaitForSeconds(1);
        npcAgent.speed = baseSpeed;
    }
}
