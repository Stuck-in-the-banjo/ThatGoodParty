using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{

    public string NPC_name;

    [TextArea(3, 10)]
    public string[] sentences;

    public bool finish_in_drug;
    public float display_time = 0.03f;

}
