using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class MainSetup : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioClip noticeSound;
    
    // [SerializeField] public bool startNotice = false; 
    private void Start()
    {
        Time.timeScale = 1.0f;
        
        SaveData data = SaveSystem.LoadGame(); // 데이터 불러오기
        if (data.startNotice == false) // startNotice == false 일때만 실행 (처음 시작시 1번만 실행)
        {
        NoticeUI noticeUI = NoticeUI.Instance;
        SoundManager.Instance.PlaySFX(noticeSound);

        noticeUI.Show("사막에 왔다는게 너구나?", 2f);
        noticeUI.Show("겁도 없이.... 아아, 들렸어?", 2f);
        noticeUI.Show("보물을 찾으러 온거지? \n밤이 되기 전에 찾을 수 있겠어??", 3f);
        noticeUI.Show("사막의 밤은 꽤나 무섭거든. \n내일이면 사막 어딘가에\n네 몸이 굴러다니고 있을지도 몰라.", 4f);
        noticeUI.Show("뭐...밤을 안전하게 보내기 위한\n아주 좋은 방법을 내가 알고 있긴 해", 3f);
        noticeUI.Show("원한다면 날 찾아와. 어디있는지까지 말해줘야 하는건\n아니겠지?", 3f);
        noticeUI.Show("좀있다 보자고. 아니면 이게 마지막 대화가 되던가. \n깔깔", 3f);
        noticeUI.Show("\n< NPC를 찾아가세요 >\n\n당신이 사막에서 살아남을 수 있는 방법을\n알려줄지도 모릅니다.\n\n", 5);        
        
        data.startNotice = true; // 또 나오지 않게 true로 변경.
        SaveSystem.SaveGame(data);
        Debug.Log("데이터가 저장되었습니다");
        }
        else return;
   
    }
}

