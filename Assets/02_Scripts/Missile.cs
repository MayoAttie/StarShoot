using UnityEngine;

public class Missile : MonoBehaviour
{
    public int damage;


    void Update()
    {
        Vector3 screenPos  =  Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < 0f || screenPos.x > 1f || screenPos.y < -0.5f || screenPos.y > 1.1f)
        {
            Destroy(gameObject);
        }
    }
}
