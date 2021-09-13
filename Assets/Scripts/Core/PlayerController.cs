using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    private TankControls controls;
    [SerializeField, Range(0, 5)]
    private float speed = 1f;
    [SerializeField, Min(0)]
    private float reload = 1f;

    private void Awake()
    {
        controls = new TankControls();
    }

    private void OnEnable()
    {
        controls.Tank.Enable();

        controls.Tank.Shooting.started += (x) => StartCoroutine(Shooting());
    }
    private void OnDisable()
    {
        controls.Tank.Shooting.started -= (x) => StartCoroutine(Shooting());

        controls.Tank.Disable();
    }

    private void Update()
    {
        var axis = controls.Tank.Movement.ReadValue<Vector2>();

        transform.Translate(new Vector2(axis.x, axis.y) * speed * Time.deltaTime, Space.Self);
    }

    private IEnumerator Shooting()
    {
        while (controls.Tank.Shooting.activeControl != null)
        {
            //Логика создания снаряда
            Debug.Log("Fire");
            yield return new WaitForSeconds(reload);
        }
        yield break;
    }
}
