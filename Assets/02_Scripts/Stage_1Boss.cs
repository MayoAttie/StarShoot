using System.Collections;
using UnityEngine;

public class Stage_1Boss : Flight
{
    // 서브캐논 Transform
    [SerializeField]
    Transform SubFirePos_1;
    [SerializeField]
    Transform SubFirePos_2;

    // 이동시작지점
    Transform MoveingPoint;

    // 매인캐논 오브젝트
    [SerializeField]
    GameObject MainCannonPrefab;

    int subFireIndex;
    private bool movingRight = true;
    public bool isStageBossTrue = true;


    private void Awake()
    {
        MoveingPoint = GameObject.Find("BossComeOriginPos").transform;
        nFireLevel = 1;
        subFireIndex = 0;
        fFireInterval = 0.8f;
        fSpeed = 0.4f;
    }

    void Start()
    {
        StartCoroutine(SubFire());
        StartCoroutine(MainCannon());
    }
    protected override void Update()
    {

        base.Update();
        if (transform.position.y > MoveingPoint.position.y)
        {
            transform.Translate(Vector3.down * fSpeed * Time.deltaTime);
        }
        else
        {
            // Move left or right
            MoveRightAndLeft();
        }

        if(hp<=0 && isStageBossTrue == true)
        {
            GameManager.Instance.setStageLevel(GameManager.eSTAGE.Stage2);
            EnemyManager.Instance.SetIsStageIng(false);
            EnemyManager.Instance.SetIsBossApeared(false);
            GameManager.Instance.PlusScore(70000);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                FlightManager playCls = player.GetComponent<FlightManager>();
                int maxHp = playCls.getMaxHp();
                if (playCls.getHp() + 20 >= maxHp)
                {
                    playCls.setHp(maxHp);
                    playCls.HpBarView();
                }
                else
                {
                    playCls.RecoveryHp(20);
                }
                playCls.getFireLevel();
                int len = playCls.missiles.Length;
                if (playCls.getFireLevel() >= len - 1)
                    return;
                else
                {
                    int num = playCls.getFireLevel();
                    playCls.setFireLevel(num + 1);
                }
                playCls.setHaveBoomCount();
            }

        }
        else if(hp <= 0 && isStageBossTrue == false)
        {
            GameManager.Instance.PlusScore(20000);
        }

    }

    void MoveRightAndLeft()
    {
        if (movingRight)
        {
            transform.Translate(Vector3.right * fSpeed * Time.deltaTime * 2);
        }
        else
        {
            transform.Translate(Vector3.left * fSpeed * Time.deltaTime * 2);
        }

        if (Mathf.Abs(transform.position.x) > 2)
        {
            movingRight = !movingRight;
        }
    }



    IEnumerator SubFire()
    {
        while(true)
        {
            {
                GameObject obj = Instantiate(missiles[subFireIndex]);
                obj.transform.position = SubFirePos_1.transform.position;
                Rigidbody2D rb = obj.transform.GetComponent<Rigidbody2D>();
                rb.AddForce(-mainCannon.up * 5f, ForceMode2D.Impulse);
            }
            {
                GameObject obj = Instantiate(missiles[subFireIndex]);
                obj.transform.position = SubFirePos_2.transform.position;
                Rigidbody2D rb = obj.transform.GetComponent<Rigidbody2D>();
                rb.AddForce(-mainCannon.up * 5f, ForceMode2D.Impulse);
            }


             
            yield return new WaitForSeconds(fFireInterval);
        }
    }

    IEnumerator MainCannon()
    {
        while (true)
        {
            // Player 태그를 가진 객체의 위치를 찾음
            GameObject target = GameObject.FindGameObjectWithTag("Player");

            if (target == null)
            {
                Debug.LogError("Player not found!");
                yield break;
            }

            //캐논 회전함수
            StartCoroutine(MainCannonRotate(target));

            // 미사일 발사
            GameObject obj = Instantiate(missiles[nFireLevel]);
            obj.transform.position = mainCannon.position;

            // 미사일 방향 설정
            Vector3 direction = target.transform.position - mainCannon.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.AngleAxis(angle + 180f, Vector3.forward); // 180도 회전 추가
            obj.transform.rotation = rotation;

            // 미사일 발사
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.rotation = obj.transform.rotation.eulerAngles.z;
            rb.AddForce(-obj.transform.up * 4f, ForceMode2D.Impulse);

            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator MainCannonRotate(GameObject target)
    {
        while(true)
        {


            // Player가 없으면 0.5초 대기
            if (target == null)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            // 메인 캐논이 Player를 향해 회전하도록 함
            Vector3 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            MainCannonPrefab.transform.rotation = Quaternion.RotateTowards(MainCannonPrefab.transform.rotation, q, 180f);

            yield return new WaitForEndOfFrame();
        }
    }
}
