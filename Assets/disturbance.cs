using System.Collections;
using UnityEngine;

public class disturbance : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite availableSprite;
    public Sprite unAvailableSprite;
    public bool isAvailable = true;

    [Header("Disturbance Settings")]
    public float canHearRadius;
    public float timeToFix;
    private NPCBehavioru npcBehavior;
    public GameObject pressEPrompt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        availableSprite = sr.sprite;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isAvailable)
            {
                pressEPrompt.SetActive(true);
                if (Input.GetKey(KeyCode.E))
                {
                    print("pluep");
                    ActivateDisturbance();
                }
            }
            else
            {
                pressEPrompt.SetActive(false);
            }
        }
        else if (other.gameObject.CompareTag("Resident"))
        {
            npcBehavior = other.GetComponent<NPCBehavioru>();
            StartCoroutine("Fix");
        }
    }

    public IEnumerator Fix()
    {
        yield return new WaitForSeconds(timeToFix);
        sr.sprite = availableSprite;
        npcBehavior.currentState = NPCBehavioru.npcState.Wander;
        isAvailable = true;
    }
    
    public void ActivateDisturbance()
    {
        sr.sprite = unAvailableSprite;
        isAvailable = false;

        foreach(GameObject resident in GameObject.FindGameObjectsWithTag("Resident"))
        {
            npcBehavior = resident.GetComponent<NPCBehavioru>();
            if(Vector2.Distance(transform.position, resident.transform.position) < resident.GetComponent<NPCBehavioru>().baseAwarenessDistance)
            {
                npcBehavior.investigationTarget = gameObject.transform;
                npcBehavior.currentState = NPCBehavioru.npcState.Investigate;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
