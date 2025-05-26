using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingSystem : MonoBehaviour
{
    // 설치에 사용할 카메라 
    public Camera mainCamera;

    // 들고있는 건설 아이템 
    public BuildItem bluePrint;

    // 마우스 고스트(프리뷰) 옵젝
    private GameObject ghostObject;

    private void Update()
    {    
        // bluePrint를 들고있을때만 
        if(bluePrint != null)
        {
            ShowGhostPrefab();
            if (Input.GetMouseButtonDown(0))
            {
                // 마우스 위치로 ray를 쏴
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                // Ray가 Collider에 닿았는지 확인
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 buildPosition = hit.point;
                    PlacePrefab(buildPosition);
                }
            }

        }
        else
        {// 없으면 부스고 
            if(ghostObject != null)
            {
                Destroy(ghostObject);
                ghostObject = null;
            }
        }
    }

    void ShowGhostPrefab()
    {
        
    }
    void PlacePrefab(Vector3 position)
    {
        // 프리팹 해당 위치 생성
        Instantiate(bluePrint.prefab,position, Quaternion.identity);
    }
}