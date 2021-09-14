using System.Collections;
using UnityEngine;

public abstract class BaseTank : MonoBehaviour
{
    [Header("Tank Options")]
    [SerializeField, Min(0)]
    private float _speed = 2f;
    [SerializeField, Min(0)]
    private float _reload = 1f;
    [SerializeField]
    private Object _projectile;

    private bool IsShooting = false;

    private Vector2 DirectionVector { set; get; } = Vector2.up;   
    private Rigidbody2D Rigidbody { set; get; }

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }
    protected virtual void OnEnable()
    {

    }

    protected enum Direction { Up, Down, Left, Right }
    protected void ChangeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                DirectionVector = Vector2.up;
                break;
            case Direction.Down:
                transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                DirectionVector = Vector2.down;
                break;
            case Direction.Left:
                transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                DirectionVector = Vector2.left;
                break;
            case Direction.Right:
                transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
                DirectionVector = Vector2.right;
                break;
        }
        Rigidbody.velocity = DirectionVector * _speed;
    }
    protected void ResetVelocity() => Rigidbody.velocity = Vector2.zero;
    protected void Shoot()
    {
        StartCoroutine(Shooting());
    }

    private IEnumerator Shooting()
    {
        if (!IsShooting)
        {
            IsShooting = true;
            //while (controls.Tank.Shooting.activeControl != null)
            //{
            //    var size = GetComponent<Collider2D>().bounds.size.x;
            //    Instantiate(_projectile, (Vector2)transform.position + DirectionVector, transform.rotation);
            //    yield return new WaitForSeconds(_reload);
            //}
            IsShooting = false;
            yield break;
        }
        else
            yield break;
    } 
}
