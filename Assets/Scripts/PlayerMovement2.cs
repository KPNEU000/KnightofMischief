using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine.InputSystem;
using System.Numerics;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
// [RequireComponent(typeof(AudioSource))]
public class PlayerMovement2 : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField] private float speed = 10f;
    public static bool moving = false;
    // float basespeed;
    // float sprintspeed;
    // public float sprintboost = 5f; //How much speed is increased by sprinting
    // public float stamina = 100;
    UnityEngine.Vector2 input;
    // Vector2 moveDirection;
    Rigidbody2D rb;
    // public float sprintAwarenessBoost = 5;
    // public bool hiding = false;
    
        [Header("Animation")]
        Animator animator;

        // [Header("Audio")]
        // public AudioClip genericWalkSFX;
        // private AudioSource playerAudioSource;

        // public float range = 1;

        // [Header("UI")]
        // public Slider staminaSlider;
        

    void Start()
    {
        // basespeed = speed;
        // sprintspeed = speed + sprintboost;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // playerAudioSource = GetComponent<AudioSource>();
        UpdatePlayerAnim(0);

    }

    void Update()
    {
        rb.linearVelocity = input * speed;
        // if (!hiding)
        // {
        //     staminaSlider.value = stamina;
        //     if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) && stamina > 0)
        //     {
        //         speed = sprintspeed;
        //         NPCBehavioru.awarenessBoostPercentage = sprintAwarenessBoost;
        //         stamina--;
        //     }
        //     else
        //     {
        //         StartCoroutine("RegainStamina");
        //         NPCBehavioru.awarenessBoostPercentage = 0;
        //         speed = basespeed;
        //     }
        // }
    }
    /*
        public IEnumerator RegainStamina()
        {
            yield return new WaitForSeconds(0.2f);
            stamina++;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!hiding && moving)
            {
                float moveHorizontal = Input.GetAxisRaw("Horizontal");
                float moveVertical = Input.GetAxisRaw("Vertical");



                //Play still animation
                if (moveHorizontal == 0
                && moveVertical == 0)
                {
                    UpdatePlayerAnim(0);
                }
                //Play Walk Animation
                else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
                {
                    UpdatePlayerAnim(1);
                }

                input = transform.right * moveHorizontal + transform.up * moveVertical;
                input.Normalize();

                moveDirection = input;

                rb.linearVelocity = new Vector2(moveHorizontal * speed, moveVertical * speed);
                //rb.AddForce(input * speed * Time.deltaTime);
            }
        }

        public void PlayWalkSound()
        {
            if (input != Vector2.zero)
            {
                playerAudioSource.pitch = UnityEngine.Random.Range(-5, 5);
                playerAudioSource.PlayOneShot(genericWalkSFX);
            }
        }

*/
    public void UpdatePlayerAnim(int animState)
    {
        animator.SetInteger("animState", animState);
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<UnityEngine.Vector2>();
        UpdatePlayerAnim(1);
        moving = true;

        if (context.canceled)
        {
            moving = false;
            UpdatePlayerAnim(0);
        }

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
        if (input.x > 0)
            transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 90);       // Right
        else
            transform.rotation = UnityEngine.Quaternion.Euler(0, 0, -90);     // Left
        }
        else
        {
            if (input.y > 0)
                transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 180);      // Up
            else
                transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);     // Down
        }
        

    }
    
}