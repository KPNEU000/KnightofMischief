using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightCheck : MonoBehaviour
{
    NPCBehavioru npcBehavior;
    Abilities abilities;
    public float lightRadius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightRadius = GetComponent<Light2D>().pointLightOuterRadius;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, lightRadius); //A trigger collider might be faster
        foreach(Collider2D col in colliders)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                abilities = col.gameObject.GetComponent<Abilities>();
                Vector2 dir = transform.position - col.transform.position;
                if (Physics2D.Raycast(transform.position, dir).collider.gameObject.CompareTag("Player"))
                {
                    abilities.isInDark = false;
                }
                else
                {
                    abilities.isInDark = true;
                }
            }
        }
    }
}
