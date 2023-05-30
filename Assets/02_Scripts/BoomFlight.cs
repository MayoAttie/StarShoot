using UnityEngine;

public class BoomFlight : Flight
{

    private void Awake()
    {
        hp = 10000;
        maxHp = 10000;
        fSpeed = 2.5f;
        fFireInterval = 0.9f;
    }

    private float bulletSpeed = 10f; // 탄속
    private int bulletCount = 20; // 탄막 개수
    private float fireRate = 0.3f; // 발사 간격

    private float angle; // 원주 위의 각도
    private float time;



    protected override void Update()
    {
        base.Update();
        transform.Translate(Vector3.up * fSpeed * Time.deltaTime);

        // 발사 간격이 되면 탄막 발사
        time += Time.deltaTime;
        if (time > fireRate)
        {
            time = 0f;
            Shoot();
        }
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 게임 오브젝트의 태그가 "Missile"인 경우에만 체력 감소
        if (collision.gameObject.CompareTag("Missile"))
        {
            // 미사일의 데미지 값을 가져와 체력 감소
            int damage = collision.gameObject.GetComponent<Missile>().damage;
            hp -= damage;

            // 체력이 0 이하인 경우에는 파괴 처리
            if (hp <= 0)
            {
                Destroy(gameObject);
                GameManager.Instance.isGameEnd = true;
            }

            // 미사일도 파괴 처리
            Destroy(collision.gameObject);
        }
        
    }



    void Shoot()
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
    }




}
