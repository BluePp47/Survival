<<<<<<< HEAD
﻿using System.Collections;
=======
using System.Collections;
>>>>>>> 22004982856045d28e6b925938831d48acf7b099
using System.Collections.Generic;
using UnityEngine;

public class BuildUI : MonoBehaviour
{
    public BuildingSystem buildingSystem;

    //private void Start()
    //{
<<<<<<< HEAD
    //    OnClickBuild(); //디버그용
=======
    //    OnClickBuild(); ����׿�
>>>>>>> 22004982856045d28e6b925938831d48acf7b099
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
