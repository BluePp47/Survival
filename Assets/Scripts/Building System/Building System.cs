using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingSystem : MonoBehaviour
{
    // ��ġ�� ����� ī�޶� 
    public Camera mainCamera;

    // ����ִ� �Ǽ� ������ 
    public BuildItem bluePrint;

    // ���콺 ��Ʈ(������) ����
    private GameObject ghostObject;

    private void Update()
    {    
        // bluePrint�� ����������� 
        if(bluePrint != null)
        {
            ShowGhostPrefab();
            if (Input.GetMouseButtonDown(0))
            {
                // ���콺 ��ġ�� ray�� ��
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                // Ray�� Collider�� ��Ҵ��� Ȯ��
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 buildPosition = hit.point;
                    PlacePrefab(buildPosition);
                }
            }

        }
        else
        {// ������ �ν��� 
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
        // ������ �ش� ��ġ ����
        Instantiate(bluePrint.prefab,position, Quaternion.identity);
    }
}