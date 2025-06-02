using UnityEngine;

public class CloseButton : MonoBehaviour
{
    [SerializeField] private NpcInterAction npcInteraction;

    public void CloseButtonPressed()
    {
        npcInteraction.CloseDialogManually();
    }
}
