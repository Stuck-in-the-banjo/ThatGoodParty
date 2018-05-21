﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    public Player player_state;
    public Dialogue[] dialogues;
    public int turn_to_talk;

    private void Start()
    {
        //dialogues = new Dialogue[5];
    }

    public void TriggerDialogue()
    {

        if(turn_to_talk == (int)player_state.player_trips)
            FindObjectOfType<DialogueManager>().StartDialogue(dialogues[0]);
    }
}
