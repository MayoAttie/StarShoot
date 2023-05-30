using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlight : Flight
{
    public EnemyFlight(int hp, float fSpeed, float fFireInterval) : base(hp, fSpeed, fFireInterval) { }

    [SerializeField]
    GameObject[] ItemPrefab;

    void Start()
    {
        MainAttackStart(false);
    }

    protected override void Update()
    {
        base.Update();
        // 적기가 파괴될 때 아이템 생성
        if (hp <= 0)
        {
            // 25% 확률로 아이템 생성
            if (Random.value <= 0.7f)
            {
                // 랜덤으로 아이템 종류 결정
                Item.eITEM_TYPE itemType = (Item.eITEM_TYPE)Random.Range(1, (int)Item.eITEM_TYPE.Max);

                // 아이템 생성
                GameObject item = Instantiate(ItemPrefab[(int)itemType - 1], transform.position, Quaternion.identity);

                // 아이템 타입 설정
                item.GetComponent<Item>().setType(itemType);
            }
        }
        transform.Translate(Vector3.down * fSpeed * Time.deltaTime);

    }

    public void Copy(EnemyFlight other)
    {
        this.hp = other.hp;
        this.maxHp = other.maxHp;
        this.fSpeed = other.fSpeed;
        this.fFireInterval = other.fFireInterval;
    }






}
