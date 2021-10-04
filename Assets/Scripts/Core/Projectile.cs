using Tanks.Units;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public enum Owner { Player, Enemy }

    [SerializeField]
    private float _lifetime;
    [SerializeField]
    private float _speed = 8;
    private Owner _owner;

    private void Start()
    {
        Destroy(transform.gameObject, _lifetime);
    }
    private void Update()
    {
        transform.Translate(Vector2.up * _speed * Time.deltaTime, Space.Self);
    }

    public void SetOwner(Owner owner) => _owner = owner;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (_owner.ToString() != collision.transform.tag)
            {
                collision.gameObject.GetComponent<BaseTank>().Hit();
            }
        }
        Destroy(gameObject);
    }
}
