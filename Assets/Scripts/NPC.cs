using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public Player player_state;
    public Dialogue[] dialogues;

    private void Start()
    {
        dialogues = new Dialogue[5];
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogues[(int)player_state.player_trips]);
    }
}
