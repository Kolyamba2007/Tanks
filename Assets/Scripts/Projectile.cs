using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float liveTime;
    float speed = 8;

    void Start()
    {
        Destroy(transform.gameObject, liveTime);
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime, Space.Self);
    }
}
