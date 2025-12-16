public class AlongMovableVelocityRotatableController : Controller
{
    private IDirectionalMovable _movable;
    private IDirectionalRotatable _rotatable;

    public AlongMovableVelocityRotatableController(IDirectionalMovable movable, IDirectionalRotatable rotatable)
    {
        _movable = movable;
        _rotatable = rotatable;
    }

    protected override void UpdateLogic(float deltaTime) => _rotatable.SetRotationDirection(_movable.CurrentHorizontalVelocity);
}