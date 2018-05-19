using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;
    private Queue<string> sentences;

    public Animator animator;

    //Display text
    public float letterTime;
    private float draw_time;
    private float timeCounter = 0.0f;

    public bool sentence_finished = false;
    public bool dialog_finished = false;

    //Music
    public AudioSource audioSource;
    public AudioClip soundClip;

    public void Start()
    {
        sentences = new Queue<string>();

        draw_time = letterTime;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        sentence_finished = false;
        dialog_finished = false;

        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.NPC_name;
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

        int tmp = 0;

        foreach (char letter in sentence.ToCharArray())
        {
            while (timeCounter < draw_time)
            {
                timeCounter += Time.deltaTime;
                yield return null;
            }
            dialogueText.text += letter;
            timeCounter = 0.0f;
            audioSource.PlayOneShot(soundClip);

            //Srry for this
            tmp++;

            if (tmp == sentence.Length)
                sentence_finished = true;


            yield return null;
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        Debug.Log("Ending conversation");
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
