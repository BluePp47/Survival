using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingSystem : MonoBehaviour
{
    // ��ġ�� ����� ī�޶� 
    public Camera mainCamera;

    private bool IsBuildmode = false;
    private BuildItem currentItem = null; // ���� ���õ� �Ǽ� ������
    private GameObject ghostObject = null; // ���콺 ��ġ�� ǥ�õ� ��Ʈ ������
                                           // ����ִ� �Ǽ� ������ 
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
                // ���콺 ��ġ�� ray�� ��
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                // Ray�� Collider�� ��Ҵ��� Ȯ��
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 buildPosition = hit.point;
                    // ��ġ 
                    PlacePrefab(buildPosition);
                }
            }

        }
        // currentItem�� ����������� 

        else
        {// ������ �ν��� 
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

            // ���� ��Ʈ�� ������ ����
            if (ghostObject == null)
            {
                // BuildItem���� �������� ������ �ν��Ͻ�ȭ
                ghostObject = Instantiate(currentItem.prefab);

                // Ray �浹 ���������� ���̾� ����
                SetLayerRecursively(ghostObject, LayerMask.NameToLayer("Ignore Raycast"));

                // ��� Renderer�� ��Ƽ������ ������ ó��
                foreach (var renderer in ghostObject.GetComponentsInChildren<Renderer>())
                {
                    foreach (var mat in renderer.materials)
                    {
                        Color color = mat.color;
                        color.a = 0.5f; // ������ ����
                        mat.color = color;

                        // ��Ƽ���� ������ ��带 Transparent�� ����
                        mat.SetFloat("_Mode", 2);
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        mat.SetInt("_ZWrite", 0);
                        mat.DisableKeyword("_ALPHATEST_ON");
                        mat.EnableKeyword("_ALPHABLEND_ON");
                        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        mat.renderQueue = 3000;
                        // material �� standard shader ����̾���� ! 
                    }
                }
            }
            ghostObject.transform.position = targetPosition;
        }

    }
    void PlacePrefab(Vector3 position)
    {
        // ������ �ش� ��ġ ����
        Instantiate(currentItem.prefab, position, Quaternion.identity);
    }

    // ���̾ �ٲ㼭 ray cast�� ���� �ʰ��� ��Ʈ�� �浹�� �� ���� �� ���ִ� �� 
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        // ��� �ڽĿ��Ե� ������ ���̾� ����
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

        // ���� ��Ʈ ����
        if (ghostObject != null)
        {
            Destroy(ghostObject);
            ghostObject = null;
        }
    }

}