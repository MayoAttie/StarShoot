using System.Collections;
using UnityEngine;

public class Stage_2Boss : Flight
{
    private float bulletSpeed = 1.8f; // 탄속
    private int bulletCount = 20; // 탄막 개수

    private float angle; // 원주 위의 각도

    private GameObject player;

    [SerializeField]
    Transform BossMovePoint;
    [SerializeField]
    Transform BossInitPoint;

    bool isMoving =true;
    bool MoveCheeck = false;
    public bool isStageBossTrue = true;
    private void Awake()
    {
        maxHp = hp;
        nFireLevel = 0;
        fFireInterval = 0.8f;
        fSpeed = 0.78f;

        BossInitPoint = GameObject.Find("SpawnPoint_1").transform;
        BossMovePoint = GameObject.Find("BossComeOriginPos").transform;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Shoot());
    }
    protected override void Update()
    {

        base.Update();

        // BossMovePoint까지 이동
        if (transform.position.y > BossMovePoint.position.y && MoveCheeck ==false)
        {
            transform.Translate(Vector3.down * fSpeed * Time.deltaTime);
        }
        else
        {
            MoveUpAndDown();
        }

        if (hp <= 0 && isStageBossTrue ==true)
        {
            GameManager.Instance.setStageLevel(GameManager.eSTAGE.Stage3);
            EnemyManager.Instance.SetIsStageIng(false);
            EnemyManager.Instance.SetIsBossApeared(false);
            GameManager.Instance.PlusScore(160000);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null)
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
            GameManager.Instance.PlusScore(70000);
        }
    }

    void MoveUpAndDown()
    {
        MoveCheeck = true;
        if (isMoving)
            transform.Translate(Vector3.up * fSpeed * Time.deltaTime);

        if (transform.position.y <= BossInitPoint.position.y)
            isMoving = true;
        else if (transform.position.y > BossMovePoint.position.y)
            MoveCheeck = false;
    }

    IEnumerator Shoot()
    {
        int numShots = 10;
        float radius = 2f;
        float angleStep = 360f / numShots;

        while (true)
        {
            for (int i = 0; i < numShots; i++)
            {
                // 미사일 생성 및 발사 처리
                GameObject missile = Instantiate(missiles[0], mainCannon.transform.position, Quaternion.identity);
                if (player == null)
                {
                    Debug.LogError("Player not found!");
                    yield break;
                }
                // 타겟 위치 계산
                Vector3 targetDir = (player.transform.position - missile.transform.position).normalized;
                // 미사일 방향으로 힘을 가하기
                missile.GetComponent<Rigidbody2D>().AddForce(targetDir * bulletSpeed, ForceMode2D.Impulse);

                // 탄막 발사 각도 계산
                float angle = 180f + i * angleStep;
                float x = mainCannon.transform.position.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                float y = mainCannon.transform.position.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad)+2f;

                // 탄막 위치 이동 및 회전
                missile.transform.position = new Vector3(x, y, 0f);
                missile.transform.rotation = Quaternion.Euler(0f, 0f, angle);

                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(1f);

            if(hp<=(maxHp/3))
            {
                StartCoroutine(CircleFire());
                yield break;
            }
        }
    }

    IEnumerator CircleFire()
    {
        while(true)
        {
            angle = 0f; // 원주 위의 각도 초기화

            // 원형으로 분산된 방향으로 탄막 발사
            for (int i = 0; i < bulletCount; i++)
            {
                // 원주 위의 각도 계산
                angle += 360f / bulletCount;

                // 방향 계산
                Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);

                // 탄막 발사
                GameObject bullet = Instantiate(missiles[0], mainCannon.transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            }
            yield return new WaitForSecondsRealtime(2.1f);
        }
    }
}
