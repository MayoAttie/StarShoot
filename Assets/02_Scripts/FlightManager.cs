using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class FlightManager : Flight
{
    Controller controller;
    Transform rightCannon;
    Transform leftCannon;

    [SerializeField]
    Image img_FillHp;


    // 폭탄 관련 객체
    [SerializeField]
    GameObject BoomberObject;
    [SerializeField]
    TextMeshProUGUI txt_BoomHave;
    int nHaveBoomCount;


    private void Awake()
    {
        isGameEnd = false;
        fSpeed = 0.65f;
        maxHp = 120;
        hp = 120;
        nFireLevel = 0;
        fFireInterval = 0.3f;
        rightCannon = transform.GetChild(2);
        leftCannon = transform.GetChild(3);
        nHaveBoomCount = 2;
        txt_BoomHave.text = nHaveBoomCount.ToString();

    }

    void Start()
    {
        HpBarView();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
        MainAttackStart(true);
    }

    protected override void Update()
    {
        base.Update();
        MoveWing();

    }

    private void MoveWing()
    {
        float x = controller.GetHorizontalValue();
        float y = controller.GetVerticalValue();


        // 현재 객체의 스크린 좌표를 구함
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);

        // 새로운 객체의 스크린 좌표를 계산함
        Vector3 newScreenPos = screenPos + new Vector3(x, y, 0f) * fSpeed * Time.deltaTime;

        // 객체가 뷰포트를 벗어난다면 위치를 조정함
        newScreenPos.x = Mathf.Clamp01(newScreenPos.x);
        newScreenPos.y = Mathf.Clamp01(newScreenPos.y);
        newScreenPos.y = Mathf.Clamp(newScreenPos.y, 0.25f, 0.96f);
        newScreenPos.x = Mathf.Clamp(newScreenPos.x, 0.07f, 0.93f);

        // 새로운 객체의 월드 좌표를 계산함
        Vector3 newWorldPos = Camera.main.ViewportToWorldPoint(newScreenPos);
        newWorldPos.z = transform.position.z;

        // 객체의 위치를 이동함
        transform.position = newWorldPos;
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 게임 오브젝트의 태그가 "Missile"인 경우에만 체력 감소
        if (collision.gameObject.CompareTag("Missile"))
        {
            // 미사일의 데미지 값을 가져와 체력 감소
            int damage = collision.gameObject.GetComponent<Missile>().damage;
            hp -= damage;

            HpBarView();

            // 체력이 0 이하인 경우에는 파괴 처리
            if (hp <= 0)
            {
                GameManager.Instance.isGameEnd = true;
                Destroy(gameObject);
            }
        }

        // 미사일도 파괴 처리
        Destroy(collision.gameObject);
    }



    public void BoomUseClick()
    {
        if (nHaveBoomCount > 0)
        {
            nHaveBoomCount--;
            txt_BoomHave.text = nHaveBoomCount.ToString();
            Transform initPos = GameObject.Find("SpawnPoint_2").transform;
            Instantiate(BoomberObject, initPos.position, Quaternion.identity);
        }
    }


    public void setHaveBoomCount()
    {
        nHaveBoomCount++;
        txt_BoomHave.text = nHaveBoomCount.ToString();
    }

    public void RecoveryHp(int hp)
    {
        this.hp += hp;
        HpBarView();
    }

    public void HpBarView()
    {
        img_FillHp.fillAmount = (float)hp / (float)maxHp;
    }





}
