using UnityEngine;
using UnityEngine.UI;

public class BuildItemUI : MonoBehaviour
{
    public Image iconImage;
    public Text itemNameText;
    public Text costText;
    public Button buildButton;

    private BuildItem buildItem;
    private System.Action<BuildItem> onBuildClicked;

    public void Setup(BuildItem item, System.Action<BuildItem> onClick)
    {
        buildItem = item;
        onBuildClicked = onClick;

        iconImage.sprite = item.icon;
        itemNameText.text = item.itemName;
        costText.text = $"Wood: {item.woodCost}, Stone: {item.stoneCost}";
        buildButton.onClick.AddListener(() => onBuildClicked?.Invoke(buildItem));
    }
}