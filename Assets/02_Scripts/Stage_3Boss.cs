using System.Collections;
using UnityEngine;

public class Stage_3Boss : Flight
{

    Transform[] bossPosArr;
    bool isMoveTrue;
    [SerializeField]
    Transform BackCannon;

    [SerializeField]
    GameObject BackCannonPrefab;

    private float bulletSpeed = 0.9f; // 탄속
    private int bulletCount = 14; // 탄막 개수

    private float angle; // 원주 위의 각도
    private Coroutine backCannonCoroutine;  //backCannon
    private Coroutine backCannonCoroutine2; //CircleFire
    private Coroutine backCannonCoroutine3; //MainCannon1
    private Coroutine backCannonCoroutine4; //MainCannon2




    float fBulletSpeed;
    private void Awake()
    {
        isMoveTrue = false;
        maxHp = hp;
        nFireLevel = 0;
        fFireInterval = 0.8f;
        fSpeed = 0.78f;
        fBulletSpeed = 5f;

        bossPosArr = new Transform[5];
        bossPosArr[0] = GameObject.Find("BossComeOriginPos").transform;
        bossPosArr[1] = GameObject.Find("BossPos_1").transform;
        bossPosArr[2] = GameObject.Find("BossPos_2").transform;
        bossPosArr[3] = GameObject.Find("BossPos_3").transform;
        bossPosArr[4] = GameObject.Find("BossPos_4").transform;
    }
    void Start()
    {

    }

    protected override void Update()
    {

        base.Update();
        if (transform.position.y > bossPosArr[0].position.y && isMoveTrue==false)
        {
            transform.Translate(Vector3.down * fSpeed * Time.deltaTime);
        }
        else
        {
            if(isMoveTrue==false)
            {
                StartCoroutine(Move());
            }
        }

        if(hp<=0)
        {
            GameManager.Instance.setStageLevel(GameManager.eSTAGE.Clear);
            EnemyManager.Instance.SetIsStageIng(false);
            EnemyManager.Instance.SetIsBossApeared(false);
            GameManager.Instance.PlusScore(500000);
        }
    }


    IEnumerator Move()
    {
        isMoveTrue = true;
        int index = 0;
        
        while (true)
        {
            // 보스의 x 좌표가 현재 위치보다 크고 인덱스가 0인 경우
            if (bossPosArr[1].position.x > transform.position.x && index == 0)
            {
                // 백캐논 발사 코루틴이 실행 중이지 않은 경우 실행합니다
                if (backCannonCoroutine == null)
                    backCannonCoroutine = StartCoroutine("BackCannonFire");
                if (backCannonCoroutine3 == null)
                    backCannonCoroutine3 = StartCoroutine("MainCannonFire");
                if (backCannonCoroutine4 == null)
                    backCannonCoroutine4 = StartCoroutine("MainCannonFire2");
                // CircleFire 코루틴을 정지합니다
                StopCoroutine("CircleFire");
                // 오른쪽으로 이동합니다
                transform.Translate(Vector3.right * fSpeed * Time.deltaTime);
                // 현재 위치와 다음 위치 사이의 거리를 계산합니다
                float dist = Vector3.Distance(transform.position, bossPosArr[index + 1].position);

                if (dist <= 0.1f)
                {
                    index = 1;
                }
            }
            // 보스의 y 좌표가 현재 위치보다 작고 인덱스가 1인 경우
            else if (bossPosArr[2].position.y < transform.position.y && index == 1)
            {
                // 아래로 이동합니다
                transform.Translate(Vector3.down * fSpeed * Time.deltaTime);
                // 현재 위치와 다음 위치 사이의 거리를 계산합니다
                float dist = Vector3.Distance(transform.position, bossPosArr[index + 1].position);

                if (dist <= 0.1f)
                {
                    index = 2;
                }
            }
            // 보스의 x 좌표가 현재 위치보다 작고 인덱스가 2인 경우
            else if (bossPosArr[3].position.x < transform.position.x && index == 2)
            {
                // 왼쪽으로 이동합니다
                transform.Translate(Vector3.left * fSpeed * Time.deltaTime);
                // 현재 위치와 다음 위치 사이의 거리를 계산합니다
                float dist = Vector3.Distance(transform.position, bossPosArr[index + 1].position);

                if (dist <= 0.1f)
                {
                    index = 3;
                }
            }
            // 보스의 y 좌표가 현재 위치보다 크고 인덱스가 3인 경우
            else if (bossPosArr[4].position.y > transform.position.y && index == 3)
            {
                // 위로 이동합니다
                transform.Translate(Vector3.up * fSpeed * Time.deltaTime);
                // 현재 위치와 다음 위치 사이의 거리를 계산합니다
                float dist = Vector3.Distance(transform.position, bossPosArr[index + 1].position);

                if (dist <= 0.1f)
                {
                    index = 4;
                }
            }
            // 보스의 x 좌표가 현재 위치보다 크고 인덱스가 4인 경우
            else if(bossPosArr[0].position.x > transform.position.x && index == 4)
            {
                // 오른쪽으로 이동합니다.
                transform.Translate(Vector3.right * fSpeed * Time.deltaTime);
                // 현재 위치와 초기 위치 사이의 거리를 계산합니다.
                float dist = Vector3.Distance(transform.position, bossPosArr[0].position);
                if (dist <= 0.1f)
                {
                    // CircleFire 코루틴을 실행합니다.
                    if (backCannonCoroutine2 == null)
                        backCannonCoroutine2 = StartCoroutine("CircleFire");
                    // 발사 코루틴을 정지합니다.
                    StopCoroutine("BackCannonFire");
                    StopCoroutine("MainCannonFire");
                    StopCoroutine("MainCannonFire2");
                    // 15초 동안 대기합니다.
                    yield return new WaitForSeconds(15f);
                    // 인덱스를 0으로 초기화합니다.
                    index = 0;
                }
            }
            // 다음 프레임까지 대기합니다.
            yield return null;
        }
    }

    #region MainCannn

    IEnumerator MainCannonFire()
    {
        while (true)
        {
            Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
            if (playerPos == null)
            {
                Debug.LogError("Player not found!");
                yield break;
            }
            // 방향 벡터
            Vector3 direction = playerPos.position - transform.position;
            direction.Normalize();

            // 수직인 왼쪽 벡터와 오른쪽 벡터
            Vector3 left = new Vector3(-direction.y, direction.x, 0f);
            Vector3 right = new Vector3(direction.y, -direction.x, 0f);

            // 방향 벡터와 수직인 왼쪽, 오른쪽 방향 벡터를 이용해 총 네 개의 방향 벡터 생성
            Vector3 dir1 = (direction + left).normalized;
            Vector3 dir2 = (direction - left).normalized;
            Vector3 dir3 = (direction + right).normalized;
            Vector3 dir4 = (direction - right).normalized;

            // 생성된 방향 벡터를 이용해 총 네 개의 탄 생성
            Vector3 pos1 = new Vector3(0f, 0f, 0);
            Vector3 pos2 = new Vector3(0.1f, 0f, 0);
            Vector3 pos3 = new Vector3(0f, 0.1f, 0);
            Vector3 pos4 = new Vector3(0.1f, 0.1f, 0);

            // 구한 위치 벡터를 캐논 Position에 보정하여 탄환 발사
            GameObject bullet1 = Instantiate(missiles[1], mainCannon.position + pos1, Quaternion.identity);
            bullet1.GetComponent<Rigidbody2D>().AddForce(dir1.normalized * fBulletSpeed, ForceMode2D.Impulse);

            GameObject bullet2 = Instantiate(missiles[1], mainCannon.position + pos2, Quaternion.identity);
            bullet2.GetComponent<Rigidbody2D>().AddForce(dir2.normalized * fBulletSpeed, ForceMode2D.Impulse);

            GameObject bullet3 = Instantiate(missiles[1], mainCannon.position + pos3, Quaternion.identity);
            bullet3.GetComponent<Rigidbody2D>().AddForce(dir3.normalized * fBulletSpeed, ForceMode2D.Impulse);

            GameObject bullet4 = Instantiate(missiles[1], mainCannon.position + pos4, Quaternion.identity);
            bullet4.GetComponent<Rigidbody2D>().AddForce(dir4.normalized * fBulletSpeed, ForceMode2D.Impulse);

            yield return new WaitForSeconds(1.2f);
            backCannonCoroutine3 = null;
        }

    }

    IEnumerator MainCannonFire2()
    {
        while (true)
        {
            Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
            if (playerPos == null)
            {
                Debug.LogError("Player not found!");
                yield break;
            }
            // 보스 위치에서 플레이어 기체 위치로의 방향을 정규화 하여 백터로 저장
            Vector3 direction = (playerPos.position - transform.position).normalized;
            // 4방위를 벡터로 저장.
            Vector3 up = new Vector3(0f, 1f, 0f);
            Vector3 down = new Vector3(0f, -1f, 0f);
            Vector3 left = new Vector3(-1f, 0f, 0f);
            Vector3 right = new Vector3(1f, 0f, 0f);

            // 8방향으로 발사하는 탄 생성
            GameObject bullet1 = Instantiate(missiles[3], mainCannon.position + up * 0.5f, Quaternion.identity);
            bullet1.GetComponent<Rigidbody2D>().AddForce((direction + up).normalized * fBulletSpeed, ForceMode2D.Impulse);

            GameObject bullet2 = Instantiate(missiles[3], mainCannon.position + down * 0.5f, Quaternion.identity);
            bullet2.GetComponent<Rigidbody2D>().AddForce((direction + down).normalized * fBulletSpeed, ForceMode2D.Impulse);

            GameObject bullet3 = Instantiate(missiles[3], mainCannon.position + left * 0.5f, Quaternion.identity);
            bullet3.GetComponent<Rigidbody2D>().AddForce((direction + left).normalized * fBulletSpeed, ForceMode2D.Impulse);

            GameObject bullet4 = Instantiate(missiles[3], mainCannon.position + right * 0.5f, Quaternion.identity);
            bullet4.GetComponent<Rigidbody2D>().AddForce((direction + right).normalized * fBulletSpeed, ForceMode2D.Impulse);

            GameObject bullet5 = Instantiate(missiles[3], mainCannon.position + (up + right) * 0.5f, Quaternion.identity);
            bullet5.GetComponent<Rigidbody2D>().AddForce((direction + up + right).normalized * fBulletSpeed, ForceMode2D.Impulse);

            GameObject bullet6 = Instantiate(missiles[3], mainCannon.position + (up + left) * 0.5f, Quaternion.identity);
            bullet6.GetComponent<Rigidbody2D>().AddForce((direction + up + left).normalized * fBulletSpeed, ForceMode2D.Impulse);

            GameObject bullet7 = Instantiate(missiles[3], mainCannon.position + (down + right) * 0.5f, Quaternion.identity);
            bullet7.GetComponent<Rigidbody2D>().AddForce((direction + down + right).normalized * fBulletSpeed, ForceMode2D.Impulse);

            GameObject bullet8 = Instantiate(missiles[3], mainCannon.position + (down + left) * 0.5f, Quaternion.identity);
            bullet8.GetComponent<Rigidbody2D>().AddForce((direction + down + left).normalized * fBulletSpeed, ForceMode2D.Impulse);

            yield return new WaitForSeconds(2.1f);
            backCannonCoroutine4 = null;
        }
    }

    #endregion


    #region CircleFire

    IEnumerator CircleFire()
    {
        while (true)
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
                GameObject bullet = Instantiate(missiles[2], mainCannon.transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            }
            yield return new WaitForSecondsRealtime(2.1f);
            backCannonCoroutine2 = null;
        }
    }
    #endregion


    #region BackCannon
    IEnumerator BackCannonFire()
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
            StartCoroutine(BackCannonRotate(target));

            // 미사일 발사
            GameObject obj = Instantiate(missiles[0]);
            obj.transform.position = BackCannon.position;

            // 미사일 방향 설정
            Vector3 direction = target.transform.position - BackCannon.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.AngleAxis(angle + 180f, Vector3.forward); // 180도 회전 추가
            obj.transform.rotation = rotation;

            // 미사일 발사
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.rotation = obj.transform.rotation.eulerAngles.z;
            rb.AddForce(-obj.transform.up * 4f, ForceMode2D.Impulse);

            yield return new WaitForSeconds(1.8f);
            backCannonCoroutine = null;

        }


    }
    IEnumerator BackCannonRotate(GameObject target)
    {
        while (true)
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
            BackCannonPrefab.transform.rotation = Quaternion.RotateTowards(BackCannonPrefab.transform.rotation, q, 180f);

            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

}
