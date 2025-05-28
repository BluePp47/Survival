using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialogues : MonoBehaviour
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
