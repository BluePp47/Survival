using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[CreateAssetMenu(fileName = "NPC",menuName ="NPC/Dialog",order = 6)]
public class NPC : ScriptableObject
{
    public string NPCName;
    public string dialogueText;
    public string npcName;

    public string option1Buton;
    public string option2Buton;

    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;
}

