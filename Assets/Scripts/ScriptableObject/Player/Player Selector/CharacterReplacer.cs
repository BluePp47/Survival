using System.Collections;
using UnityEngine;

public class CharacterReplacer : MonoBehaviour
{
    [SerializeField] private GameObject[] characterModels; // 외형 프리팹들
    [SerializeField] private Transform modelParent;        // Player의 자식 Transform

    private Animator playerAnimator;

    void Start()
    {
        StartCoroutine(SpawnCharacterAfterFrame());
    }

    IEnumerator SpawnCharacterAfterFrame()
    {
        yield return null; // 1 프레임 기다림

        int index = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);

        // 기존 모델 제거
        foreach (Transform child in modelParent)
        {
            Destroy(child.gameObject);
        }

        // 새 모델 생성 및 부모에 붙이기
        GameObject model = Instantiate(characterModels[index], modelParent);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.identity;

        // ✅ Animator 재연결
        playerAnimator = model.GetComponent<Animator>();
        if (playerAnimator == null)
        {
            Debug.LogError("❌ 새 모델에 Animator가 없습니다!");
            yield break;
        }

        // ✅ PlayerController의 animator 필드에 할당
        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.SetAnimator(playerAnimator);
            Debug.Log("✅ PlayerController에 새 Animator 연결 완료");
        }
        else
        {
            Debug.LogWarning("⚠ PlayerController가 현재 오브젝트에 없습니다!");
        }
    }
}
