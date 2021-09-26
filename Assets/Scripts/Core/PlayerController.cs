//using static UnityEngine.InputSystem.InputAction;

using System;

public class PlayerController : BaseTank
{
    private TankControls controls;

    public event Action Fire;
    public event Action Pause;

    protected override void Awake()
    {
        base.Awake();
        controls = new TankControls();
        controls.Tank.Pause.started += _ => Pause?.Invoke();
    }
    private void Update()
    {
        if (Dead) return;

        var isFire = controls.Tank.Shooting.IsPressed();
        if (isFire && CanShoot)
        {
            Shoot();
            Fire?.Invoke();
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        controls.Tank.Enable();

        controls.Tank.Up.started += _ => ChangeDirection(DirectionType.Up);
        controls.Tank.Down.started += _ => ChangeDirection(DirectionType.Down);
        controls.Tank.Left.started += _ => ChangeDirection(DirectionType.Left);
        controls.Tank.Right.started += _ => ChangeDirection(DirectionType.Right);

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
            ChangeDirection(DirectionType.Zero);
    }
}
