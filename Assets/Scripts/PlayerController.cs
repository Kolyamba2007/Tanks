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

    void Update()
    {
        var axis = controls.Tank.Movement.ReadValue<Vector2>();

        transform.Translate(new Vector2(axis.x, axis.y) * speed * Time.deltaTime, Space.Self);
    }

    private IEnumerator Shooting()
    {
        var activeControl = controls.Tank.Shooting.activeControl;

        while (activeControl != null)
        {
            //Логика создания снаряда
            Debug.Log("Fire");
            yield return new WaitForSeconds(reload);
        }
        yield break;
    }
}
