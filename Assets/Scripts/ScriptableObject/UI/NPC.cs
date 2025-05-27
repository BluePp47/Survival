using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "NPC",menuName ="NPC/Dialog",order = 6)]
public class NPC : ScriptableObject
{
    public DialogOptOutDecisionType dialog;

}
