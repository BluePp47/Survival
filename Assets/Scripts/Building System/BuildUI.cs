using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUI : MonoBehaviour
{
    public BuildingSystem buildingSystem;

    //private void Start()
    //{
    //    OnClickBuild(); ����׿�
    //}
    public void OnClickBuild()
    {
        
        buildingSystem.SetBuildItem(buildingSystem.bluePrint);
        
    }
    public void OnClickBuild1()
    {
        
        buildingSystem.SetBuildItem(buildingSystem.bluePrint1);
    }

    public void OnClickBuild2()
    {
        buildingSystem.SetBuildItem(buildingSystem.bluePrint2);
    }

}
