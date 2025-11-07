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
    public bool settingTrap;
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

        if (Input.GetKey(KeyCode.E))
        {
            PlayerMovement.speed = 0;
            if (!settingTrap)
            {
                settingTrap = true;
                StartCoroutine("SetTrap", activeTrap);
            }
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
        else
        {
            PlayerMovement.speed = 10f;
            StopCoroutine("SetTrap");
            settingTrap = false;
        }
    }
    
    public IEnumerator SetTrap(GameObject trap)
    {
        //Play Animation #
        yield return new WaitForSeconds(trap.GetComponent<Trap>().setUpTime);
        Instantiate(trap, transform.position, Quaternion.identity);
        //yield return new WaitForSeconds(cooldown)
        settingTrap = false;
    }

    public void Hide()
    {
        NPCBehavioru.awarenessBoostPercentage = 0.1f;
        playerMovement.hiding = true;
    }
}
