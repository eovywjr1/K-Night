using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TalkManager : MonoBehaviour
{
    // public이지만 절대 유니티 인스펙터 창에서 Player 오브젝트를 직접 넣으면 안 됩니다.
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

    // 이전 세이브 포인트.
    private GameObject panelPrefsNull;
    private GameObject panelPrefsNullText;

    // Death Panel.
    public GameObject deathPanel;
    bool playerIsDead = false;

    // 대사 딕셔너리.
    Dictionary<int, string[]> talkData;

    public int lastTalkID = 0;

    // 애니메이션.
    public Animator Amulet;
    public Animator GhostToKing;
    public Animator BCGhost;

    public Animator Medal;
    public Animator Firework1;
    public Animator Firework2;
    public Animator Firework3;

    // YesNo 관련 변수.
    public GameObject talkYesNoPanel;
    public GameObject talkYesArrow;
    public GameObject talkNoArrow;
    private bool isYesNoOn;
    public int toggleOneOrTwo;

    // 체력바 관련.
    private Vector3 HPBarAnchor;
    private GameObject HPBar;
    private GameObject HPBarRed;

    // 엔딩 관련.
    public bool alreadyFireworked = false;
    public GameObject panelClearMedal;

    // 타이틀 화면.
    public GameObject panelNewGame;
    public GameObject panelClears;
    public GameObject panelCreators;

    private void Awake()
    {
        FindSomePanels();
        FindHpbar();
        PlayerTracker();

        isYesNoOn = false;
        talkIsActive = false;
        escMenuPanelIsActive = false;
        talkData = new Dictionary<int, string[]>();
        GenerateTalkData();

        // For nextQuest.
    }

    private void Start()
    {
        


    }

    private void Update()
    {
        ActivateBtStartGameWithNameInTitleScreen();

        ShowMedalClearsInUI_AfterClear();

        CheckPlayerIsDead();

        ShowingHPBar();

        PlayerTracker();

        ToggleArrowGhostSuggestion();

        TextQuestInEscAndTriggerStoryEvent();

        if (Input.GetKeyDown("escape") && playerIsDead == false && (lastTalkID != 350  && lastTalkID != 900))
        {
            ActivateEscMenuPanel();
        }
    }

    private void FixedUpdate()
    {
        CameraAscendInFirstEndingInFixedUpdate();
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
            "괴물을 드디어 쓰러뜨렸다..!:" + player.GetComponent<Player>().myName,
            "괴물에게 왕에 대한 자세한 이야기를 들어보자..:" + player.GetComponent<Player>().myName
        });

        // 과거의 괴물(혼령)과 처치 이후. (300).
        talkData.Add(300, new string[] {
            "나는 원래 이 마을의 왕자로서, 왕이 되었어야 했어.:혼령",
            "그런데 지금 네가 섬기는 왕이 나를 죄인으로 몰아 죽였고:혼령",
            "나는 결국 이렇게 과거에 원혼으로 남게 되었어.:혼령",
            "...:"+ player.GetComponent<Player>().myName, // 화자: 플레이어.
            "마을 근처 호숫가에 한 주술사가 살고 있는 숲이 있는데:혼령", // 적절한 오두막 에셋을 찾으면 '숲'을 '오두막'으로 바꾸겠습니다.
            "그 주술사한테서 부활의 부적을 받을 수 있어.:혼령",
            "부적을 받으면 난 사람으로 되살아날 수 있고 더 강력한 스킬도 사용할 수 있게 돼.:혼령",
            "...:"+ player.GetComponent<Player>().myName, //화자: 플레이어.
            player.GetComponent<Player>().myName + ", 나와 같이 부정한 왕을 처치하고 내가 왕이 되는 것을 도와주지 않을래..? (화살표키와 엔터키로 선택) \n" +
            "[1] 혼령의 말을 무시하고 포탈을 타고 ‘현재’로 가서, 일상으로 돌아간다. \n" +
            "[2] 혼령의 부탁을 수락하고, 포탈을 타고 ‘현재’의 왕을 처치하러 간다.:혼령:YesNo|1|0"
        });

        // (1)을 선택한 경우 괴물이 플레이어에게. (310)
        talkData.Add(310, new string[] {
            "...:혼령",
            "동굴로 가서 현재로 돌아가자..!:"+ player.GetComponent<Player>().myName

        });

        // (1)을 선택한 경우. ((1)을 선택하여 플레이어가 포탈을 타고 난 후 엔딩 씬에서.) (350 ~ 352).
        talkData.Add(350, new string[] {
            player.GetComponent<Player>().myName + "!! 우리 마을을 위해 괴물을 처치해주다니 정말 고맙다!:왕",
            "우리 마을을 구해 준 공적으로 너를 우리 마을의 기사로 임명하겠다!:왕",
            "(괴물이 했던 말이 조금 신경 쓰이기는 하지만...):"+ player.GetComponent<Player>().myName,
            "(평범한 일상으로 돌아가고 기사가 된 것으로 만족하자.):"+ player.GetComponent<Player>().myName
        });

        // (2)를 선택한 경우. 괴물이 플레이어에게. (370).
        talkData.Add(370, new string[] {
            "잘 선택했어!:혼령",
            "포탈을 타고 주술사가 살고 있는 오두막으로 가자.:혼령"
        });

        talkData.Add(380, new string[]
        {
            "주술사에게 말을 걸어서 부적을 받자!:혼령"
        });


        // (2)를 선택한 경우. 오두막의 주술사가 플레이어에게. (400).
        talkData.Add(400, new string[] {
            "이 부적을 붙이면 다시 살아날 수 있다.:주술사",
            "하지만 부적이 떨어지지 않게 조심해야 해. 그러면 다시 혼령으로 돌아갈테니.:주술사"
        });

        // 되살아난 혼령(오두막에서). (500).
        talkData.Add(500, new string[] {
            "자, 이제 성으로 바로 갈 수 있는 포탈을 소환할게.:왕",
            "그 전에 주의할 점은... 미리 말하지 않고 성으로 들어가는 것이라:왕",
            "왕이 바로 공격을 시작할 거라는 점이야.:왕",
            "그리고 왕은 지금까지 네가 본 적 없는 스킬을 쓸 수도 있으니,:왕",
            "포탈을 타기 전에 마음의 준비를 하도록 해.:왕",
            "그럼 건투를 빌게. 꼭 승리해서 왕의 자리를 되찾는거야!:왕"
        });

        // 현재의 왕을 처치하고 나서 괴물로부터 듣는 사건의 전말. (600).
        talkData.Add(600, new string[] {
            player.GetComponent<Player>().myName + "!! 드디어 왕을 처치했군!:왕",
            "우리가 해냈어!!!:왕",
        });
        talkData.Add(601, new string[] {
            "라고 할 줄 알았나?:왕..?",
            "사실 나는 부적과 왕의 마법 지팡이를 얻어서 더 강한 마법을 얻고,:괴물",
            "이 마을을 차지하려 했던 것이다.:괴물",
            "나는 애초에 왕자도 아니었고, 그저 왕의 자리를 노렸을 뿐이었다.:괴물",
            "그래서 반란을 일으키려 했는데 결국 들켰고 처형당했지.:괴물",
            "내가 과거에서 괴물을 소환해서 왕을 처치하기에는 힘이 부족해서,:괴물",
            "너를 이용해서 왕을 처치하고 내가 더 강한 마법을 갖고 마을을 지배하려 했는데,:괴물",
            "일이 너무 잘 풀려버렸네.:괴물",
            "자, 이제 너만 처리하면 끝이다. 그동안 수고 많았다!!!:괴물"
        });

        // 괴물(Re) 처치 후, 플레이어의 독백. (700).
        talkData.Add(700, new string[] {
            "그런 음모를 꾸미고 있었다니...:"+ player.GetComponent<Player>().myName,
            "내가 섬기던 왕은 내가 죽였고.. 어떻게 해야 할까...:"+ player.GetComponent<Player>().myName,
            "괴물이 죽으면서 떨어뜨린 부적을 왕에게 붙이면 왕을 살려낼 수 있을까?:"+ player.GetComponent<Player>().myName
        });

        // 부적을 왕에게 붙이고 나서, 왕과 플레이어의 대화. (800).
        talkData.Add(800, new string[] {
            "정신이 드십니까?:"+ player.GetComponent<Player>().myName,
            "...:왕",
            "정말 죄송합니다... 괴물이 그런 음모를 꾸몄을지는 정말 상상도 못했습니다:"+ player.GetComponent<Player>().myName,
            "저를 용서해주십시오...:"+ player.GetComponent<Player>().myName,
            "...:왕",
            "아니네.:왕",
            "저 괴물도 죽고 나도 죽은 상황에서 다른 선택을 했을 수도 있는데,:왕",
            "오히려 이번 일로 자네에 대한 신뢰가 생긴 것 같네.:왕",
            "게다가 마법을 쓰는 나를 상대로도 이기다니 자네의 실력도 몰라보게 출중해졌군.:왕",
            "자네, 우리 마을의 공식 기사가 되지 않겠나?:왕",
            "....!:"+ player.GetComponent<Player>().myName,
            /*
             아직 수정 중입니다... 이 부분 대사에 대한 아이디어가 있으시다면 직접 수정해주셔도 좋습니다.
             */
        });

        talkData.Add(900, new string[] {
            player.GetComponent<Player>().myName + "!! 우리 마을을 위해 괴물을 처치해주다니 정말 고맙다!:왕",
            "우리 마을을 구해 준 공적으로 너를 우리 마을의 기사로 임명하겠다!:왕",
            "다시 한 번.. 우리 마을을 구해줘서 정말 고맙다..!:왕"
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

            if (GetTalkSentence(id, talkIndex).Split(':').Length >= 3) // 이 Length가 3 이상인 상황은 past after combat 씬의 선택지밖에 없으므로.
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
        if(!( SceneManager.GetActiveScene().name == "Village_FirstEnding" || SceneManager.GetActiveScene().name == "Village_SecondEnding" )){
            Time.timeScale = 0;
        }
        
        ObjectForTalkId = ObjectTriggeringTalk;
        ObjTalkData objTalkData = ObjectForTalkId.GetComponent<ObjTalkData>();

        if (objTalkData.talkId <= 9000 && objTalkData.talkId > 0) //일반적인 NPC 대화.
        {
            Talk(objTalkData.talkId);

            SetLastTalkID(objTalkData.talkId);

            talkPanel.SetActive(talkIsActive);
            talkerNamePanel.SetActive(talkIsActive);
        }
        
    }

    



    public void ActivateEscMenuPanel()
    {
        if(escMenuPanelIsActive == false)
        {
            Time.timeScale = 0;
            escMenuPanelIsActive = true;
            escMenuPanel.SetActive(true);
            ShowMedalClears();
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
            if (nextQuestText != null)
            {
                nextQuestText.text = "";
            }
        }
        else if(lastTalkID == 100)
        {
            nextQuestText.text = "마을 서쪽에 나타난 괴물을 처치하자!";
        }
        else if(lastTalkID == 200)
        {
            nextQuestText.text = "마을 동쪽의 동굴을 통해 과거로 가서, 괴물의 원한을 잠재우자!";
            RemoveCaveBarricadeAndActivatePortalToPast();
        }
        else if(lastTalkID == 250)
        {
            nextQuestText.text = "괴물에게 왕에 대한 이야기를 물어보자.";
        }
        else if(lastTalkID == 310)
        {
            nextQuestText.text = "포탈을 타고 현재로 돌아가자!";
            ActivatePortalVillageToFirstEnding();
        }
    // lastTalkID == 350: FirstEnding 관련 애니메이션 발동.
        else if(lastTalkID == 350)
        {
            if(talkIndex == 2)
            {
                Medal.SetBool("onMedalGiving", true);
                alreadyFireworked = true;
            }
            if(talkIndex == 0 && alreadyFireworked == true)
            {
                GameObject firstEndingTrigger1 = GameObject.Find("FirstEndingTrigger");
                firstEndingTrigger1.GetComponent<ObjTalkData>().talkId = 0;

                Firework1.SetBool("onFirework", true);
                Firework2.SetBool("onFirework", true);
                Firework3.SetBool("onFirework", true);



            }
        }
        else if(lastTalkID == 370)
        {
            ActivatePortalVillageToSorcerer();
            nextQuestText.text = "포탈을 타고 숲으로 가서 주술사로부터 부적을 받자.";
        }
        else if(lastTalkID == 400)
        {
            nextQuestText.text = "혼령에게 말을 걸고, 성으로 가자!";
            Amulet.SetBool("onAmuletUp", true);
            GhostToKing.SetBool("onGhostToKing", true);
            BCGhost.SetBool("onBCGhost", true);

            GameObject GhostFound = GameObject.Find("Ghost");
            GhostFound.GetComponent<ObjTalkData>().talkId = 500;

            //GameObject BCSorcerer1 = GameObject.Find("BubbleCaution_Sorcerer1");
            //BCSorcerer1.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);

        }
        else if(lastTalkID == 500)
        {
            nextQuestText.text = "성으로 가서 부정한 왕을 처치하자!";
            // 여기에 포탈 활성화하는 코드.ㅅㅈ
        }
        else if (lastTalkID == 600)
        {
            nextQuestText.text = "드디어 부정한 왕을 처치했다.. ";
        }
        else if (lastTalkID == 601)
        {
            nextQuestText.text = "왕이 된 괴물을 처지하자..!";
        }
        else if (lastTalkID == 700)
        {
            nextQuestText.text = "왕에게 부적을 붙여보자..!";
        }
        else if (lastTalkID == 800)
        {
            nextQuestText.text = " ";
        }
        else if (lastTalkID == 900)
        {
            if (talkIndex == 1)
            {
                Medal.SetBool("onMedalGiving", true);
                alreadyFireworked = true;
            }
            if (talkIndex == 0 && alreadyFireworked == true)
            {
                GameObject firstEndingTrigger1 = GameObject.Find("FirstEndingTrigger");
                firstEndingTrigger1.GetComponent<ObjTalkData>().talkId = 0;

                Firework1.SetBool("onFirework", true);
                Firework2.SetBool("onFirework", true);
                Firework3.SetBool("onFirework", true);



            }
        }
        else
        {
            //nextQuestText.text = " ";
        }

    }

    private void CameraAscendInFirstEndingInFixedUpdate()
    {
        if (lastTalkID == 350)
        {
            
            if (talkIndex == 0 && alreadyFireworked == true)
            {

                GameObject cameraInThisScene = GameObject.Find("Main Camera");
                cameraInThisScene.transform.position += new Vector3(0, 0.01f, 0);


                if(cameraInThisScene.transform.position.y >= 1.4f)
                {
                    GameObject blackPanelFadeOut = GameObject.Find("BlackPanelFadeOut");
                    blackPanelFadeOut.GetComponent<Image>().color += new Color(0, 0, 0, 0.01f);

                }

                if (cameraInThisScene.transform.position.y >= 2.8f)
                {
                    panelClearMedal.SetActive(true);

                }

                if (cameraInThisScene.transform.position.y >= 3.7f)
                {
                    panelClearMedal.transform.Find("Medal").gameObject.GetComponent<Image>().color -= new Color(0, 0, 0, 0.1f);
                    panelClearMedal.transform.Find("Text").gameObject.GetComponent<Text>().color -= new Color(0, 0, 0, 0.1f);
                }

                if (cameraInThisScene.transform.position.y >= 4.1f)
                {
                    SetEndingCleared();
                    SceneManager.LoadScene("UI_AfterClear");
                }


            }
        }
        if (lastTalkID == 900)
        {

            if (talkIndex == 0 && alreadyFireworked == true)
            {

                GameObject cameraInThisScene = GameObject.Find("Main Camera");
                cameraInThisScene.transform.position += new Vector3(0, 0.01f, 0);


                if (cameraInThisScene.transform.position.y >= 1.4f)
                {
                    GameObject blackPanelFadeOut = GameObject.Find("BlackPanelFadeOut");
                    blackPanelFadeOut.GetComponent<Image>().color += new Color(0, 0, 0, 0.01f);

                }

                if (cameraInThisScene.transform.position.y >= 2.8f)
                {
                    panelClearMedal.SetActive(true);

                }

                if (cameraInThisScene.transform.position.y >= 3.7f)
                {
                    panelClearMedal.transform.Find("Medal").gameObject.GetComponent<Image>().color -= new Color(0, 0, 0, 0.1f);
                    panelClearMedal.transform.Find("Text").gameObject.GetComponent<Text>().color -= new Color(0, 0, 0, 0.1f);
                }

                if (cameraInThisScene.transform.position.y >= 4.1f)
                {
                    SetEndingCleared();
                    SceneManager.LoadScene("UI_AfterClear");
                }
                

            }
        }

    }

    void CheckPlayerIsDead()
    {
        if (player.GetComponent<Player>().hp <= 0)
        {
            playerIsDead = true;
            player.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            if (deathPanel != null)
            {
                deathPanel.SetActive(true);
            }
            Time.timeScale = 0;
        }
    }


    public void SaveGame()
    {
        Save(player.GetComponent<Player>());
    }

    public void LoadGame()
    {

        Load(player.GetComponent<Player>());
        
    }




    // (스토리용 함수) 호출 시, "CaveBarricade"라는 이름의 오브젝트가 씬에 존재한다면 파괴합니다.
    public void RemoveCaveBarricadeAndActivatePortalToPast()
    {
        if (SceneManager.GetActiveScene().name == "Village_Present_AfterTutorial")
        {
            GameObject caveBarricade = GameObject.Find("CaveBarricade");
            if (caveBarricade != null)
            {
                Destroy(caveBarricade);
            }

            GameObject portalToPast = GameObject.Find("PortalParent").transform.Find("Village>Cave").gameObject;
            portalToPast.SetActive(true);
        }
    }

    // 스토리용 함수 :: FirstEnding으로 가는 포탈 활성화.
    public void ActivatePortalVillageToFirstEnding()
    {
        if(SceneManager.GetActiveScene().name == "Village_Past_AfterCombat")
        {
            GameObject portalVillageToFirstEnding = GameObject.Find("PortalParent").transform.Find("Village>FirstEnding").gameObject;
            portalVillageToFirstEnding.SetActive(true);

        }
    }

    // 스토리용 함수 :: 주술사 씬으로 가는 포탈 활성화.
    public void ActivatePortalVillageToSorcerer()
    {
        if (SceneManager.GetActiveScene().name == "Village_Past_AfterCombat")
        {
            GameObject portalVillageToSorcerer = GameObject.Find("PortalParent").transform.Find("Village>Sorcerer").gameObject;
            portalVillageToSorcerer.SetActive(true);

        }
    }

    // 일단 지금은 호출 금지. (스토리용 함수 | 주술사 씬의 애니메이션 eventer입니다.
    public void SorcererEvent()
    {
        int eventTalkIndex = 0;

        talkerNamePanel.SetActive(true);
        talkPanel.SetActive(true);

        talkText.text = "이 부적을 붙이면 다시 인간으로 살아날 수 있다.";
        talkerNameText.text = "주술사";

        while(eventTalkIndex == 0)
        {
            print("While문 실행 중");
            if (IsPressedNPCNextText())
            {
                talkText.text = "하지만 부적이 떨어지지 않게 조심해야 해. 그러면 다시 혼령으로 돌아갈테니.";
                break;
            }

            
        }

        //talkerNamePanel.SetActive(false);
        //talkPanel.SetActive(false);





        Time.timeScale = 1;
    }





    public bool IsPressedNPCNextText()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            while (true)
            {
                if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
                {
                    break;
                }
                
            }
            return true;
        }
        else
        {
            return false;
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

    private void PlayerTracker()
    {
        if (GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player");
        }
        else
        {
            player = null;
        }
    }






    private void FindHpbar()
    {
        HPBar = GameObject.Find("HPBar");
        HPBarRed = GameObject.Find("HPBarRed");
    }

    private void ShowingHPBar()
    {
        if (HPBar != null && HPBarRed != null)
        {
            if (SceneManager.GetActiveScene().name == "TutorialBoss") // 튜토리얼 보스 씬에서 체력바 위치가 이상함.
            {
                Vector3 toControlVector = new Vector3(0, Screen.height / 11.7f, 0);
                HPBarAnchor = Camera.main.WorldToScreenPoint(player.transform.position) + toControlVector;

                HPBar.transform.position = HPBarAnchor;

                HPBarRed.GetComponent<Image>().fillAmount = player.GetComponent<Player>().hp / 100.0f;
            }
            else
            {

                Vector3 toControlVector = new Vector3(2.5f, Screen.height / 9.8f, 0);
                HPBarAnchor = Camera.main.WorldToScreenPoint(player.transform.position) + toControlVector;

                HPBar.transform.position = HPBarAnchor;

                HPBarRed.GetComponent<Image>().fillAmount = player.GetComponent<Player>().hp / 100.0f;

            }

        }
    }


    //저장
    public void Save(Player player)
    {
        PlayerPrefs.SetString("playerName", player.myName);
        PlayerPrefs.SetString("sceneName", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("Hp", player.hp);
        PlayerPrefs.SetFloat("playerX", player.transform.position.x);
        PlayerPrefs.SetFloat("playerY", player.transform.position.y);
    }

    //불러오기
    private void Load(Player player)
    {
        if (PlayerPrefs.HasKey("playerX") == true)
        {
            float x = PlayerPrefs.GetFloat("playerX");
            float y = PlayerPrefs.GetFloat("playerY");
            player.transform.position = new Vector3(x, y, 0);
            SceneManager.LoadScene(PlayerPrefs.GetString("sceneName"));
            player.myName = PlayerPrefs.GetString("playerName");
            player.hp = PlayerPrefs.GetInt("Hp");
            Time.timeScale = 1;
            print("게임 로드 성공");
        }
        else
        {
            if (escMenuPanel != null)
            {
                ActivateEscMenuPanel();
            }
            panelPrefsNull.SetActive(true);
            panelPrefsNull.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            panelPrefsNullText.GetComponent<Text>().color = new Color(0.7215f, 0.7215f, 0.7215f, 1);
            
            Time.timeScale = 1;
            Invoke("PrefsNullPanelHide", 0.8f);
            print("게임 데이터 없음");
            
        }
        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    void PrefsNullPanelHide()
    {
        panelPrefsNull.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        panelPrefsNullText.GetComponent<Text>().color = new Color(0.7215f, 0.7215f, 0.7215f, 0);
        panelPrefsNull.SetActive(false);
    }

    void FindSomePanels()
    {
        if (GameObject.Find("Canvas").transform.Find("PanelPrefsNull").gameObject != null)
        {
            panelPrefsNull = GameObject.Find("Canvas").transform.Find("PanelPrefsNull").gameObject;

            if(panelPrefsNull.transform.Find("PanelPrefsNullText").gameObject != null)
            {
                panelPrefsNullText = panelPrefsNull.transform.Find("PanelPrefsNullText").gameObject;
            }
        }

        
    }

    public void SetEndingCleared()
    {
        if(SceneManager.GetActiveScene().name == "Village_FirstEnding")
        {
            PlayerPrefs.SetInt("ClearedNormalEnding", 1);
        }
        if (SceneManager.GetActiveScene().name == "Village_SecondEnding")
        {
            PlayerPrefs.SetInt("ClearedHappyEnding", 1);
        }

    }

    void ShowMedalClearsInUI_AfterClear()
    {
        if(SceneManager.GetActiveScene().name == "UI_AfterClear")
        {
            GameObject medalNormal = GameObject.Find("MedalNormal");
            GameObject medalHappy = GameObject.Find("MedalHappy");
            GameObject medalHidden = GameObject.Find("MedalHidden");

            GameObject textMedalNormal = GameObject.Find("TextNormalClear");
            GameObject textMedalHappy = GameObject.Find("TextHappyClear");
            GameObject textMedalHidden = GameObject.Find("TextHiddenClear");

            if (PlayerPrefs.HasKey("ClearedNormalEnding") == true)
            {
                if(PlayerPrefs.GetInt("ClearedNormalEnding") == 1)
                {
                    medalNormal.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    textMedalNormal.GetComponent<Text>().color += new Color(0, 0, 0, 1);
                }
            }

            if (PlayerPrefs.HasKey("ClearedHappyEnding") == true)
            {
                if (PlayerPrefs.GetInt("ClearedHappyEnding") == 1)
                {
                    medalHappy.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    textMedalHappy.GetComponent<Text>().color += new Color(0, 0, 0, 1);
                }
            }

            if (PlayerPrefs.HasKey("ClearedNormalEnding") == true && PlayerPrefs.HasKey("ClearedHappyEnding") == true)
            {
                if (PlayerPrefs.GetInt("ClearedNormalEnding") == 1 && PlayerPrefs.GetInt("ClearedHappyEnding") == 1)
                {
                    medalHidden.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    textMedalHidden.GetComponent<Text>().color += new Color(0, 0, 0, 1);
                }
            }







        }

        


    }

    public void ShowMedalClears()
    {
        if(escMenuPanelIsActive == true)
        {
            if (PlayerPrefs.HasKey("ClearedNormalEnding") == true && PlayerPrefs.GetInt("ClearedNormalEnding") == 1)
            {
                GameObject.Find("MedalNormal").GetComponent<Image>().color += new Color(0, 0, 0, 1);
                GameObject.Find("TextNormalClear").GetComponent<Text>().text = "클리어:\n노말 엔딩";
            }
            if (PlayerPrefs.HasKey("ClearedHappyEnding") == true && PlayerPrefs.GetInt("ClearedHappyEnding") == 1)
            {
                GameObject.Find("MedalRealClear").GetComponent<Image>().color += new Color(0, 0, 0, 1);
                GameObject.Find("TextRealClear").GetComponent<Text>().text = "클리어:\n해피 엔딩";
            }
            if (PlayerPrefs.HasKey("ClearedNormalEnding") == true && PlayerPrefs.HasKey("ClearedHappyEnding") == true && PlayerPrefs.GetInt("ClearedNormalEnding") == 1 && PlayerPrefs.GetInt("ClearedHappyEnding") == 1)
            {
                GameObject.Find("MedalHidden").GetComponent<Image>().color += new Color(0, 0, 0, 1);
                GameObject.Find("TextHiddenClear").GetComponent<Text>().text = "클리어:\n히든 엔딩";
            }



        }
    }

    public void ShowMedalClearsInTitleScreen()
    {
        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            GameObject medalNormal = GameObject.Find("MedalNormal");
            GameObject medalHappy = GameObject.Find("MedalHappy");
            GameObject medalHidden = GameObject.Find("MedalHidden");

            GameObject textMedalNormal = GameObject.Find("TextNormalClear");
            GameObject textMedalHappy = GameObject.Find("TextHappyClear");
            GameObject textMedalHidden = GameObject.Find("TextHiddenClear");

            if (PlayerPrefs.HasKey("ClearedNormalEnding") == true)
            {
                if (PlayerPrefs.GetInt("ClearedNormalEnding") == 1)
                {
                    medalNormal.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    textMedalNormal.GetComponent<Text>().color += new Color(0, 0, 0, 1);
                }
            }

            if (PlayerPrefs.HasKey("ClearedHappyEnding") == true)
            {
                if (PlayerPrefs.GetInt("ClearedHappyEnding") == 1)
                {
                    medalHappy.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    textMedalHappy.GetComponent<Text>().color += new Color(0, 0, 0, 1);
                }
            }

            if (PlayerPrefs.HasKey("ClearedNormalEnding") == true && PlayerPrefs.HasKey("ClearedHappyEnding") == true)
            {
                if (PlayerPrefs.GetInt("ClearedNormalEnding") == 1 && PlayerPrefs.GetInt("ClearedHappyEnding") == 1)
                {
                    medalHidden.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    textMedalHidden.GetComponent<Text>().color += new Color(0, 0, 0, 1);
                }
            }
        }
    }

    // Title Screen.
    public void ActivateBtStartGameWithNameInTitleScreen()
    {
        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            if (panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text.Length > 0 &&
                panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text.Length <= 6 &&
                !(panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "" ||
                panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == " " ||
                panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "  " ||
                panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "   " ||
                panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "    " ||
                panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "     " ||
                panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "      " ||
                panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "       "))
            {
                panelNewGame.transform.Find("BtStart").gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                panelNewGame.transform.Find("BtStart").gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void BtNewGame()
    {
        panelNewGame.SetActive(true);
        
    }

    public void BtStartGameWithName()
    {
        panelNewGame.SetActive(false);
        if(panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "" ||
            panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == " " ||
            panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "  " ||
            panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "   " ||
            panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "    " ||
            panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "     " ||
            panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "      " ||
            panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text == "       ")
        {
            player.GetComponent<Player>().myName = "플레이어";
        }
        else
        {
            player.GetComponent<Player>().myName = panelNewGame.transform.Find("InputField").transform.Find("Text").gameObject.GetComponent<Text>().text;
        }
        player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        player.GetComponent<SpriteRenderer>().flipX = false;
        player.transform.position = new Vector3(17.836f, -1.324f, 0);
        SceneManager.LoadScene("Village_Present");
        Time.timeScale = 1;
    }

    public void BtCloseNewGamePanel()
    {
        panelNewGame.SetActive(false);
    }
    
    public void BtLoadGame()
    {
        LoadGame();
    }

    public void BtMedals()
    {
        panelClears.SetActive(true);
        ShowMedalClearsInTitleScreen();
    }

    public void BtCloseClearsPanel()
    {
        panelClears.SetActive(false);
    }

    public void BtGoToCredits()
    {
        panelCreators.SetActive(true);
    }

    public void BtCloseCreatorsPanel()
    {
        panelCreators.SetActive(false);
    }

    public void BtExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void BtGoToTitle()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
        SceneManager.LoadScene("TitleScreen");
    }

    public void BtChooseAnother()
    {
        player.GetComponent<SpriteRenderer>().flipX = true;
        player.transform.position = new Vector3(-1.6f, -1.324f, 0);
        SceneManager.LoadScene("Village_Past_AfterCombat");
    }

    


}
