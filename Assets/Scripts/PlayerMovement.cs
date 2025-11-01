using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Controls")]
    public float speed = 10f;
    Vector2 input;
    Vector2 moveDirection;
    CharacterController controller;

    [Header("Animation")]
    Animator animator;

    [Header("Audio")]
    public AudioClip genericWalkSFX;
    private AudioSource playerAudioSource;

    public float range = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerAudioSource = GetComponent<AudioSource>();
        UpdatePlayerAnim(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        //Play still animation
        if (moveHorizontal == 0
        && moveVertical == 0
        && controller.isGrounded)
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

        controller.Move(input * speed * Time.deltaTime);
    }

    public void PlayWalkSound()
    {
        if (input != Vector2.zero)
        {
            playerAudioSource.pitch = UnityEngine.Random.Range(-5, 5);
            playerAudioSource.PlayOneShot(genericWalkSFX);
        }
    }


    public void UpdatePlayerAnim(int animState)
    {
        animator.SetInteger("animState", animState);
    }
}
