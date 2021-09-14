﻿using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseTank : MonoBehaviour
{
    [Header("Tank Options")]
    [SerializeField]
    private byte _health = 1;
    [SerializeField, Range(0, 5f)]
    private float _speed = 2f;
    [SerializeField, Min(0)]
    private float _reload = 1f;
    [SerializeField]
    private UnityEngine.Object _projectile;

    private bool CanShoot = true;

    private Vector2 DirectionVector { set; get; } = Vector2.up;   
    private Rigidbody2D Rigidbody { set; get; }

    public event Action RecievedDamage;
    public event Action Died;

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }
    protected virtual void OnEnable()
    {

    }
    protected virtual void Disable()
    {
        CanShoot = false;
        ResetVelocity();
        GetComponent<Collider2D>().enabled = false;
        Rigidbody.isKinematic = true;
    }

    public void Hit()
    {
        if (_health - 1 > 0)
        {
            _health--;
            RecievedDamage?.Invoke();
        }
        else
        {
            _health = 0;
            Disable();
            Died?.Invoke();
        }
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
        if (CanShoot)
        {
            var projectile = Instantiate(_projectile, (Vector2)transform.position + DirectionVector * 0.5f, transform.rotation);
            bool isPlayer = this is PlayerController;
            (projectile as GameObject).GetComponent<Projectile>().SetOwner(isPlayer ? Projectile.Owner.Player : Projectile.Owner.Enemy);
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        CanShoot = false;
        yield return new WaitForSeconds(_reload);
        CanShoot = true;
    }
}