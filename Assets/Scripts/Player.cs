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
        ON_DRUGS,
        TALKING
    }

    //Move variables
    public float max_speed;
    public float max_acceleration;
    float acceleration;
    float current_speed = 0.0f;

    //Gameplay variables
    PLAYER_CONTEXT player_context = PLAYER_CONTEXT.FREE;

    public float max_impulse;
    public float impulse_increment = 0.05f;
    public float gravity;
    float current_impulse = 0.0f;

    bool impulsed = false;
    float impulse_variation = 0.0f;

    // Use this for initialization
    void Start()
    {
        acceleration = max_acceleration;

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
        Debug();
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
        current_speed += Input.GetAxis("Horizontal") * acceleration;
        current_speed = Mathf.Clamp(current_speed, -max_speed, max_speed);
        transform.Translate(current_speed * Time.deltaTime, 0.0f, 0.0f);

        if (Input.GetAxis("Horizontal") == 0.0f)
        {
            if (current_speed < 0.0f)
            {
                current_speed += acceleration;
                current_speed = Mathf.Clamp(current_speed, -max_speed, 0.0f);
            }

            if (current_speed > 0.0f)
            {
                current_speed -= acceleration;
                current_speed = Mathf.Clamp(current_speed, 0.0f, max_speed);
            }
        }
    }

    void HandleA()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            switch (player_context)
            {
                case PLAYER_CONTEXT.FREE:

                    break;

                case PLAYER_CONTEXT.ON_DRUGS:

                    if (!impulsed)
                    {
                        impulsed = true;
                        impulse_variation = 0.0f;
                    }
                    break;

                case PLAYER_CONTEXT.TALKING:

                    break;
            }
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

    void Debug()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            acceleration = max_acceleration * 0.5f;
            player_context = PLAYER_CONTEXT.ON_DRUGS;
        }

        if (Input.GetKeyDown(KeyCode.Q))
            player_context = PLAYER_CONTEXT.FREE;
    }
}