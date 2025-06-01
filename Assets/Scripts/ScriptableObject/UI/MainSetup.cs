using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSetup : MonoBehaviour
{
    // Start is called before the first frame update


    void Start()
    {
        NoticeUI noticeUI = NoticeUI.Instance;

        noticeUI.Show("사막에 왔다는게 너구나?", 1f);
        noticeUI.Show("겁도 없이.... 아아, 들렸어?", 1.5f);
        noticeUI.Show("보물을 찾으러 온거지? 밤이 되기 전에 찾을 수 있겠어??", 2f);
        noticeUI.Show("사막의 밤은 꽤나 무섭거든. 내일이면 사막 어딘가에 네 몸이 굴러다니고 있을지도 몰라.", 2.5f);
        noticeUI.Show("뭐...아주 좋은 방법을 내가 알고 있긴 해", 2f);
        noticeUI.Show("원한다면 날 찾아와. 어디있는지까지 말해줘야 하는건 아니지?", 2.5f);
        noticeUI.Show("좀있다 보자고. 아니면 이게 마지막 대화가 되던가. 깔깔", 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
