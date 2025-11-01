using System.Collections;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCBehavioru : MonoBehaviour
{
    public enum npcState { Wander, Chase, RunAway, Investigate, Scared }
    public npcState currentState = npcState.Wander;
    public NavMeshAgent npcAgent;
   
    public GameObject player;
    public Rigidbody2D rb;
    float awarenessDistance;
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

    [Header("Scare Settings")]
    public Transform runAwayDestination; //FOR PROTOTYPE: where the navmeshagent pathfinds to when running out of the house
    private GameObject raycastedObject;

    //TODO: Make multiple runAwayDestinations and/or allow NPCs to jump out of windows 

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
            Vector2 direction = player.transform.position - transform.position;

            if (Physics2D.Raycast(transform.position, direction, seeingRange).collider.gameObject.CompareTag("Player"))
            {
                if (Vector3.Angle(transform.forward, direction) < viewAngle / 2)
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
        /*
        raycastedObject = Physics2D.Raycast(transform.position, transform.forward, seeingRange).collider.gameObject;
        if (raycastedObject.CompareTag("Player"))
        {
            Vector3 direction = (raycastedObject.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, direction) < viewAngle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
                if (distanceToTarget < seeingRange)
                {
                    canSeeTarget = true;
                }
                else
                {
                    canSeeTarget = false;
                }
            }
        }
        /*
        Collider2D rangeCheck = Physics2D.OverlapCircle(transform.position, awarenessDistance);
        print(rangeCheck);

        if (rangeCheck && rangeCheck.gameObject.CompareTag("Player"))
        {
            target = rangeCheck.transform;
            print(target);
            

            if (Vector3.Angle(transform.forward, direction) < viewAngle / 2)
            {
                

                if (Physics2D.Raycast(transform.position, direction, distanceToTarget, targetMask))
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
        */
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

            case npcState.RunAway:
                RunAway();
                break;
        }

        if(npcAgent.destination != null)
        {
            Vector3 dir = npcAgent.destination - transform.position;
            float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void Wandering()
    {
        Wander();
        //Detect enemies
        LookForEnemy();
    }

    private void Wander()
    {
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

    void RunAway()
    {
        npcAgent.SetDestination(runAwayDestination.position);
    }
}
