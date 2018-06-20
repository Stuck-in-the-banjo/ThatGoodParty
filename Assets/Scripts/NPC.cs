using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public Player player_state;
    public Dialogue[] dialogues;
    public Dialogue[] secondRound;
    public bool talked = false;
    public int turn_to_talk;

    private void Start()
    {
        //dialogues = new Dialogue[5];
    }

    public void TriggerDialogue()
    {
        if(!talked)
            FindObjectOfType<DialogueManager>().StartDialogue(dialogues[(int)player_state.player_trips]);
        else FindObjectOfType<DialogueManager>().StartDialogue(secondRound[(int)player_state.player_trips]); 

        /* if(turn_to_talk == (int)player_state.player_trips && talked == false)
         {
             FindObjectOfType<DialogueManager>().StartDialogue(dialogues[0]);
             talked = true;
         }
         else
             FindObjectOfType<DialogueManager>().StartDialogue(secondRound[0]);*/

    }
}
