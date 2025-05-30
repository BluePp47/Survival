using UnityEngine;
using UnityEngine.UI;  
public class BuildUI : MonoBehaviour
{
    public BuildingSystem buildingSystem;
    public GameObject BuildingUI; // 건설 슬롯 UI 오브젝트
    public GameObject Inventory;

    public Button buildButton0;
    public Button buildButton1;
    public Button buildButton2;
    public Button exitButton;
    private void Start()
    {
        buildButton0.onClick.AddListener(OnClickBuild);
        buildButton1.onClick.AddListener(OnClickBuild1);
        buildButton2.onClick.AddListener(OnClickBuild2);
        exitButton.onClick.AddListener(OnClickExit);
    }

    public void OnClickBuild()
    {
        buildingSystem.SetBuildItem(buildingSystem.bluePrint);
        BuildingUI.SetActive(false);
    }

    public void OnClickBuild1()
    {
        buildingSystem.SetBuildItem(buildingSystem.bluePrint1);
        BuildingUI.SetActive(false);
    }

    public void OnClickBuild2()
    {
        buildingSystem.SetBuildItem(buildingSystem.bluePrint2);
        BuildingUI.SetActive(false);

    }
    public void OnClickExit()
    {
        BuildingUI.SetActive(false);
        Inventory.SetActive(true);
    }

    public void EnterBuildingSystem()
    {
        BuildingUI.SetActive(true);
        Inventory.SetActive(false);
    }
}
