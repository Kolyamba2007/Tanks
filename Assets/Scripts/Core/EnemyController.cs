using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseTank
{
    public float MovementIntervalMin;
    public float MovementIntervalMax;

    private void Start()
    {
        RecievedDamage += OnRecievedDamage;
    }
    private void LateUpdate()
    {
        if (CanShoot) Shoot();
    }

    private void ChangeDirection()
    {
        List<int> array = new List<int>() { 0, 1, 2, 3 };
        array.Remove((int)Direction);
        int rand = UnityEngine.Random.Range(0, array.Count);
        ChangeDirection((DirectionType)array[rand]);
    }
    private void OnRecievedDamage()
    {
        int length = Enum.GetValues(typeof(DirectionType)).Length;
        ChangeDirection((DirectionType)UnityEngine.Random.Range(0, length));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            var otherVelocity = collision.otherRigidbody.velocity;
            rigidbody.velocity -= otherVelocity;
        }

        ChangeDirection();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        ChangeDirection();
    }
}
