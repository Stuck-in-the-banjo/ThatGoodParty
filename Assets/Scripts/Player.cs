using System.Collections;
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
        START_DRUGS,
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

    //Flying gameplay
    public float max_impulse;
    public float impulse_increment = 0.05f;
    public float max_gravity = 5.0f;
    public float gravity;
    public float initial_gravity;
    float current_impulse = 0.0f;

    bool impulsed = false;
    float impulse_variation = 0.0f;
    bool waving = false;
    float tmp = 0.0f;

    //NPC talk
    bool able_to_talk = false;
    NPC npc_to_talk = null;
    public DialogueManager dialogue_manager;

    //Debug
    float lolol = 0.0f;

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
            


            if (transform.position.y <= 4.0f)
            {
                waving = true;               
            }

            if (transform.position.y >= 5.5f)
            {
                waving = false;
            }

            if (waving == false)
            {
                current_impulse = current_impulse - gravity;

                transform.Translate(0.0f, (current_impulse * Time.deltaTime), 0.0f);
                current_impulse = Mathf.Clamp(current_impulse, 0.0f, max_impulse);

                tmp = 0.0f;
            }
            else
            {      
                transform.Translate(0.0f, (current_impulse * Time.deltaTime) - (gravity * Time.deltaTime) + (Mathf.Pow(Mathf.Abs(Mathf.Sin(tmp)) * gravity, 2) * Time.deltaTime), 0.0f);
                tmp += Time.deltaTime; 
            }

            Debug.Log(waving);
        }

        if (player_context == PLAYER_CONTEXT.START_DRUGS)
        {

            current_impulse = Mathf.Abs(Mathf.Cos(impulse_variation)) * max_impulse * 0.75f;
            impulse_variation += impulse_increment;

            transform.Translate(0.0f, (current_impulse * Time.deltaTime) - (gravity * Time.deltaTime), 0.0f);

            if (transform.position.y >= 4.0f)
            {
                player_context = PLAYER_CONTEXT.ON_DRUGS;
                //current_impulse = 0.0f;
            }
        }

        //Debug keys
        DebugPlayer();
    }

    //All input goes here
    void HandleInput()
    {
        if (player_context == PLAYER_CONTEXT.START_DRUGS)
            return;

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

                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Q))
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

                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Q))
                {
                    if (!impulsed)
                    {
                        impulsed = true;
                        impulse_variation = 0.0f;
                    }
                }

                break;

            case PLAYER_CONTEXT.TALKING:

                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Q))
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

                if (Input.GetKey(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Q))
                {
                    //Pass text faster
                    dialogue_manager.FasterLetters();
                }

                if (Input.GetKeyUp(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Q))
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

            Debug.Log("To the sky" + current_impulse);

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

        if (Input.GetKeyDown(KeyCode.P))
            StartDrug();
    }

    void StartDrug()
    {
        acceleration = max_acceleration * 0.5f;
        deceleration = max_deceleration * 0.33f;
        player_context = PLAYER_CONTEXT.START_DRUGS;
    }

    void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Collectable"))
        {
            //Collectable music

            Destroy(other.gameObject);
        }

        if(other.CompareTag("NPC") && player_context == PLAYER_CONTEXT.FREE)
        {
            able_to_talk = true;
            npc_to_talk = other.GetComponent<NPC>();
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC") && player_context == PLAYER_CONTEXT.FREE)
        {
            able_to_talk = false;
            npc_to_talk = null;
        }
    }
}