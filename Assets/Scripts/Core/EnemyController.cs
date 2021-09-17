using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseTank
{
    public float MovementIntervalMin;
    public float MovementIntervalMax;

    [Header("Editor Options")]
    [SerializeField, Tooltip("Shows potential directions for free movement")]
    private bool ShowTracking = true;
    private float checkStayTime = 1.5f;

    private void Start()
    {
        RecievedDamage += OnRecievedDamage;

        ChangeDirection();
    }
    private void LateUpdate()
    {
        if (!Dead && CanShoot) Shoot();

        if (checkStayTime > 0)
        {
            checkStayTime -= Time.deltaTime;
        }
        else
        {
            checkStayTime = 1.5f;
            if (Rigidbody.velocity == Vector2.zero) ChangeDirection();
        }
    }
    private void Update()
    {
        if (ShowTracking)
        {
            Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.left, CanMove(DirectionType.Left) ? Color.green : Color.red);
            Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.right, CanMove(DirectionType.Right) ? Color.green : Color.red);
            Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.up, CanMove(DirectionType.Up) ? Color.green : Color.red);
            Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.down, CanMove(DirectionType.Down) ? Color.green : Color.red);
        }
    }

    private void OnRecievedDamage()
    {
        int length = Enum.GetValues(typeof(DirectionType)).Length;
        ChangeDirection((DirectionType)UnityEngine.Random.Range(0, length));
    }

    private bool CanMove(DirectionType direction)
    {
        Vector2 vector = Vector2.zero;
        switch (direction)
        {
            case DirectionType.Down: vector = Vector2.down; break;
            case DirectionType.Up: vector = Vector2.up; break;
            case DirectionType.Right: vector = Vector2.right; break;
            case DirectionType.Left: vector = Vector2.left; break;
        }

        if (Physics2D.Raycast(transform.position, vector, 1f, 1 << 9))
        {
            return false;
        }
        return true;
    }
    private void ChangeDirection()
    {
        List<DirectionType> array = new List<DirectionType>();
        if (CanMove(DirectionType.Down)) array.Add(DirectionType.Down);
        if (CanMove(DirectionType.Up)) array.Add(DirectionType.Up);
        if (CanMove(DirectionType.Left)) array.Add(DirectionType.Left);
        if (CanMove(DirectionType.Right)) array.Add(DirectionType.Right);

        System.Random random = new System.Random();
        int rand = random.Next(0, array.Count);
        ChangeDirection(array[rand]);
    }
    private IEnumerator MovementInterval()
    {
        float rand = UnityEngine.Random.Range(MovementIntervalMin, MovementIntervalMax);
        yield return new WaitForSeconds(rand);
        ChangeDirection();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeDirection();
    }
}
