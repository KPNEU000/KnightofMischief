using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private NPCBehavioru npcBehavior;
    public float speedDebuff;
    public float scareValue;
    public int lureChance = 1;
    public NPCBehavioru.npcType cantLureType;
    public List<GameObject> canLure = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RollForWhoToLure();
    }
    
    public void RollForWhoToLure()
    {
        foreach(GameObject resident in GameObject.FindGameObjectsWithTag("Resident"))
        {
            npcBehavior = resident.GetComponent<NPCBehavioru>();
            if(npcBehavior.thisType != cantLureType)
            {
                if(npcBehavior.baseAwarenessDistance + Random.Range(-0.3f, 0.3f) < lureChance)
                {
                    canLure.Add(resident);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Resident"))
        {
            ApplyEffects(other.gameObject);
        }
    }

    public void DestroyTrap()
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    public void ApplyEffects(GameObject target)
    {
        npcBehavior = target.GetComponent<NPCBehavioru>();
        npcBehavior.StartStun();
        print(npcBehavior.currentScareMeterValue);
        npcBehavior.baseSpeed -= speedDebuff;
        npcBehavior.currentScareMeterValue += scareValue;
        print(npcBehavior.currentScareMeterValue);
        Invoke("DestroyTrap", 2);
    }
}
