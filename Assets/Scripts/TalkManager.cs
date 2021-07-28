using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    public GameObject talkPanel;
    public Text talkText;
    GameObject ObjectForTalkId;
    bool talkIsActive;
    public int talkIndex;

    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateTalkData();
    }

    // ---------------------------------------------------------

    /* 화자(Talker) 번호 정리표.
     * 0: 플레이어("플레이어의 이름")
     * 1: "마을 주민"
     * 2: "노인"
     * 3: "주술사"
     * 4: "괴물"
     * 5: "혼령"
     * 6: "되살아난 혼령"
     * 
     * 20: "???"
     * 21: "INFO"
     */

    // GenerateTalkData()에 대본 적용.
    void GenerateTalkData()
    {
        // 게임 시작 직후, 어떤 마을 NPC가 플레이어에게. (100).
        talkData.Add(100, new string[] {
            "마을 호숫가에 괴물이 나타났대!",
            "그런데 지금은 우리 마을 기사들이 다른 곳으로 원정을 가고 없는데...",
            "우리 마을의 유일한 견습 기사인 네가 여기선 제일 강할 테니...",
            "괴물이 마을에 오기 전에 네가 빨리 처치해주지 않겠니?",
            "마을 서쪽으로 가면 괴물이 있을거야... 부탁한다..!"
        });

        // 튜토리얼 클리어 후, 어떤 노인이 플레이어에게. (200).
        talkData.Add(200, new string[] {
            "요즘 들어 마을에 괴물들이 많이 나타나고 있는 이유를 아는가?",
            "과거 우리 마을에 큰 죄를 저질러서 처형된 죄인이 있었는데",
            "사실 그 자가 정말로 죄를 지었는지에 대해서 의견이 분분했다네.",
            "아마 그 처형된 자가 그때의 일로 원한을 품고",
            "마을에 계속 괴물을 소환하는 것이 아닌가 싶네.",
            "이대로 내버려두면 마을에 계속 괴물을 소환할 것이네.",
            "마을 동쪽으로 가면 과거로 갈 수 있는 동굴이 있는데",
            //"동굴이 위험하기는 하지만 자네라면 과거에 도달할 수 있을 것이네.",
            // 위 주석처리 된 대사는 동굴 내부가 제대로 추가된 후에 주석 해제합니다.
            "자네가 과거로 가서 그의 원한을 잠재워주지 않겠나? 꼭 부탁하겠네!"
        });

        // 과거의 괴물(혼령)과 처치 이후. (300).
        talkData.Add(300, new string[] {
            "나는 원래 이 마을의 왕자로서, 왕이 되었어야 했어.",
            "그런데 지금 네가 섬기는 왕이 나를 죄인으로 몰아 죽였고",
            "나는 결국 이렇게 과거에 원혼으로 남게 되었어.",
            "...", // 화자: 플레이어.
            "마을 근처 호숫가에 한 주술사가 살고 있는 오두막이 있는데",
            "그 주술사한테서 부활의 부적을 받을 수 있어.",
            "부적을 받으면 난 사람으로 되살아날 수 있고 더 강력한 스킬도 사용할 수 있게 돼.",
            "...", //화자: 플레이어.
            /*플레이어의 이름 + */ ", 나와 같이 부정한 왕을 처치하고 이 마을의 왕이 되지 않을래? \n " +
            "(1) 혼령의 말을 무시하고 포탈을 타고 ‘현재’로 가서, 일상으로 돌아간다. \n" +
            "(2) 혼령의 부탁을 수락하고, 포탈을 타고 ‘현재’의 왕을 처치하러 간다."
        });

        // (1)을 선택한 경우. ((1)을 선택하여 플레이어가 포탈을 타고 난 후 엔딩 씬에서.) (350 ~ 352).
        talkData.Add(350, new string[] {
            /*플레이어의 이름 + */ "! 우리 마을을 위해 괴물을 처치해주다니 정말 고맙다!",
            "우리 마을을 구해 준 공적으로 너를 우리 마을의 기사로 임명하겠다!"
        });
        talkData.Add(351, new string[] {
            "괴물이 했던 말이 조금 신경 쓰이기는 하지만...",
            "평범한 일상으로 돌아가고 기사가 된 것으로 만족하자."
        });
        talkData.Add(352, new string[] {
            "과거 괴물의 부탁 장면에서 다른 선택을 해보시겠습니까? \n"
            // id 352: (1)과 (2)의 선택 분기점으로 돌아가는 [예, 아니오] 함수 구현 후 수정 예정.
        });

        // (2)를 선택한 경우. 괴물이 플레이어에게. (370).
        talkData.Add(370, new string[] {
            "잘 선택했어!",
            "포탈을 타고 주술사가 살고 있는 오두막으로 가자."
        });

        // (2)를 선택한 경우. 오두막의 주술사가 플레이어에게. (400).
        talkData.Add(400, new string[] {
            "이 부적을 붙이면 다시 살아날 수 있다",
            "하지만 부적이 떨어지지 않게 조심해야 해. 그러면 다시 혼령으로 돌아갈테니."
        });

        // 되살아난 혼령(오두막에서). (500).
        talkData.Add(500, new string[] {
            "자, 이 포탈을 타고 왕이 있는 성으로 가자.",
            "왕은 네가 지금까지 본 적 없는 이상한 스킬을 쓸 수도 있다는 점을 조심해.",
            "가서 부정한 왕을 처치하고 왕의 자리를 되찾는거야!"
        });

        // 현재의 왕을 처치하고 나서 괴물로부터 듣는 사건의 전말. (600).
        talkData.Add(600, new string[] {
            /*플레이어의 이름 + */ ", 드디어 왕을 처치했군!",
            "우리가 해냈어!!!",
        });
        talkData.Add(601, new string[] {
            "라고 할 줄 알았나?",
            "사실 나는 부적과 왕의 마법 지팡이를 얻어서 더 강한 마법을 얻고,",
            "이 마을을 차지하려 했던 것이다.",
            "나는 애초에 왕자도 아니었고, 그저 왕의 자리를 노렸을 뿐이었다.",
            "그래서 반란을 일으키려 했는데 결국 들켰고 처형당했지.",
            "내가 과거에서 괴물을 소환해서 왕을 처치하기에는 힘이 부족해서,",
            "너를 이용해서 왕을 처치하고 내가 더 강한 마법을 갖고 마을을 지배하려 했는데,",
            "일이 너무 잘 풀려버렸네.",
            "자, 이제 너만 처리하면 끝이다. 그동안 수고 많았다!!!"
        });

        // 괴물(Re) 처치 후, 플레이어의 독백. (700).
        talkData.Add(700, new string[] {
            "그런 음모를 꾸미고 있었다니...",
            "내가 섬기던 왕은 내가 죽였고.. 어떻게 해야 할까...",
            "괴물이 죽으면서 떨어뜨린 부적을 왕에게 붙이면 왕을 살려낼 수 있을까?"
        });

        // 부적을 왕에게 붙이고 나서, 왕과 플레이어의 대화. (800).
        talkData.Add(800, new string[] {
            "정신이 드십니까?",
            "...",
            "정말 죄송합니다... 괴물이 그런 음모를 꾸몄을지는 정말 상상도 못했습니다",
            "저를 용서해주십시오...",
            "...",
            "아니네.",
            "저 괴물도 죽고 나도 죽은 상황에서 다른 선택을 했을 수도 있는데,",
            "오히려 이번 일로 자네에 대한 신뢰가 생긴 것 같네.",
            "게다가 마법을 쓰는 나를 상대로도 이기다니 자네의 실력도 몰라보게 출중해졌군",
            "자네, 우리 마을의 공식적인 기사가 되지 않겠나?",
            "...!",
            "정말 영광입니다..!"
            /*
             아직 수정 중입니다... 이 부분 대사에 대한 아이디어가 있으시다면 직접 수정해주셔도 좋습니다.
             */
        });

        // (2)번 선택지의 엔딩. (900).
        // 350, 352 대사 사용.

        // (2)번 선택지를 거치고 나서 (1)번을 선택한 경우. (950).
        // 350, 352 대사 사용.

        // 대사 string 배열에서 특정한 특수문자가 있다면 '예/아니오'함수를 실행시키도록 할 예정.

        // 대사 추가 함수 복붙 시, key(id)도 수정했는지 반드시 확인할 것.

    }

    public string GetTalkSentence(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
        {
            return null;
        }
        else
        {
            return talkData[id][talkIndex];
        }
    }

    void Talk(int id)
    {
        string talkString = GetTalkSentence(id, talkIndex);


        if(talkString == null)
        {
            Time.timeScale = 1;
            talkIndex = 0;
            talkIsActive = false;
            return;
        }


        talkText.text = talkString;

        talkIsActive = true;
        talkIndex += 1;
    }

    public void TriggerTalks(GameObject ObjectTriggeringTalk)
    {
        Time.timeScale = 0;
        ObjectForTalkId = ObjectTriggeringTalk;
        ObjTalkData objTalkData = ObjectForTalkId.GetComponent<ObjTalkData>();
        Talk(objTalkData.talkId);

        talkPanel.SetActive(talkIsActive);
        
    }

}
