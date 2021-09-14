//using static UnityEngine.InputSystem.InputAction;

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

        controls.Tank.Up.started += _ => ChangeDirection(Direction.Up);
        controls.Tank.Down.started += _ => ChangeDirection(Direction.Down);
        controls.Tank.Left.started += _ => ChangeDirection(Direction.Left);
        controls.Tank.Right.started += _ => ChangeDirection(Direction.Right);

        controls.Tank.Up.canceled += _ => ZeroState();
        controls.Tank.Down.canceled += _ => ZeroState();
        controls.Tank.Left.canceled += _ => ZeroState();
        controls.Tank.Right.canceled += _ => ZeroState();
    }
    protected override void Disable()
    {
        base.Disable();
        controls.Tank.Disable();
    }

    private void ZeroState()
    {
        if (!(controls.Tank.Up.inProgress || controls.Tank.Down.inProgress || controls.Tank.Left.inProgress || controls.Tank.Right.inProgress))
            ChangeDirection(Direction.Zero);
    }
}
