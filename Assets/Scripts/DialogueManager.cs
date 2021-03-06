﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;
    public TextMeshProUGUI namepro;
    public TextMeshProUGUI dialoguePro;
    
    /*private TextMeshPro nameTextPro;
    private TextMeshPro dialogueTextPro;*/

    public GameObject dialogue_ui;

    private Queue<string> sentences;
    public float posision = 0;

    public Animator animator;

    //Display text
    private float letterTime;
    private float timeCounter = 0.0f;

    public bool sentence_finished = false;
    public bool dialog_finished = false;

    //Dialogue variables
    bool drug_finish;
    private float draw_time;

    //Music
    public AudioSource audioSource;
    public AudioClip soundClip;

    //Player
    public Player player_script;

    public void Start()
    {
        sentences = new Queue<string>();
        dialogue_ui.SetActive(false);

        /*nameTextPro = GetComponent<TextMeshPro>() ?? gameObject.AddComponent<TextMeshPro>();
        dialogueTextPro = GetComponent<TextMeshPro>() ?? gameObject.AddComponent<TextMeshPro>();*/
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentence_finished = false;
        dialog_finished = false;
        drug_finish = dialogue.finish_in_drug;

        letterTime = dialogue.display_time;
        draw_time = dialogue.display_time;

        dialogue_ui.SetActive(true);
        nameText.text = dialogue.NPC_name;
        namepro.text = dialogue.NPC_name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayFirstSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {            
            dialog_finished = true;
            return;
        }

        if (sentence_finished == false)
            return;

        string sentence = sentences.Dequeue();
        

        StopAllCoroutines(); 
        StartCoroutine(TypeSentence(sentence));
    }

    public void DisplayFirstSentence()
    {
        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        sentence_finished = false;
        dialogueText.text = "";
        dialogueText.transform.position = new Vector3(dialogueText.transform.position.x, posision, dialogueText.transform.position.z);

        dialoguePro.text = "";
        //dialoguePro.transform.position = new Vector3(dialoguePro.transform.position.x, posision, dialoguePro.transform.position.z);
        int tmp = 0;

        foreach (char letter in sentence.ToCharArray())
        {
            while (timeCounter < draw_time)
            {
                timeCounter += Time.deltaTime;
                yield return null;
            }
            dialogueText.text += letter;
            dialoguePro.text += letter;
            timeCounter = 0.0f;
            if(letter != ' ')
            {
                audioSource.PlayOneShot(soundClip);
            }
            

            //Srry for this
            tmp++;

            if (tmp == sentence.Length)
                sentence_finished = true;


            yield return null;
        }
    }

    public void EndDialogue()
    {
        dialogue_ui.SetActive(false);
        Debug.Log("Ending conversation");

        if(drug_finish)
        {
            player_script.takeDrugFX.Play();
            player_script.drugPicked = true;
            if(player_script.UsingControllerIsActive.activeSelf)
            {
                player_script.enjoyController.SetActive(true);
            }
            else
                player_script.enjoyPC.SetActive(true);
            //player_script.StartDrug();
        }
    }

    public void FasterLetters()
    {
        draw_time = 0.0f;
    }

    public void SlowLetters()
    {
        draw_time = letterTime;
    }
}
