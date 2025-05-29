using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[Serializable]
public class Dialogues 
{
    public string dialogue;

    public List<string> Selections;
    public List<UnityEvent>events;

    public int SelectCount()
    {
        if(Selections == null)
        {
            return 0;
        }
        return Selections.Count;
    }
}
