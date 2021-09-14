using static UnityEngine.InputSystem.InputAction;

public class PlayerController : BaseTank
{
    private TankControls controls;

    protected override void Awake()
    {
        base.Awake();
        controls = new TankControls();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        controls.Tank.Enable();
        controls.Tank.Shooting.started += _ => Shoot();
        controls.Tank.Movement.canceled += _ => ResetVelocity();
        controls.Tank.Movement.started += OnChangeDirection;
    }
    protected override void Disable()
    {
        base.Disable();
        controls.Disable();
        controls.Tank.Movement.started -= OnChangeDirection;
    }

    private void OnChangeDirection(CallbackContext context)
    {
        switch (context.control.name)
        {
            case "w":
                ChangeDirection(Direction.Up);
                break;
            case "s":
                ChangeDirection(Direction.Down);
                break;
            case "a":
                ChangeDirection(Direction.Left);
                break;
            case "d":
                ChangeDirection(Direction.Right);
                break;
        }
    }
}
