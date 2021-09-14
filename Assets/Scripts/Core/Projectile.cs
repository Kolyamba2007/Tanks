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
        GetComponent<Collider2D>().isTrigger = true;
        Destroy(transform.gameObject, _lifetime);
    }
    private void Update()
    {
        transform.Translate(Vector2.up * _speed * Time.deltaTime, Space.Self);
    }

    public void SetOwner(Owner owner) => _owner = owner;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 8)
        {
            if (_owner.ToString() != collider.tag)
            {
                collider.GetComponent<BaseTank>().Hit();
            }
        }
        Destroy(gameObject);
    }
}
