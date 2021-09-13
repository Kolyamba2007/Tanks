using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    private TankControls controls;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float reload = 1f;
    private bool IsShooting = false;

    private Vector2 DirectionVector;

    [SerializeField]
    private Object projectile;

    Rigidbody2D rb;


    private void Awake()
    {
        controls = new TankControls();

        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        controls.Tank.Enable();

        controls.Tank.Shooting.started += (x) => StartCoroutine(Shooting());

        controls.Tank.Movement.started += ChangingDirection;
        controls.Tank.Movement.canceled += (x) => DirectionVector = Vector2.zero;
    }

    private void OnDisable()
    {
        controls.Tank.Shooting.started -= (x) => StartCoroutine(Shooting());

        controls.Tank.Movement.started -= ChangingDirection;
        controls.Tank.Movement.canceled -= (x) => DirectionVector = Vector2.zero;

        controls.Tank.Disable();
    }

    void Update()
    {
        rb.velocity = DirectionVector * speed;
    }

    private void ChangingDirection(CallbackContext contex)
    {
        switch(controls.Tank.Movement.activeControl.name)
        {
            case "w":
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                DirectionVector = Vector2.up;
                break;
            case "s":
                transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                DirectionVector = Vector2.down;
                break;
            case "a":
                transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                DirectionVector = Vector2.left;
                break;
            case "d":
                transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
                DirectionVector = Vector2.right;
                break;
        }
    }

    private IEnumerator Shooting()
    {
        if (!IsShooting)
        {
            IsShooting = true;
            while (controls.Tank.Shooting.activeControl != null)
            {
                Instantiate(projectile, transform.position, transform.rotation);
                yield return new WaitForSeconds(reload);
            }
            IsShooting = false;
            yield break;
        }
        else
            yield break;
    }
}
