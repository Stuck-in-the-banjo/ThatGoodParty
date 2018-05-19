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
    private float timeCounter = 0.0f;

    //Music
    public AudioSource audioSource;
    public AudioClip soundClip;

    public void Start()
    {
        sentences = new Queue<string>();
    }
    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.NPC_name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        //dialogueText.text = sentence;
        StopAllCoroutines(); 
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";  
        
        foreach (char letter in sentence.ToCharArray())
        {
            while (timeCounter < letterTime)
            {
                timeCounter += Time.deltaTime;
                yield return null;
            }
            dialogueText.text += letter;
            timeCounter = 0.0f;
            audioSource.PlayOneShot(soundClip);
            yield return null;
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        Debug.Log("Ending conversation");
    }
}
