using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private NPCBehavioru npcBehavior;
    public float speedDebuff;
    public float scareValue;
    public int lureChance = 1; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        Destroy(gameObject);
    }

    public void ApplyEffects(GameObject target)
    {
        npcBehavior = target.GetComponent<NPCBehavioru>();
        npcBehavior.StartCoroutine("Stun()");
        print(npcBehavior.currentScareMeterValue);
        npcBehavior.baseSpeed -= speedDebuff;
        npcBehavior.currentScareMeterValue += scareValue;
        print(npcBehavior.currentScareMeterValue);
        Invoke("DestroyTrap", 2);
    }
}
