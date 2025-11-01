using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Abilities : MonoBehaviour
{
    public bool isInDark = true;
    public PlayerMovement playerMovement;
    public GameObject activeTrap;
    public List<GameObject> traps = new List<GameObject>();
    int index = 0;
    public GameObject hiding;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        activeTrap = traps[index];
    }

    // Update is called once per frame
    void Update()
    {
        if (isInDark)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Hide();
                hiding.SetActive(true);
            }
            else
            {
                playerMovement.hiding = false;
                hiding.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            StartCoroutine("SetTrap", activeTrap);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (index == 0)
            {
                index = traps.Count;
            }
            else
            {
                index -= 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (index == traps.Count)
            {
                index = 0;
            }
            else
            {
                index += 1;
            }
        }
    }
    
    public IEnumerator SetTrap(GameObject trap)
    {
        yield return new WaitForSeconds(2);
        Instantiate(trap, transform.position, Quaternion.identity);
    }

    public void Hide()
    {
        NPCBehavioru.awarenessBoostPercentage = 0.1f;
        playerMovement.hiding = true;
    }
}
