using System;
using System.Collections;
using UnityEngine;

namespace Tanks.Units
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [DisallowMultipleComponent]
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

        public bool Dead => _health == 0;
        public bool Invulnerable { private set; get; }
        public byte Health => _health;
        protected bool CanShoot { private set; get; } = true;

        private Renderer Renderer { set; get; }
        private Vector2 DirectionVector { set; get; } = Vector2.up;
        protected Rigidbody2D Rigidbody { private set; get; }
        protected DirectionType Direction { private set; get; }

        public event Action RecievedDamage;
        public event Action Died;

        protected virtual void Awake()
        {
            Rigidbody = this.FindComponent<Rigidbody2D>();
            Renderer = this.FindComponent<Renderer>();
        }
        protected virtual void OnEnable()
        {

        }
        protected virtual void Disable()
        {
            StopAllCoroutines();
            CanShoot = false;
            ResetVelocity();
            GetComponent<Collider2D>().enabled = false;
            Rigidbody.isKinematic = true;
            Rigidbody.simulated = false;
        }

        #region API
        public bool Hit()
        {
            if (Invulnerable) return false;

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
                Destroy(gameObject);
            }
            return true;
        }
        public void SetInvunlerable(float time)
        {
            if (Invulnerable) return;

            StopCoroutine("InvulnerableCoroutine");
            StopCoroutine("PulseCoroutine");
            StartCoroutine(InvulnerableCoroutine(time));
            StartCoroutine(PulseCoroutine());
        }
        private IEnumerator InvulnerableCoroutine(float time)
        {
            Invulnerable = true;
            yield return new WaitForSeconds(time);
            Invulnerable = false;
        }
        private IEnumerator PulseCoroutine()
        {
            Color startColor = Renderer.material.color;
            while (Invulnerable)
            {
                Renderer.material.color = Color.red;
                yield return new WaitForSeconds(1f);
                Renderer.material.color = startColor;
            }
        }
        #endregion

        protected enum DirectionType { Up, Down, Left, Right, Zero }
        protected void ChangeDirection(DirectionType direction)
        {
            if (Dead || this == null) return;
            switch (direction)
            {
                case DirectionType.Up:
                    transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                    DirectionVector = Vector2.up;
                    break;
                case DirectionType.Down:
                    transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                    DirectionVector = Vector2.down;
                    break;
                case DirectionType.Left:
                    transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                    DirectionVector = Vector2.left;
                    break;
                case DirectionType.Right:
                    transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
                    DirectionVector = Vector2.right;
                    break;
                case DirectionType.Zero:
                    Rigidbody.velocity = Vector2.zero;
                    return;
            }
            Direction = direction;
            Rigidbody.velocity = DirectionVector * _speed;
        }
        protected void ResetVelocity() => Rigidbody.velocity = Vector2.zero;
        protected void Shoot()
        {
            if (Dead) return;

            float size = GetComponent<Collider2D>().bounds.size.x * 0.7f;
            var projectile = Instantiate(_projectile, (Vector2)transform.position + DirectionVector * size, transform.rotation);
            bool isPlayer = this is PlayerController;
            (projectile as GameObject).GetComponent<Projectile>().SetOwner(isPlayer ? Projectile.Owner.Player : Projectile.Owner.Enemy);
            StartCoroutine(Reload());
        }        

        private IEnumerator Reload()
        {
            CanShoot = false;
            yield return new WaitForSeconds(_reload);
            CanShoot = true;
            yield break;
        }
    }
}