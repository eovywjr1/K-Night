using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TalkManager : MonoBehaviour
{
    public GameObject player;

    public GameObject talkPanel;
    public Text talkText;
    public GameObject talkerNamePanel;
    public Text talkerNameText;
    GameObject ObjectForTalkId;
    bool talkIsActive;
    public int talkIndex;

    // ESC.
    bool escMenuPanelIsActive;
    public GameObject escMenuPanel;
    public Text nextQuestText;

    // 대사 딕셔너리.
    Dictionary<int, string[]> talkData;

    public int lastTalkID;

    // YesNo 관련 변수.
    public GameObject talkYesNoPanel;
    public GameObject talkYesArrow;
    public GameObject talkNoArrow;
    private bool isYesNoOn;
    public int toggleOneOrTwo;

    private void Awake()
    {
        isYesNoOn = false;
        talkIsActive = false;
        escMenuPanelIsActive = false;
        talkData = new Dictionary<int, string[]>();
        GenerateTalkData();

        // For nextQuest.
        lastTalkID = 0;
    }

    private void Start()
    {
        


    }

    private void Update()
    {
        ToggleArrowGhostSuggestion();

        TextQuestInEscAndTriggerStoryEvent();

        if (Input.GetKeyDown("escape"))
        {
            ActivateEscMenuPanel();
        }
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

    // GenerateTalkData()에 대본 적용되어 있음.
    void GenerateTalkData()
    {
        // 게임 시작 직후, 어떤 마을 NPC가 플레이어에게. (100).
        talkData.Add(100, new string[] {
            "마을 호숫가에 괴물이 나타났대!:마을 주민",
            "그런데 지금은 우리 마을 기사들이 다른 곳으로 원정을 가고 없는데...:마을 주민",
            "우리 마을의 유일한 견습 기사인 네가 여기선 제일 강할 테니...:마을 주민",
            "괴물이 마을에 오기 전에 네가 빨리 처치해주지 않겠니?:마을 주민",
            "마을 서쪽으로 가면 괴물이 있을거야... 부탁한다..!:마을 주민"
        });

        // 튜토리얼 클리어 후, 어떤 노인이 플레이어에게. (200).
        talkData.Add(200, new string[] {
            "요즘 들어 마을에 괴물들이 많이 나타나고 있는 이유를 아는가?:노인",
            "과거 우리 마을에 큰 죄를 저질러서 처형된 죄인이 있었는데:노인",
            "사실 그 자가 정말로 죄를 지었는지에 대해서 의견이 분분했다네.:노인",
            "아마 그 처형된 자가 그때의 일로 원한을 품고:노인",
            "마을에 계속 괴물을 소환하는 것이 아닌가 싶네.:노인",
            "이대로 내버려두면 마을에 계속 괴물을 소환할 것이네.:노인",
            "마을 동쪽으로 가면 과거로 갈 수 있는 동굴이 있는데:노인",
            //"동굴이 위험하기는 하지만 자네라면 과거에 도달할 수 있을 것이네.:노인",
            // 위 주석처리 된 대사는 동굴 내부가 제대로 추가된 후에 주석 해제합니다.
            "자네가 과거로 가서 그의 원한을 잠재워주지 않겠나? 꼭 부탁하겠네!:노인"
        });

        // 과거의 괴물 처치 후 시작되는 대사. (250)
        talkData.Add(250, new string[] {
            "괴물을 드디어 쓰러뜨렸다..!:playerName",
            "괴물에게 왕에 대한 자세한 이야기를 들어보자..:playerName"
        });

        // 과거의 괴물(혼령)과 처치 이후. (300).
        talkData.Add(300, new string[] {
            "나는 원래 이 마을의 왕자로서, 왕이 되었어야 했어.:혼령",
            "그런데 지금 네가 섬기는 왕이 나를 죄인으로 몰아 죽였고:혼령",
            "나는 결국 이렇게 과거에 원혼으로 남게 되었어.:혼령",
            "...:playerName", // 화자: 플레이어.
            "마을 근처 호숫가에 한 주술사가 살고 있는 숲이 있는데:혼령", // 적절한 오두막 에셋을 찾으면 '숲'을 '오두막'으로 바꾸겠습니다.
            "그 주술사한테서 부활의 부적을 받을 수 있어.:혼령",
            "부적을 받으면 난 사람으로 되살아날 수 있고 더 강력한 스킬도 사용할 수 있게 돼.:혼령",
            "...:playerName", //화자: 플레이어.
            /*플레이어의 이름 + */ "나와 같이 부정한 왕을 처치하고 이 마을의 왕이 되지 않을래? \n" +
            "[1] 혼령의 말을 무시하고 포탈을 타고 ‘현재’로 가서, 일상으로 돌아간다. \n" +
            "[2] 혼령의 부탁을 수락하고, 포탈을 타고 ‘현재’의 왕을 처치하러 간다.:혼령:YesNo|1|0"
        });

        // (1)을 선택한 경우 괴물이 플레이어에게. (310)
        talkData.Add(310, new string[] {
            "...:혼령"

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
            "잘 선택했어!:혼령",
            "포탈을 타고 주술사가 살고 있는 오두막으로 가자.:혼령"
        });

        // (2)를 선택한 경우. 오두막의 주술사가 플레이어에게. (400).
        talkData.Add(400, new string[] {
            "이 부적을 붙이면 다시 살아날 수 있다.:주술사",
            "하지만 부적이 떨어지지 않게 조심해야 해. 그러면 다시 혼령으로 돌아갈테니.:주술사"
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
        string talkString;
        string talkerName;
        string clueToYesNo;

        if (GetTalkSentence(id, talkIndex) != null)
        {
            talkString = GetTalkSentence(id, talkIndex).Split(':')[0];
            if (GetTalkSentence(id, talkIndex).Split(':')[1] != null)
            {
                talkerName = GetTalkSentence(id, talkIndex).Split(':')[1];
            }
            else
            {
                talkerName = " ";
            }

            if (GetTalkSentence(id, talkIndex).Split(':').Length >= 3)
            {
                if (GetTalkSentence(id, talkIndex).Split(':')[2] != null)
                {
                    OnTogglePanelGhostSuggestion();
                }
            }

        }
        else
        {
            talkString = null;
            talkerName = null;
            clueToYesNo = null;
        }
        


        if(talkString == null)
        {
            Time.timeScale = 1;
            talkIndex = 0;
            talkIsActive = false;
            return;
        }


        talkText.text = talkString;
        talkerNameText.text = talkerName;

        talkIsActive = true;
        talkIndex += 1;
    }

    public void TriggerTalks(GameObject ObjectTriggeringTalk)
    {
        Time.timeScale = 0;
        ObjectForTalkId = ObjectTriggeringTalk;
        ObjTalkData objTalkData = ObjectForTalkId.GetComponent<ObjTalkData>();
        Talk(objTalkData.talkId);

        SetLastTalkID(objTalkData.talkId);

        talkPanel.SetActive(talkIsActive);
        talkerNamePanel.SetActive(talkIsActive);
        
    }

    



    public void ActivateEscMenuPanel()
    {
        if(escMenuPanelIsActive == false)
        {
            Time.timeScale = 0;
            escMenuPanelIsActive = true;
            escMenuPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            escMenuPanelIsActive = false;
            escMenuPanel.SetActive(false);
        }

    }

    
    private void SetLastTalkID(int id)
    {
        if(id > lastTalkID)
        {
            lastTalkID = id;
        }

    }


   // ESC 메뉴 화면에 나타난 '현재 목표'의 텍스트를, 가장 마지막 대화에 따라 변경해주는 함수입니다.
    private void TextQuestInEscAndTriggerStoryEvent()
    {
        if(lastTalkID == 0)
        {
            nextQuestText.text = " ";
        }
        if(lastTalkID == 100)
        {
            nextQuestText.text = "마을 서쪽에 나타난 괴물을 처치하자!";
        }
        if(lastTalkID == 200)
        {
            nextQuestText.text = "마을 동쪽의 동굴을 통해 과거로 가서, 괴물의 원한을 잠재우자!";
            RemoveCaveBarricade();
        }
        if(lastTalkID == 250)
        {
            nextQuestText.text = "괴물에게 왕에 대한 이야기를 물어보자.";
        }
        if(lastTalkID == 310)
        {
            nextQuestText.text = "포탈을 타고 현재로 돌아가자!";
        }
        if(lastTalkID == 370)
        {
            nextQuestText.text = "포탈을 타고 숲으로 가서 주술사로부터 부적을 받자.";
        }
        else
        {
            nextQuestText.text = " ";
        }

    }










    // (스토리용 함수) 호출 시, "CaveBarricade"라는 이름의 오브젝트가 씬에 존재한다면 파괴합니다.
    public void RemoveCaveBarricade()
    {
        GameObject caveBarricade = GameObject.Find("CaveBarricade");
        if (caveBarricade != null)
        {
            Destroy(caveBarricade);
        }
    }


    // (스토리용 UI 함수) 호출 시, TalkYesNoPanel이 활성화되도록 TM과 Player의 isYesNoOn의 값을 true로 바꿉니다.
    public void OnTogglePanelGhostSuggestion()
    {
        isYesNoOn = true;
        player.GetComponent<Player>().isYesNoOn = true;

        talkYesNoPanel.SetActive(true);

        toggleOneOrTwo = 1;
    }

    // (스토리용 UI 함수) 플레이어가 선택한 선택지가 1 또는 2로 반환됩니다.
    public void ToggleArrowGhostSuggestion()
    {
        if (isYesNoOn)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                toggleOneOrTwo = 2;
                talkYesArrow.SetActive(false);
                talkNoArrow.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                toggleOneOrTwo = 1;
                talkYesArrow.SetActive(true);
                talkNoArrow.SetActive(false);
            }


            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                isYesNoOn = false;
                player.GetComponent<Player>().isYesNoOn = false;

                TriggerTalks(player.GetComponent<Player>().scannedTalker);

                //talkIndex = 0;
                AnswerOneOrTwoToGhost(toggleOneOrTwo);
            }


        }
    }

    // (스토리용 UI 함수) 1번 또는 2번 선택지에 따라 혼령의 talkID가 바뀌고, 그 id의 대사를 새로 시작시킵니다.
    public void AnswerOneOrTwoToGhost(int toggleOneOrTwo)
    {
        
        talkYesNoPanel.SetActive(false);

        // if문: 선택한 번호 보내기.
        GameObject ghost = GameObject.Find("Ghost(DoNotChangeItsName)");

        if(toggleOneOrTwo == 1)
        {
            ghost.GetComponent<ObjTalkData>().talkId = 310;
            TriggerTalks(player.GetComponent<Player>().scannedTalker);

        }
        else if(toggleOneOrTwo == 2)
        {
            ghost.GetComponent<ObjTalkData>().talkId = 370;
            TriggerTalks(player.GetComponent<Player>().scannedTalker);
        }



    }

}
