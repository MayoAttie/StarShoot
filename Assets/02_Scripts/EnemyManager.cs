using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    // 적기체배열
    [SerializeField]
    GameObject[] enemyFlights;

    // 스폰 포인트들
    [SerializeField]
    Transform[] spawnPoints;

    // 스테이지 진행유무
    public bool isStageIng;
    private bool isBossAppeared;

    // 스테이지 안내문구
    [SerializeField]
    Image img_ExplainBgr;
    [SerializeField]
    TextMeshProUGUI txt_ExplainText;

    static EnemyManager instance;
    public static EnemyManager Instance
    { get { return instance; } }

    private void Awake()
    {
        isBossAppeared=false;
        instance = this;
        isStageIng = false;
        img_ExplainBgr.gameObject.SetActive(false);
        txt_ExplainText.gameObject.SetActive(false);
    }
    private void Start()
    {
    }
    private void Update()
    {
        if (GameManager.Instance.isGameEnd)
            return;
    }

    public void StageStart(GameManager.eSTAGE stage)
    {
        isStageIng = true;
        switch (stage)
        {
            case GameManager.eSTAGE.Stage1:
                SoundManager._instance.PlayBGM(SoundManager.eTYPE_BGM.Stage_1);
                // 가이드 텍스트 Active True
                img_ExplainBgr.gameObject.SetActive(true);
                txt_ExplainText.gameObject.SetActive(true);
                // 가이드 텍스트 Active False 함수 호출
                Invoke("TextOff", 2f);
                // 적 비행체 생성 함수 호출
                StartCoroutine(Stage_1());
                break;
            case GameManager.eSTAGE.Stage2:
                SoundManager._instance.PlayBGM(SoundManager.eTYPE_BGM.Stage_2);
                txt_ExplainText.text= "스테이지 : 2";
                img_ExplainBgr.gameObject.SetActive(true);
                txt_ExplainText.gameObject.SetActive(true);
                Invoke("TextOff", 2f);
                StartCoroutine(Stage_2());
                break;
            case GameManager.eSTAGE.Stage3:
                SoundManager._instance.PlayBGM(SoundManager.eTYPE_BGM.Stage_3);
                txt_ExplainText.text = "스테이지 : 3";
                img_ExplainBgr.gameObject.SetActive(true);
                txt_ExplainText.gameObject.SetActive(true);
                Invoke("TextOff", 2f);
                StartCoroutine(Stage_3());
                break;
        }
    }


    void TextOff()
    {
        img_ExplainBgr.gameObject.SetActive(false);
        txt_ExplainText.gameObject.SetActive(false);
    }

    IEnumerator Stage_1()
    {

        while (isStageIng)
        {
            if (GameManager.Instance.isGameEnd)
                yield break;
            if (GameManager.Instance.getKillCount() <10)
            {
                Transform spawnPoint = spawnPoints[0];
                Vector3 spawnOffset = new Vector3(Random.Range(-2f, 2f),0f, 0f);

                GameObject obj = Instantiate(enemyFlights[0], spawnPoint.position + spawnOffset, Quaternion.identity);
                EnemyFlight ef = new EnemyFlight(20, 1, 1.8f);
                obj.GetComponent<EnemyFlight>().Copy(ef);
            }
            else if(isBossAppeared ==false)
            {
                Transform spawnPoint = spawnPoints[0];
                GameObject obj = Instantiate(enemyFlights[2], spawnPoint.position, Quaternion.identity);
                isBossAppeared=true;
                yield break;
            }

            yield return new WaitForSeconds(2.1f);
        }
    }

    IEnumerator Stage_2()
    {
        while(isStageIng)
        {
            if (GameManager.Instance.isGameEnd)
                yield break;
            if (GameManager.Instance.getKillCount() < 25 && isBossAppeared == false)
            {
                int rand = Random.Range(0,10);

                if(rand>4)
                {
                    Transform spawnPoint = spawnPoints[0];
                    Vector3 spawnOffset = new Vector3(Random.Range(-2f, 2f), 0f, 0f);

                    GameObject obj = Instantiate(enemyFlights[0], spawnPoint.position + spawnOffset, Quaternion.identity);
                    EnemyFlight ef = new EnemyFlight(30, 1.1f, 1.7f);
                    obj.GetComponent<EnemyFlight>().Copy(ef);
                }
                else
                {
                    Transform spawnPoint = spawnPoints[0];
                    Vector3 spawnOffset = new Vector3(Random.Range(-2f, 2f), 0f, 0f);

                    GameObject obj = Instantiate(enemyFlights[1], spawnPoint.position + spawnOffset, Quaternion.identity);
                    EnemyFlight ef = new EnemyFlight(50, 0.9f, 2.3f);
                    obj.GetComponent<EnemyFlight>().Copy(ef);
                }
                
            }
            else if (isBossAppeared == false)
            {
                Transform spawnPoint = spawnPoints[0];
                GameObject obj = Instantiate(enemyFlights[3], spawnPoint.position, Quaternion.identity);
                isBossAppeared = true;
                yield break;
            }
            yield return new WaitForSeconds(1.8f);
        }
    }

    IEnumerator Stage_3()
    {
        while (isStageIng)
        {
            if (GameManager.Instance.isGameEnd)
                yield break;
            if (GameManager.Instance.getKillCount() < 60 && isBossAppeared == false)
            {
                int rand = Random.Range(0, 10);
                // 킬 카운트 조건문
                if (GameManager.Instance.getKillCount() == 30|| GameManager.Instance.getKillCount() == 40)
                {
                    // 스테이지 1 보스를 중간 보스로 생성
                    Transform spawnPoint = spawnPoints[0];
                    GameObject obj = Instantiate(enemyFlights[2], spawnPoint.position, Quaternion.identity);
                    Stage_1Boss cls = obj.GetComponent<Stage_1Boss>();
                    cls.setHp(2000);
                    cls.setFireInterval(0.4f);
                    cls.isStageBossTrue = false;    //보스가 아님을 부울 변수로 초기화
                }

                if (GameManager.Instance.getKillCount() == 50 && isBossAppeared == false)
                {
                    // 스테이지 2 보스를 중간 보스로 생성
                    Transform spawnPoint = spawnPoints[0];
                    GameObject obj = Instantiate(enemyFlights[3], spawnPoint.position, Quaternion.identity);
                    Stage_2Boss cls = obj.GetComponent<Stage_2Boss>();
                    cls.setHp(4500);
                    cls.isStageBossTrue = false;    //보스가 아님을 부울 변수로 초기화
                }

                if (rand >7)    // 랜덤으로 enemyFlight를 결정.
                {
                    Transform spawnPoint = spawnPoints[0];
                    Vector3 spawnOffset = new Vector3(Random.Range(-2f, 2f), 0f, 0f);

                    GameObject obj = Instantiate(enemyFlights[0], spawnPoint.position + spawnOffset, Quaternion.identity);
                    EnemyFlight ef = new EnemyFlight(40, 1.2f, 1.5f);   // Enemy스테이터스 초기화
                    obj.GetComponent<EnemyFlight>().Copy(ef);
                }
                else
                {
                    Transform spawnPoint = spawnPoints[0];
                    Vector3 spawnOffset = new Vector3(Random.Range(-2f, 2f), 0f, 0f);

                    GameObject obj = Instantiate(enemyFlights[1], spawnPoint.position + spawnOffset, Quaternion.identity);
                    EnemyFlight ef = new EnemyFlight(55, 1f, 2.1f);     // Enemy스테이터스 초기화
                    obj.GetComponent<EnemyFlight>().Copy(ef);
                }

            }
            else if (isBossAppeared == false)
            {
                // 보스 비행체 생성
                Transform spawnPoint = spawnPoints[0];
                GameObject obj = Instantiate(enemyFlights[4], spawnPoint.position, Quaternion.identity);
                isBossAppeared = true;
                yield break;
            }
            yield return new WaitForSeconds(1.5f);
        }
    }



    public void SetIsBossApeared(bool isBossApeared)
    {
        this.isBossAppeared= isBossApeared;
    }
    public void SetIsStageIng(bool isStageIng)
    {
        this.isStageIng= isStageIng;
    }

}
