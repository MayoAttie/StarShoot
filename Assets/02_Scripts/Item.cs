using UnityEngine;

public class Item : MonoBehaviour
{
    public enum eITEM_TYPE
    {
        None =0,
        HP,
        Power,
        Boom,
        Interval,
        Max
    }

    private float fSpeed;
    eITEM_TYPE eItemType;

    private void Awake()
    {
        fSpeed = 1.5f;
    }

    void Update()
    {
        transform.Translate(Vector3.down * fSpeed * Time.deltaTime);

        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < -0.1f || screenPos.x > 1.1f || screenPos.y < -0.1f || screenPos.y > 1.1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            switch(eItemType)
            {
                case eITEM_TYPE.HP:
                    HP_ItemFunc(collision.gameObject);
                    break;
                case eITEM_TYPE.Power:
                    Power_ItemFunc(collision.gameObject);
                    break;
                case eITEM_TYPE.Boom:
                    Boom_ItemFunc(collision.gameObject);
                    break;
                case eITEM_TYPE.Interval:
                    Interval_ItemFunc(collision.gameObject);
                    break;
                default: break;
            }
            GameManager.Instance.PlusScore(2000);
            Destroy(this.gameObject);

        }
    }


    public void setType(eITEM_TYPE type)
    {
        this.eItemType = type;
    }

    void HP_ItemFunc(GameObject obj)
    {
        FlightManager manager = obj.GetComponent<FlightManager>();
        int maxHp = manager.getMaxHp();
        if(manager.getHp()+10  >= maxHp)
        {
            manager.setHp(maxHp);
            manager.HpBarView();
        }
        else
        {
            manager.RecoveryHp(10);
        }
    }

    void Power_ItemFunc(GameObject obj)
    {
        FlightManager manager = obj.GetComponent<FlightManager>();
        int len = manager.missiles.Length;
        if (manager.getFireLevel() >= len-1)
            return;
        else
        {
            int num = manager.getFireLevel();
            manager.setFireLevel(num+1);
        }
    }

    void Boom_ItemFunc(GameObject obj)
    {
        FlightManager manager = obj.GetComponent<FlightManager>();
        manager.setHaveBoomCount();
    }
    void Interval_ItemFunc(GameObject obj)
    {
        FlightManager manager = obj.GetComponent<FlightManager>();
        float interval = manager.getFireInterval();
        interval = interval - (interval * 0.1f);
        manager.setFireInterval(interval);
    }

    
}
