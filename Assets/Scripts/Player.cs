using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    //Tutorial
    public GameObject tutorialCanvas;
    public GameObject movePC;
    public GameObject interactPC;
    public GameObject interactController;
    private bool moveDone = false;
    private bool interactDone = false;
    public GameObject UsingControllerIsActive;
    private bool alreadyShownJump = false;
    public Text language;

    public GameObject jumpController;
    public GameObject jumpPC;

    public GameObject enjoyController;
    public GameObject enjoyPC;

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
    public AudioSource[] pick_star_fx;
    private int improveRandomPls = 0;
    //private int pick_star_count = 0;
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
    private BloomModel.Settings bloomSettings;
    private VignetteModel.Settings vignetteSettings;
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
    public float bloomIntensity = 0.5f;
    public float vignetteIntensity = 0.65f;
    private float magicNumber = 0.0f;
    private bool booleanBugFixer = false;
    private bool booleanBugFixer2 = false;

    // Collider
    private bool offDrugsReached = false;

    //Get high
    public bool drugPicked = false;

    //Dsiplay Logic
    bool flipped = false;

    //Debug
    float lolol = 0.0f;
    public float floor = 0.0f;
    public AudioSource takeDrugFX;

    bool lolaso = false;

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
        bloomSettings = profilePP.bloom.settings;
        bloomSettings.bloom.softKnee = 0;
        profilePP.bloom.settings = bloomSettings;
        vignetteSettings = profilePP.vignette.settings;
        vignetteSettings.intensity = 0.0f;
        profilePP.vignette.settings = vignetteSettings;
        //Tutorial
        if (UsingControllerIsActive.activeSelf)
        {
            movePC.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        Tutorial();
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
            // --------------------------------------
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

            //Trip end
            if (offDrugsReached == true || trip_timer >= trips[player_trips]) 
            {
                //Shader and PP
                tripOffDrugs = false;
                tripOffDrugsChromatic = false;
                // --------------------------------------

                player_context = PLAYER_CONTEXT.OFF_DRUGS;
                rave_music.Play();
                rave_music.volume = 0.0f;
                offDrugsReached = false;
                trip_timer = 0.0f;
            }
            else trip_timer += Time.deltaTime;

            //Debug.Log(waving);
        }

        if (player_context == PLAYER_CONTEXT.START_DRUGS)
        {
            //Tutorial jump
            if(alreadyShownJump == false)
            {
                if (UsingControllerIsActive.activeSelf)
                {
                    jumpController.SetActive(true);
                }
                else
                    jumpPC.SetActive(true);
                alreadyShownJump = true;
            }

            //Shader and PP
            SetTripShader();
            SetTripChromaticPP();
            /*bloomSettings.bloom.softKnee = 0.3f;
            profilePP.bloom.settings = bloomSettings;*/
            // --------------------------------------

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
            //Tutorial jump
            if (UsingControllerIsActive.activeSelf)
            {
                jumpController.SetActive(false);
            }
            else
                jumpPC.SetActive(false);

            anim.SetBool("falling", true);
            float distance_to_floor = transform.position.y - floor;

            if (distance_to_floor < distance_to_slow)
            {
                anim.SetBool("falling", false);
                distance_to_floor /= distance_to_slow;

                //Music
                if (player_trips != PLAYER_STATE.FOURTH_STATE)
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

                //Debug.Log(distance_to_floor);
            }
            else distance_to_floor = 1.0f;

            transform.Translate(0.0f, (current_impulse * Time.deltaTime) - (gravity * Time.deltaTime * distance_to_floor), 0.0f);

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
                if (language.text == "English")
                {
                    SceneManager.LoadScene("TheEndEN");
                }
                else
                {
                    SceneManager.LoadScene("TheEnd");
                }    
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
        float axis = Input.GetAxis("Horizontal");

        if(UsingControllerIsActive.activeInHierarchy == false)
        {
            axis = Input.GetAxisRaw("Horizontal");
        }

        
        current_speed += axis * acceleration;
        current_speed = Mathf.Clamp(current_speed, -max_speed, max_speed);

        Debug.Log(axis);

        if(IsInsideMap())
            transform.Translate(current_speed * Time.deltaTime, 0.0f, 0.0f);
        
        if (axis == 0.0f)
        {
            if (current_speed < 0.0f)
            {
                if (UsingControllerIsActive.activeInHierarchy)
                    current_speed += deceleration;
                else current_speed += deceleration;

                Debug.Log("Frenando");

                current_speed = Mathf.Clamp(current_speed, -max_speed, 0.0f);
            }

            if (current_speed > 0.0f)
            {
                if (UsingControllerIsActive.activeInHierarchy)
                    current_speed -= deceleration;
                else current_speed -= deceleration;

                Debug.Log("Frenando");

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
            //Debug.Log(gravity);
        }

        //Display
        FlipSprite(axis);

    }

    void HandleA()
    {

        switch (player_context)
        {
            case PLAYER_CONTEXT.FREE:

                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.E))
                {
                    if (able_to_talk)
                    {
                        current_speed = 0.0f;
                        player_context = PLAYER_CONTEXT.TALKING;
                        anim.SetBool("Idle", true);
                        npc_to_talk.TriggerDialogue();
                        npc_to_talk.talked = true;
                        //Debug.Log("Start talking");
                    }
                }
                else if(drugPicked == true && (Input.GetKey(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.Q)))
                {
                    //Disable enjoy tutorial
                    drugPicked = false;
                    enjoyController.SetActive(false);
                    enjoyPC.SetActive(false);

                    //Reset npc
                    NPC[] npcs = FindObjectsOfType<NPC>();
                    foreach(NPC npc in npcs)
                    {
                        npc.talked = false;
                    }

                    //Drug effects
                    pick_star_fx[0].Play();
                    anim.SetBool("Idle", true);
                    StartDrug();
                }

                break;

            case PLAYER_CONTEXT.ON_DRUGS:

                if (player_context != PLAYER_CONTEXT.ON_DRUGS)
                    return;

                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space))
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

                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.E) || Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
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

                if (Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))
                {
                    //Pass text faster
                    dialogue_manager.FasterLetters();
                }

                if (Input.GetKeyUp(KeyCode.JoystickButton0) || Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return))
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

            //Debug.Log("To the sky" + current_impulse);

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

        /*if (Input.GetKeyDown(KeyCode.E))
            player_context = PLAYER_CONTEXT.FREE;*/

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

        chromaticFadeCount = 0.0f;

        if (player_trips != PLAYER_STATE.FOURTH_STATE)
        {
            chromaticSettings.intensity = 0.0f;
            profilePP.chromaticAberration.settings = chromaticSettings;
            bloomSettings.bloom.softKnee = 0.0f;
            profilePP.bloom.settings = bloomSettings;
            tripOffDrugsChromatic = true;
        }
        stars_roads[(int)player_trips].SetActive(false);

        transform.position = new Vector3(transform.position.x, floor, transform.position.z);

        Transform[] childs = stars_roads[(int)player_trips].GetComponentsInChildren<Transform>(true);
        foreach (Transform star in childs)
        {
            star.gameObject.SetActive(false);
        }

        if (player_trips == PLAYER_STATE.FOURTH_STATE)
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
            // PP chromatic aberration
            if(player_context != PLAYER_CONTEXT.OFF_DRUGS)
            {
                if (vignetteSettings.intensity != vignetteIntensity && player_context == PLAYER_CONTEXT.ON_DRUGS)
                {
                    vignetteSettings.intensity = vignetteIntensity;
                    profilePP.vignette.settings = vignetteSettings;
                }
                if (trips[player_trips] == second_trip || trips[player_trips] == first_trip)
                {
                    chromaticFadeCount += 0.3f;
                    chromaticSettings.intensity = chromaticFadeCount;
                    profilePP.chromaticAberration.settings = chromaticSettings;
                }
                magicNumber = 0.05f;
                if (trips[player_trips] == third_trip || trips[player_trips] == fourth_trip)
                {
                    if (chromaticFadeCount < thirdTripChromatic && trips[player_trips] == third_trip)
                    {
                        chromaticFadeCount += 0.5f;
                        chromaticSettings.intensity = chromaticFadeCount;
                        profilePP.chromaticAberration.settings = chromaticSettings;
                    }
                    else if (chromaticFadeCount < fourthTripChromatic && trips[player_trips] == fourth_trip)
                    {
                        chromaticFadeCount += 0.5f;
                        chromaticSettings.intensity = chromaticFadeCount;
                        profilePP.chromaticAberration.settings = chromaticSettings;

                    }
                    magicNumber = 0.5f;
                }
            }
            else
            {
                chromaticFadeCount += 0.05f;
                chromaticSettings.intensity = chromaticFadeCount;
                profilePP.chromaticAberration.settings = chromaticSettings;
            }
            
            //Collectable music
            PickSound().Play();


            //Impulse player when picks up a star
            if(player_context == PLAYER_CONTEXT.ON_DRUGS)
            {
                impulsed = true;
                impulse_variation = 0.0f;
                anim.SetBool("swimming", true);
            }

            other.gameObject.SetActive(false);
        }

        if(other.CompareTag("NPC"))
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

        if (other.CompareTag("cloudCollider"))
        {
            if (player_trips == PLAYER_STATE.FOURTH_STATE)
            {
                anim.SetBool("flying", false);
                anim.SetBool("die", true);
                player_context = PLAYER_CONTEXT.DEAD;
                return;
            }
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
        // Post processing
        tripOffDrugsChromatic = true;
        if (trips[player_trips] == first_trip)
        {
            if (chromaticFadeCount < firstTripChromatic)
            {
                chromaticFadeCount += 0.001f;
                chromaticSettings.intensity = chromaticFadeCount;
                profilePP.chromaticAberration.settings = chromaticSettings;
            }
            else if(chromaticFadeCount > (firstTripChromatic + 0.0005f))
            {
                chromaticFadeCount -= 0.005f;
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
            else if (chromaticFadeCount > (secondTripChromatic + 0.0005f))
            {
                chromaticFadeCount -= 0.005f;
                chromaticSettings.intensity = chromaticFadeCount;
                profilePP.chromaticAberration.settings = chromaticSettings;
            }
        }
        else if (trips[player_trips] == third_trip)
        {
            /*if (chromaticFadeCount < thirdTripChromatic)
            {
                chromaticFadeCount += 0.001f;
                chromaticSettings.intensity = chromaticFadeCount;
                profilePP.chromaticAberration.settings = chromaticSettings;
            }
            else if (chromaticFadeCount > (thirdTripChromatic + 0.0005f))
            {
                chromaticFadeCount -= 0.005f;
                chromaticSettings.intensity = chromaticFadeCount;
                profilePP.chromaticAberration.settings = chromaticSettings;
            }*/
            if (chromaticFadeCount < thirdTripChromatic)
            {
                chromaticFadeCount += 0.005f;
                chromaticSettings.intensity = chromaticFadeCount;
                profilePP.chromaticAberration.settings = chromaticSettings;
            }
            else if(magicNumber > 0)
            {
                magicNumber -= 0.005f;
                chromaticSettings.intensity = Mathf.PingPong(Time.time, 1.3f) + magicNumber;
            }
            else if(magicNumber < 0)
            {
                chromaticSettings.intensity = Mathf.PingPong(Time.time, 1.3f);
            }

            profilePP.chromaticAberration.settings = chromaticSettings;
        }
        else if (trips[player_trips] == fourth_trip)
        {
            if (chromaticFadeCount < fourthTripChromatic)
            {
                chromaticFadeCount += 0.005f;
                chromaticSettings.intensity = chromaticFadeCount;
                profilePP.chromaticAberration.settings = chromaticSettings;
            }
            else if (magicNumber > 0)
            {
                magicNumber -= 0.005f;
                chromaticSettings.intensity = Mathf.PingPong(Time.time, 2.0f)+magicNumber;
            }
            else if (magicNumber < 0)
            {
                chromaticSettings.intensity = Mathf.PingPong(Time.time, 2.0f);
            }
            profilePP.chromaticAberration.settings = chromaticSettings;
        }
        // Bloom
        if (bloomSettings.bloom.softKnee < bloomIntensity)
        {
            bloomSettings.bloom.softKnee += 0.001f;
            profilePP.bloom.settings = bloomSettings;
        }
    }
    void OffTripShader()
    {
        if (shaderFadeCount > 0.0f)
        {
            shaderFadeCount -= 0.00005f;
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
        // Bloom
        if (bloomSettings.bloom.softKnee > 0.0f)
        {
            bloomSettings.bloom.softKnee -= 0.001f;
            profilePP.bloom.settings = bloomSettings;
        }
        //Vignette
        if (vignetteSettings.intensity <= 0.0f)
        {
            vignetteSettings.intensity = 0.0f;
            profilePP.vignette.settings = vignetteSettings;
        }
        else
        {
            vignetteSettings.intensity -= 0.001f;
            if(vignetteSettings.intensity > 0.0001f)
            {
                profilePP.vignette.settings = vignetteSettings;
            }    
        }
        
        // Chromatic Aberration
        magicNumber = 0.0f;

        if (trips[player_trips] == third_trip && booleanBugFixer == false)
        {
            chromaticFadeCount = 1.0f;
            chromaticSettings.intensity = chromaticFadeCount;

            profilePP.chromaticAberration.settings = chromaticSettings;
            booleanBugFixer = true;
        }
        if (trips[player_trips] == fourth_trip && booleanBugFixer2 == false)
        {
            chromaticFadeCount = 1.5f;
            chromaticSettings.intensity = chromaticFadeCount;

            profilePP.chromaticAberration.settings = chromaticSettings;
            booleanBugFixer2 = true;
        }
        if (chromaticFadeCount > 0.0f)
        {
            chromaticFadeCount -= 0.0005f;
            chromaticSettings.intensity = chromaticFadeCount;
            
            profilePP.chromaticAberration.settings = chromaticSettings;
        }
        else
        {
            //Set every PP to 0
            chromaticSettings.intensity = 0.0f;
            profilePP.chromaticAberration.settings = chromaticSettings;
            bloomSettings.bloom.softKnee = 0.0f;
            profilePP.bloom.settings = bloomSettings;
            vignetteSettings.intensity = 0.0f;
            profilePP.vignette.settings = vignetteSettings;
            tripOffDrugsChromatic = true;
        }
    }

    private AudioSource PickSound()
    {
        /*if (pick_star_count >= pick_star_fx.Length-1)
        {
            pick_star_count = 0;
        }
        else
        {
            pick_star_count++;
        }*/
        if (pick_star_fx[0] != null)
        {
            int random = Random.Range(0, pick_star_fx.Length - 1);
            while (random == improveRandomPls)
            {
                random = Random.Range(0, pick_star_fx.Length - 1);
            }
            improveRandomPls = random;
            return pick_star_fx[random];
        }
        else
            return null;
    }

    private void Tutorial()
    {
        if (UsingControllerIsActive.activeInHierarchy && moveDone == false)
        {
            moveDone = true;
            SetXboxUI();
        }
        if (moveDone == false && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetAxis("Horizontal") != 0.0f))
        {
            moveDone = true;
            UpdateTutorial();
        }
        else if(player_context == PLAYER_CONTEXT.TALKING)
        {
            interactDone = true;
            tutorialCanvas.SetActive(false);
        }
    }
    private void UpdateTutorial()
    {
        if (Input.anyKeyDown)
        {
            SetPCUI();
        }
        else if (Input.GetKey(KeyCode.Joystick1Button0) ||
           Input.GetKey(KeyCode.Joystick1Button1) ||
           Input.GetKey(KeyCode.Joystick1Button2) ||
           Input.GetKey(KeyCode.Joystick1Button3) ||
           Input.GetKey(KeyCode.Joystick1Button4) ||
           Input.GetKey(KeyCode.Joystick1Button5) ||
           Input.GetKey(KeyCode.Joystick1Button6) ||
           Input.GetKey(KeyCode.Joystick1Button7) ||
           Input.GetKey(KeyCode.Joystick1Button8) ||
           Input.GetKey(KeyCode.Joystick1Button9) ||
           Input.GetKey(KeyCode.Joystick1Button10) ||
           Input.GetKey(KeyCode.Joystick1Button11) ||
           Input.GetKey(KeyCode.Joystick1Button12) ||
           Input.GetKey(KeyCode.Joystick1Button13) ||
           Input.GetKey(KeyCode.Joystick1Button14) ||
           Input.GetKey(KeyCode.Joystick1Button15) ||
           Input.GetKey(KeyCode.Joystick1Button16) ||
           Input.GetKey(KeyCode.Joystick1Button17) ||
           Input.GetKey(KeyCode.Joystick1Button18) ||
           Input.GetKey(KeyCode.Joystick1Button19) ||
           Input.GetAxis("Vertical") != 0.0f ||
           Input.GetAxis("Horizontal") != 0.0f)
        {
            SetXboxUI();
        }
    }
    private void SetPCUI()
    {
        UsingControllerIsActive.SetActive(false);
        if (moveDone != true)
        {
            if (movePC.activeInHierarchy == false)
            {
                DisableTutorialUI();
                movePC.SetActive(true);
            }      
        }
        else if (interactDone != true)
        {
            if(interactPC.activeInHierarchy == false)
            {
                DisableTutorialUI();
                interactPC.SetActive(true);
            }
        }  
    }
    private void SetXboxUI()
    {
        UsingControllerIsActive.SetActive(true);
        /*if (moveDone != true)
        {
            if (movePC.activeInHierarchy == false)
            {
                DisableTutorialUI();
                movePC.SetActive(true);
            }
        }*/
        if (interactDone != true)
        {
            if(interactController.activeInHierarchy == false)
            {
                DisableTutorialUI();
                interactController.SetActive(true);
            }
        }
    }
    private void DisableTutorialUI()
    {
        movePC.SetActive(false);
        interactPC.SetActive(false);
        interactController.SetActive(false);
    }
}