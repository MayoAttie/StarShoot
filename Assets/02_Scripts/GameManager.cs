using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public enum eSTAGE
    {
        None =0,
        Stage1,
        Stage2,
        Stage3,
        Clear,
        Max
    }

    public enum eGAME_STATE
    {
        None = 0,
        Ing,
        End,
        Max
    }

    LinkedList<Sprite> Bgr_images = new LinkedList<Sprite>();

    // 배경진행 관련 변수
    [SerializeField]
    Sprite[] img_BgrSprites;
    [SerializeField]
    Transform bgr_OriginPos;
    [SerializeField]
    Image img_BgrField;


    // 게임 진행용 텍스트
    [SerializeField]
    TextMeshProUGUI txt_Score;
    [SerializeField]
    TextMeshProUGUI txt_FrontText;
    [SerializeField]
    Image img_FrontTextBgr;
    [SerializeField]
    Image img_EndGameBgr;


    public bool isGameEnd;
    int nKillCount;
    int nScore;
    bool isCoroutinFlag;

    [SerializeField]
    eSTAGE eStage;
    eGAME_STATE eGameStage;

    static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        nScore = 0;
        instance = this;
        eGameStage = eGAME_STATE.None;
        eStage = eSTAGE.None;
        nKillCount = 0;
        instance = this;
        isGameEnd = false;
        isCoroutinFlag = false;
        txt_Score.text = "점수 : ";
        img_EndGameBgr.gameObject.SetActive(false);
        SetBgrSizeAndLink();

    }

    void Start()
    {
        StartCoroutine(BackGroundMove());
        StartCoroutine(GameProcess());
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameEnd)
        {
            eGameStage = eGAME_STATE.End;
        }
        if(eStage == eSTAGE.Clear)
        {
            isGameEnd =true;
        }
    }

    #region 게임 프로세스
    IEnumerator GameProcess()
    {
        while(true)
        {
            switch (eGameStage)
            {
                // 현재 상태가 None일 경우,
                case eGAME_STATE.None:
                    eGameStage = eGAME_STATE.Ing;
                    eStage = eSTAGE.Stage1;
                    break;
                // 현재 상태가 Ing일 경우,
                case eGAME_STATE.Ing:
                    StageProcess();
                    break;
                // 현재 상태가 End일 경우,
                case eGAME_STATE.End:
                    if(!isCoroutinFlag)
                    {
                        // 상태가 클리어인지 확인, 승패 판정
                        if (eStage == eSTAGE.Clear)
                            Victory();
                        else
                            Fail();
                    }
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(0.1f);
        }

    }
    #endregion


    void StageProcess()
    {
        if (!isGameEnd)
        {
            switch (eStage)
            {
                // 적 객체를 생성하기 위한, EnemyManager의 StageStart함수 호출
                case eSTAGE.Stage1:
                    if (EnemyManager.Instance.isStageIng == false)
                    {
                        EnemyManager.Instance.StageStart(eStage);
                    }
                    break;

                case eSTAGE.Stage2:

                    if (EnemyManager.Instance.isStageIng == false)
                    {
                        EnemyManager.Instance.StageStart(eStage);
                    }
                    break;

                case eSTAGE.Stage3:

                    if (EnemyManager.Instance.isStageIng == false)
                    {
                        EnemyManager.Instance.StageStart(eStage);
                    }
                    break;

            }
        }
    }

    #region 승리 패배
    void Victory()
    {
        isCoroutinFlag = true;
        img_FrontTextBgr.gameObject.SetActive(true);
        txt_FrontText.gameObject.SetActive(true);
        txt_FrontText.text = " 승 리!";
        img_EndGameBgr.gameObject.SetActive(true);

    }

    void Fail()
    {
        isCoroutinFlag = true;
        img_FrontTextBgr.gameObject.SetActive(true);
        txt_FrontText.gameObject.SetActive(true);
        txt_FrontText.text = " 패 배...";
        img_EndGameBgr.gameObject.SetActive(true);
    }

    #endregion


    #region 배경 슬라이드
    IEnumerator BackGroundMove()
    {
        float moveSpeed = 10f; // 배경 이미지의 이동 속도

        LinkedListNode<Sprite> currentNode = Bgr_images.First;
        while (!isGameEnd)
        {
            yield return new WaitForSeconds(0.05f);

            // 배경 이미지를 아래로 이동시킴
            img_BgrField.transform.position -= new Vector3(0f, moveSpeed * Time.deltaTime, 0f);

            // 배경 이미지가 일정 위치 아래로 이동하면 위치를 초기화함
            if (img_BgrField.transform.position.y < 0f)
            {
                // 현재 스프라이트를 다음 스프라이트로 바꾸고, 위치를 초기화함
                currentNode = currentNode.Next ?? Bgr_images.First;
                img_BgrField.sprite = currentNode.Value;

                img_BgrField.transform.position = bgr_OriginPos.position;
            }
        }
    }

    void SetBgrSizeAndLink()
    {
        foreach (Sprite sprite in img_BgrSprites)
        {
            Bgr_images.AddLast(sprite);
        }
    }
    #endregion

    #region Getter,Setter
    public void plusKillCount(int num)
    {
        nKillCount += num;
        nScore = nKillCount * 1000;
        txt_Score.text = "점수 : " + nScore;
    }
    public void PlusScore(int num)
    {
        nScore += num;
        txt_Score.text = "점수 : " + nScore;
    }
    public int getKillCount()
    {
        return nKillCount;
    }

    public eSTAGE getStageLevel()
    {
        return eStage;
    }
    public void setStageLevel(eSTAGE stage)
    {
        this.eStage = stage;
    }
    #endregion


    public void ExitGame()
    {
        Application.Quit();
    }
    public void ToLobbyFunc()
    {
        SceneLoadderManager._instance.SceneLoadder("02_LobbyScene");
    }
}
