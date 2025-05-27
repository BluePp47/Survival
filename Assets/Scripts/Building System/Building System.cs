using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingSystem : MonoBehaviour
{
    // 설치에 사용할 카메라 
    public Camera mainCamera;

    private bool IsBuildmode = false;
    private BuildItem currentItem = null; // 현재 선택된 건설 아이템
    private GameObject ghostObject = null; // 마우스 위치에 표시될 고스트 프리팹
                                           // 들고있는 건설 아이템 
    public BuildItem bluePrint;
    public BuildItem bluePrint1;
    public BuildItem bluePrint2;


    private void Update()
    {
        if (IsBuildmode && currentItem != null)
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
                    // 설치 
                    PlacePrefab(buildPosition);
                }
            }

        }
        // currentItem를 들고있을때만 

        else
        {// 없으면 부스고 
            if (ghostObject != null)
            {
                Destroy(ghostObject);
                ghostObject = null;
            }
        }
    }

    void ShowGhostPrefab()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;

            // 아직 고스트가 없으면 생성
            if (ghostObject == null)
            {
                // BuildItem에서 프리팹을 가져와 인스턴스화
                ghostObject = Instantiate(currentItem.prefab);

                // Ray 충돌 방지용으로 레이어 변경
                SetLayerRecursively(ghostObject, LayerMask.NameToLayer("Ignore Raycast"));

                // 모든 Renderer의 머티리얼을 반투명 처리
                foreach (var renderer in ghostObject.GetComponentsInChildren<Renderer>())
                {
                    foreach (var mat in renderer.materials)
                    {
                        Color color = mat.color;
                        color.a = 0.5f; // 반투명도 설정
                        mat.color = color;

                        // 머티리얼 렌더링 모드를 Transparent로 설정
                        mat.SetFloat("_Mode", 2);
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        mat.SetInt("_ZWrite", 0);
                        mat.DisableKeyword("_ALPHATEST_ON");
                        mat.EnableKeyword("_ALPHABLEND_ON");
                        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        mat.renderQueue = 3000;
                        // material 은 standard shader 기반이어야함 ! 
                    }
                }
            }
            ghostObject.transform.position = targetPosition;
        }

    }
    void PlacePrefab(Vector3 position)
    {
        // 프리팹 해당 위치 생성
        Instantiate(currentItem.prefab, position, Quaternion.identity);
    }

    // 레이어를 바꿔서 ray cast에 맞지 않게해 고스트와 충돌이 안 나게 끔 해주는 것 
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        // 모든 자식에게도 동일한 레이어 설정
        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    public void SetBuildItem(BuildItem item)
    {
        currentItem = item;
        IsBuildmode = true;

        // 이전 고스트 제거
        if (ghostObject != null)
        {
            Destroy(ghostObject);
            ghostObject = null;
        }
    }

}