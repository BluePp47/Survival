using UnityEngine;
using UnityEngine.UI;

public class BuildItemSlot : MonoBehaviour
{
    public Image iconImage;
    public Button buildButton;

    private BuildItem buildItem;
    private System.Action<BuildItem> onBuildClicked;

    public void Setup(BuildItem item, System.Action<BuildItem> onClick)
    {
        buildItem = item;
        onBuildClicked = onClick;

        iconImage.sprite = item.icon;
        buildButton.onClick.AddListener(() => onBuildClicked?.Invoke(buildItem));
    }
}