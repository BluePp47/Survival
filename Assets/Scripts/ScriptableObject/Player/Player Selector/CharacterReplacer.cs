using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine;
using System.Collections;

public class CharacterReplacer : MonoBehaviour
{
    [SerializeField] private GameObject[] characterModels; // 외형 프리팹들
    [SerializeField] private Transform modelParent;        // Player의 자식 Transform

    void Start()
    {
        StartCoroutine(SpawnCharacterAfterFrame()); // 1프레임 기다린 뒤 캐릭터 생성
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
    }
}
