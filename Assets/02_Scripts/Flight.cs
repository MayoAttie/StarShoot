using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour
{
    [SerializeField]
    protected int hp;
    protected int maxHp;
    public GameObject[] missiles;

    [SerializeField]
    protected GameObject DestroyEffect;
    [SerializeField]
    protected GameObject BulletCrushEffect;

    protected float fSpeed;
    protected int nFireLevel;
    protected bool isGameEnd;
    protected float fFireInterval;

    [SerializeField]
    protected Transform mainCannon;

    protected Flight(int hp, float fSpeed, float fFireInterval)
    {
        this.hp = hp;
        this.maxHp = hp;
        this.fSpeed = fSpeed;
        this.fFireInterval = fFireInterval;
    }

    protected Flight() 
    { 
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.isGameEnd)
            Destroy(this.gameObject);
        // 체력이 0 이하인 경우에는 파괴 처리
        if (hp <= 0)
        {
            // GameManager의 PlusKillCount 함수를 호출하여 킬 수 증가
            GameManager.Instance.plusKillCount(1);
            SoundManager._instance.PlayEffect(SoundManager.eEffect.Destroy, 0.8f);
            // 파괴 이펙트 생성
            GameObject destroyEffect = Instantiate(DestroyEffect, transform.position, Quaternion.identity);

            // 일정 시간 뒤에 파괴 이펙트 삭제
            Destroy(destroyEffect, 2.0f);

            // 기체 삭제
            Destroy(gameObject);
        }

        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < -0.5f || screenPos.x > 1.5f || screenPos.y < -0.1f || screenPos.y > 1.5f)
        {
            Destroy(gameObject);
        }
    }

    protected void MainAttackStart(bool isUpDown)
    {

        StartCoroutine(MainAttack(isUpDown));
    }
    IEnumerator MainAttack(bool isUpDown)
    {

        while (!isGameEnd)
        {
            GameObject obj = Instantiate(missiles[nFireLevel]);
            obj.transform.position = mainCannon.transform.position;
            Rigidbody2D rb = obj.transform.GetComponent<Rigidbody2D>();

            if (isUpDown) // Bottom -> Up 방식
            {
                SoundManager._instance.PlayEffect(SoundManager.eEffect.Fire, 0.5f);
                rb.AddForce(mainCannon.up * 5f, ForceMode2D.Impulse);
            }
            else // Up -> Bottom 방식
            {
                rb.AddForce(-mainCannon.up * 5f, ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(fFireInterval);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 게임 오브젝트의 태그가 "Missile"인 경우에만 체력 감소
        if (collision.gameObject.CompareTag("MyMissile"))
        {
            // 미사일의 데미지 값을 가져와 체력 감소
            int damage = collision.gameObject.GetComponent<Missile>().damage;
            hp -= damage;
            GameManager.Instance.PlusScore(damage);

            SoundManager._instance.PlayEffect(SoundManager.eEffect.Hit, 0.7f);

            GameObject bulletCrushEffect = Instantiate(BulletCrushEffect, transform.position, Quaternion.identity);

            Destroy(bulletCrushEffect, 2.0f);

            // 미사일도 파괴 처리
            Destroy(collision.gameObject);
        }
    }


    public void setHp(int hp)
    {
        this.hp = hp;
        this.maxHp= hp;
    }
    public void setSpeed(float speed)
    {
        this.fSpeed= speed;
    }
    public void setFireInterval(float interval)
    {
        this.fFireInterval= interval;
    }
    public float getFireInterval()
    {
        return fFireInterval;
    }
    public int getMaxHp()
    {
        return maxHp;
    }
    public int getHp()
    {
        return hp;
    }
    public int getFireLevel()
    {
        return nFireLevel;
    }
    public void setFireLevel(int level) 
    {
        this.nFireLevel = level;
    }


}
