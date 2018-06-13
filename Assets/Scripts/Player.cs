using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public enum PLAYER_STATE
    {
        FIRST_STATE = 0,
        SECOND_STATE,
        THIRD_STATE,
        FOURTH_STATE,
        FIFTH_STATE,
        FINISH_STATE
    }

    public enum PLAYER_CONTEXT
    {
        FREE = 0,
        START_DRUGS,
        ON_DRUGS,
        OFF_DRUGS,
        TALKING,
        DEAD
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
    public GameObject[] stars_roads;

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

    public float distance_to_slow = 8.0f;
    public float slow_factor = 3.0f;

    //NPC talk
    bool able_to_talk = false;
    NPC npc_to_talk = null;
    public DialogueManager dialogue_manager;

    //Drug Timers
    public float first_trip;
    public float second_trip;
    public float third_trip;
    public float fourth_trip;
    public float fifth_trip;

    float dead_timer = 0.0f;

    float trip_timer;
    public PLAYER_STATE player_trips = PLAYER_STATE.FIRST_STATE;
    Dictionary<PLAYER_STATE, float> trips;

    //Audio
    public AudioSource pick_star_audio;
    public AudioSource first_trip_music;
    public AudioSource trip_music;

    public AudioSource rave_music;

    //Animations
    Animator anim;

    //Shaders
    public Material tripShader;
    private float shaderFadeCount = 0.0f;
    [Range(0.0f, 0.02f)]
    public float firstTripShader = 0.0f;
    [Range(0.0f, 0.02f)]
    public float secondTripShader = 0.0f;
    [Range(0.0f, 0.02f)]
    public float thirdTripShader = 0.0f;
    [Range(0.0f, 0.02f)]
    public float fourthTripShader = 0.0f;
    private bool tripOffDrugs = false;

    // Post processing
    public PostProcessingProfile profilePP;
    private ChromaticAberrationModel.Settings chromaticSettings;
    private bool tripOffDrugsChromatic = false;
    private float chromaticFadeCount = 0.0f;
    [Range(0.0f, 1.0f)]
    public float firstTripChromatic = 0.0f;
    [Range(0.0f, 1.0f)]
    public float secondTripChromatic = 0.0f;
    [Range(0.0f, 1.0f)]
    public float thirdTripChromatic = 0.0f;
    [Range(0.0f, 1.0f)]
    public float fourthTripChromatic = 0.0f;

    // Collider
    private bool offDrugsReached = false;
    
    //Dsiplay Logic
    bool flipped = false;

    //Debug
    float lolol = 0.0f;

    // Use this for initialization
    void Start()
    {
        acceleration = max_acceleration;
        deceleration = max_deceleration;
        initial_gravity = gravity;

        trips = new Dictionary<PLAYER_STATE, float>();

        //Fill dicctrionary
        trips[PLAYER_STATE.FIRST_STATE] = first_trip;
        trips[PLAYER_STATE.SECOND_STATE] = second_trip;
        trips[PLAYER_STATE.THIRD_STATE] = third_trip;
        trips[PLAYER_STATE.FOURTH_STATE] = fourth_trip;
        trips[PLAYER_STATE.FIFTH_STATE] = fifth_trip;

        anim = GetComponent<Animator>();

        //Shader and PP
        //Set trip shader to 0
        tripShader.SetFloat("_Magnitude", 0.0f);
        //PostProcessing
        chromaticSettings = profilePP.chromaticAberration.settings;
        chromaticSettings.intensity = 0;
        profilePP.chromaticAberration.settings = chromaticSettings;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        //Shader and PP
        if (tripOffDrugs == false && shaderFadeCount != 0.0f)
        {
            OffTripShader();
            
        }
        if(tripOffDrugsChromatic == false && chromaticFadeCount != 0.0f)
        {
            OffTripChromaticPP();
        }

        if (player_context == PLAYER_CONTEXT.ON_DRUGS)
        {
            //Shader and PP
            SetTripChromaticPP();
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
                transform.Translate(0.0f, (current_impulse * Time.deltaTime) - (initial_gravity * Time.deltaTime) + (Mathf.Pow(Mathf.Abs(Mathf.Sin(tmp)) * 1.5f, 2) * Time.deltaTime), 0.0f);
                tmp += Time.deltaTime; 
            }

            //Trip Timing
            
            if (offDrugsReached == true) //Timer (trip_timer >= trips[player_trips])
            {
                //Shader and PP
                tripOffDrugs = false;
                tripOffDrugsChromatic = false;
                player_context = PLAYER_CONTEXT.OFF_DRUGS;
                rave_music.Play();
                rave_music.volume = 0.0f;
                offDrugsReached = false;
                //trip_timer = 0.0f;
            }
            //else trip_timer += Time.deltaTime;

            Debug.Log(waving);
        }

        if (player_context == PLAYER_CONTEXT.START_DRUGS)
        {
            //Shader and PP
            SetTripShader();
            current_impulse = Mathf.Abs(Mathf.Cos(impulse_variation)) * max_impulse * 0.75f;
            impulse_variation += impulse_increment;

            transform.Translate(0.0f, (current_impulse * Time.deltaTime) - (gravity * Time.deltaTime), 0.0f);

            if (transform.position.y >= 4.0f)
            {
                player_context = PLAYER_CONTEXT.ON_DRUGS;
                rave_music.Stop();
            }

            rave_music.volume -= 1 * Time.deltaTime;
            rave_music.volume = Mathf.Clamp(rave_music.volume, 0.0f, 1.0f);

            if(player_trips == 0)
            {
                first_trip_music.volume += 1 * Time.deltaTime;
                first_trip_music.volume = Mathf.Clamp(first_trip_music.volume, 0.0f, 1.0f);
            }
            else
            {
                trip_music.volume += 1 * Time.deltaTime;
                trip_music.volume = Mathf.Clamp(trip_music.volume, 0.0f, 1.0f);
            }
        }

        if(player_context == PLAYER_CONTEXT.OFF_DRUGS)
        {
            anim.SetBool("falling", true);
            float distance_to_floor = transform.position.y;

            if (distance_to_floor < distance_to_slow)
            {
                anim.SetBool("falling", false);
                distance_to_floor /= distance_to_slow;

                //Music
                if (player_trips != PLAYER_STATE.FIFTH_STATE)
                {


                    rave_music.volume += 1 * Time.deltaTime;
                    rave_music.volume = Mathf.Clamp(rave_music.volume, 0.0f, 1.0f);

                    if (player_trips == 0)
                    {
                        first_trip_music.volume -= 1 * Time.deltaTime;
                        first_trip_music.volume = Mathf.Clamp(first_trip_music.volume, 0.0f, 1.0f);
                    }
                    else
                    {
                        trip_music.volume -= 1 * Time.deltaTime;
                        trip_music.volume = Mathf.Clamp(trip_music.volume, 0.0f, 1.0f);
                    }
                }

                Debug.Log(distance_to_floor);
            }
            else distance_to_floor = 1.0f;

            transform.Translate(0.0f, -(gravity * Time.deltaTime * distance_to_floor), 0.0f);

            gravity = (gravity + (Time.deltaTime * slow_factor));
            gravity = Mathf.Clamp(gravity, 0.0f, max_gravity);

            if (distance_to_floor < 0.005f)
            {
                FinishDrug();
            }
        }

        if(player_context == PLAYER_CONTEXT.DEAD)
        {

            if (dead_timer >= 8.0f)
            {
                SceneManager.LoadScene("TheEnd");
            }
            else
            {
                trip_music.volume -= 0.005f * Time.deltaTime;
                dead_timer += Time.deltaTime;
            }
        }

        //Debug keys
        DebugPlayer();
    }

    //All input goes here
    void HandleInput()
    {
        if (player_context == PLAYER_CONTEXT.START_DRUGS || player_context == PLAYER_CONTEXT.DEAD) 
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

        if(IsInsideMap())
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

            anim.SetBool("Idle", true);
        }
        else
        {
            anim.SetBool("Idle", false);
        }

        //Vertical Axis
        if(player_context == PLAYER_CONTEXT.ON_DRUGS)
        {
            gravity = (-Input.GetAxis("Vertical") * max_gravity);
            gravity = Mathf.Clamp(gravity, initial_gravity, max_gravity);
            Debug.Log(gravity);
        }

        //Display
        FlipSprite(Input.GetAxis("Horizontal"));

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

                if (player_context != PLAYER_CONTEXT.ON_DRUGS)
                    return;

                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Q))
                {
                    if (!impulsed)
                    {
                        impulsed = true;
                        impulse_variation = 0.0f;
                        anim.SetBool("swimming", true);
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
                anim.SetBool("swimming", false);
            }

            Debug.Log("To the sky" + current_impulse);

        }

    }

    bool IsInsideMap()
    {

        if ((transform.position.x + (current_speed * Time.deltaTime)) <= 8.25f && (transform.position.x + (current_speed * Time.deltaTime)) >= -8.25f)
            return true;

        
        return false;
    }

    void DebugPlayer()
    {
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            acceleration = max_acceleration * 0.5f;
            deceleration = max_deceleration * 0.33f;
            player_context = PLAYER_CONTEXT.ON_DRUGS;
        }*/

        if (Input.GetKeyDown(KeyCode.E))
            player_context = PLAYER_CONTEXT.FREE;

        if (Input.GetKeyDown(KeyCode.P))
            StartDrug();

        if (Input.GetKeyDown(KeyCode.O))
            player_context = PLAYER_CONTEXT.OFF_DRUGS;
    }

    public void StartDrug()
    {
        offDrugsReached = false;
        acceleration = max_acceleration * 0.5f;
        deceleration = max_deceleration * 0.33f;
        player_context = PLAYER_CONTEXT.START_DRUGS;
        anim.SetBool("flying", true);

        stars_roads[(int)player_trips].SetActive(true);

        Transform[] childs = stars_roads[(int)player_trips].GetComponentsInChildren<Transform>(true);
        foreach (Transform star in childs)
        {
            star.gameObject.SetActive(true);
        }

     
            
        if (player_trips == 0)
        {
            first_trip_music.Play();
            first_trip_music.volume = 0.0f;
        }
        else
        {
            trip_music.Play();
            trip_music.volume = 0.0f;
        }

    }

    public void FinishDrug()
    {
        acceleration = max_acceleration;
        deceleration = max_deceleration;
        gravity = initial_gravity;
        player_context = PLAYER_CONTEXT.FREE;

        stars_roads[(int)player_trips].SetActive(false);

        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);

        Transform[] childs = stars_roads[(int)player_trips].GetComponentsInChildren<Transform>(true);
        foreach (Transform star in childs)
        {
            star.gameObject.SetActive(false);
        }

        if (player_trips == PLAYER_STATE.FIFTH_STATE)
        {
            anim.SetBool("flying", false);
            anim.SetBool("die", true);
            player_context = PLAYER_CONTEXT.DEAD;
            return;
        }

        player_trips++;
        anim.SetInteger("Player_State", (int)player_trips);
        anim.SetBool("flying", false);

        
        

        if (first_trip_music.isPlaying)
            first_trip_music.Stop();

        if (trip_music.isPlaying)
            trip_music.Stop();

    }

    void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Collectable"))
        {
            //Collectable music
            pick_star_audio.Play();

            //Impulse player when picks up a star
            impulsed = true;
            impulse_variation = 0.0f;
            anim.SetBool("swimming", true);
            

            other.gameObject.SetActive(false);
        }

        if(other.CompareTag("NPC") && (int)player_trips == other.GetComponent<NPC>().turn_to_talk)
        {
            able_to_talk = true;
            npc_to_talk = other.GetComponent<NPC>();

            if(player_context == PLAYER_CONTEXT.FREE)
            {
                Transform[] childs = other.GetComponentsInChildren<Transform>(true);

                foreach(Transform child in childs)
                {
                    if (child.CompareTag("DialogueLoad"))
                        child.gameObject.SetActive(true);
                }
            }

        }
        if (other.CompareTag("endTrip"))
        {
            offDrugsReached = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            able_to_talk = false;
            npc_to_talk = null;

            
            Transform[] childs = other.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in childs)
            {
                if (child.CompareTag("DialogueLoad"))
                    child.gameObject.SetActive(false);
            }
            
        }
    }

    void FlipSprite(float value)
    {
        if(value < 0.0f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            flipped = true;
        }
        if(value > 0.0f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            flipped = false;
        }
       
    }

    void SetTripShader()
    {
        //Shaders
        tripOffDrugs = true;
        if (trips[player_trips] == first_trip)
        {
            if(shaderFadeCount < firstTripShader)
            {
                shaderFadeCount += 0.0001f;
                tripShader.SetFloat("_Magnitude", shaderFadeCount);
            } 
        }
        else if(trips[player_trips] == second_trip)
        {
            if (shaderFadeCount < secondTripShader)
            {
                shaderFadeCount += 0.0001f;
                tripShader.SetFloat("_Magnitude", shaderFadeCount);
            }
        }
        else if (trips[player_trips] == third_trip)
        {
            if (shaderFadeCount < thirdTripShader)
            {
                shaderFadeCount += 0.0001f;
                tripShader.SetFloat("_Magnitude", shaderFadeCount);
            }
        }
        else if (trips[player_trips] == fourth_trip)
        {
            if (shaderFadeCount < fourthTripShader)
            {
                shaderFadeCount += 0.0001f;
                tripShader.SetFloat("_Magnitude", shaderFadeCount);
            }
        }
        
    }
    void SetTripChromaticPP()
    {
        //Post processing
        tripOffDrugsChromatic = true;
        if (trips[player_trips] == first_trip)
        {
            if (chromaticFadeCount < firstTripChromatic)
            {
                chromaticFadeCount += 0.001f;
                chromaticSettings.intensity = chromaticFadeCount;
                profilePP.chromaticAberration.settings = chromaticSettings;
            }
        }
        else if (trips[player_trips] == second_trip)
        {
            if (chromaticFadeCount < secondTripChromatic)
            {
                chromaticFadeCount += 0.001f;
                chromaticSettings.intensity = chromaticFadeCount;
                profilePP.chromaticAberration.settings = chromaticSettings;
            }
        }
        else if (trips[player_trips] == third_trip)
        {
            if (chromaticFadeCount < thirdTripChromatic)
            {
                chromaticFadeCount += 0.001f;
                chromaticSettings.intensity = chromaticFadeCount;
                profilePP.chromaticAberration.settings = chromaticSettings;
            }
        }
        else if (trips[player_trips] == fourth_trip)
        {
            if (chromaticFadeCount < fourthTripChromatic)
            {
                /*chromaticFadeCount += 0.001f;
                chromaticSettings.intensity = chromaticFadeCount;*/
                chromaticSettings.intensity =  Mathf.PingPong(Time.time, 2);
                profilePP.chromaticAberration.settings = chromaticSettings;
            }
        }
    }
    void OffTripShader()
    {
        if (shaderFadeCount > 0.0f)
        {
            shaderFadeCount -= 0.0001f;
            tripShader.SetFloat("_Magnitude", shaderFadeCount);
        }
        else
        {
            tripShader.SetFloat("_Magnitude", 0.0f);
            tripOffDrugs = true;
        }   
    }
    void OffTripChromaticPP()
    {
        if (chromaticFadeCount > 0.0f)
        {
            chromaticFadeCount -= 0.0005f;
            chromaticSettings.intensity = chromaticFadeCount;
            
            profilePP.chromaticAberration.settings = chromaticSettings;
        }
        else
        {
            chromaticSettings.intensity = 0.0f;
            profilePP.chromaticAberration.settings = chromaticSettings;
            tripOffDrugsChromatic = true;
        }
    }
    }