﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public enum PLAYER_STATE
    {
        NO_STATE = 0
    }

    public enum PLAYER_CONTEXT
    {
        FREE = 0,
        ON_DRUGS,
        TALKING
    }

    //Move variables
    public float max_speed;
    public float max_acceleration;
    public float max_deceleration;
    float deceleration;
    float acceleration;
    float current_speed = 0.0f;

    //Gameplay variables
    PLAYER_CONTEXT player_context = PLAYER_CONTEXT.FREE;

    public float max_impulse;
    public float impulse_increment = 0.05f;
    public float max_gravity = 5.0f;
    public float gravity;
    public float initial_gravity;
    float current_impulse = 0.0f;

    bool impulsed = false;
    float impulse_variation = 0.0f;

    public int score_count = 0;

    //NPC
    bool able_to_talk = false;
    NPC npc_to_talk = null;
    public DialogueManager dialogue_manager;

    // Use this for initialization
    void Start()
    {
        acceleration = max_acceleration;
        deceleration = max_deceleration;
        initial_gravity = gravity;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        if (player_context == PLAYER_CONTEXT.ON_DRUGS)
        {

            transform.Translate(0.0f, (current_impulse * Time.deltaTime) - (gravity * Time.deltaTime), 0.0f);

            current_impulse = Mathf.Clamp(current_impulse, 0.0f, max_impulse);
        }

        //Debug keys
        DebugPlayer();
    }

    //All input goes here
    void HandleInput()
    {
        //Axis
        HandleAxis();

        //A button
        HandleA();

        //B button
    }

    void HandleAxis()
    {
        if (player_context == PLAYER_CONTEXT.TALKING)
            return;

        //Horizontal Axis
        current_speed += Input.GetAxis("Horizontal") * acceleration;
        current_speed = Mathf.Clamp(current_speed, -max_speed, max_speed);
        transform.Translate(current_speed * Time.deltaTime, 0.0f, 0.0f);

        if (Input.GetAxis("Horizontal") == 0.0f)
        {
            if (current_speed < 0.0f)
            {
                current_speed += deceleration;
                current_speed = Mathf.Clamp(current_speed, -max_speed, 0.0f);
            }

            if (current_speed > 0.0f)
            {
                current_speed -= deceleration;
                current_speed = Mathf.Clamp(current_speed, 0.0f, max_speed);
            }
        }

        //Vertical Axis
        if(player_context == PLAYER_CONTEXT.ON_DRUGS)
        {
            gravity = (-Input.GetAxis("Vertical") * max_gravity);
            gravity = Mathf.Clamp(gravity, initial_gravity, max_gravity);
            Debug.Log(gravity);
        }
    }

    void HandleA()
    {

        switch (player_context)
        {
            case PLAYER_CONTEXT.FREE:

                if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Q))
                {
                    if (able_to_talk)
                    {
                        player_context = PLAYER_CONTEXT.TALKING;
                        npc_to_talk.TriggerDialogue();

                        Debug.Log("Start talking");
                    }
                }

                break;

            case PLAYER_CONTEXT.ON_DRUGS:

                if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Q))
                {
                    if (!impulsed)
                    {
                        impulsed = true;
                        impulse_variation = 0.0f;
                    }
                }

                break;

            case PLAYER_CONTEXT.TALKING:

                if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Q))
                {
                    //Close or next sentence
                    if (dialogue_manager.dialog_finished)
                    {
                        player_context = PLAYER_CONTEXT.FREE;
                        dialogue_manager.EndDialogue();
                    }
                    else
                    {
                        if (dialogue_manager.sentence_finished)
                            dialogue_manager.DisplayNextSentence();
                    }
                }

                if (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Q))
                {
                    //Pass text faster
                    dialogue_manager.FasterLetters();
                }

                if (Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Q))
                {
                    //Pass text faster
                    dialogue_manager.SlowLetters();
                }

                break;
        }



        if (impulsed == true)
        {
            current_impulse = Mathf.Abs(Mathf.Cos(impulse_variation)) * max_impulse;
            impulse_variation += impulse_increment;

            if (impulse_variation >= (Mathf.PI * 0.5f))
            {
                impulse_variation = 0.0f;
                current_impulse = 0.0f;
                impulsed = false;
            }

        }

    }

    void HandleB()
    {

    }

    void DebugPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            acceleration = max_acceleration * 0.5f;
            deceleration = max_deceleration * 0.33f;
            player_context = PLAYER_CONTEXT.ON_DRUGS;
        }

        if (Input.GetKeyDown(KeyCode.E))
            player_context = PLAYER_CONTEXT.FREE;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Collectable"))
        {
            score_count++;
        }

        if(other.CompareTag("NPC"))
        {
            able_to_talk = true;
            npc_to_talk = other.GetComponent<NPC>();
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            able_to_talk = false;
            npc_to_talk = null;
        }
    }
}