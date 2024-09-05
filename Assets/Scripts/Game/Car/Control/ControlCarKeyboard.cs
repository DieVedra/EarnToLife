public class ControlCarKeyboard : ControlCar
{
    private readonly KeyboardInputMethod _keyboardInputMethod;
    private readonly CarGun _carGun;
    public ControlCarKeyboard(KeyboardInputMethod keyboardInputMethod, Gyroscope gyroscope, IStateSetter stateSetterPropulsionUnit,
        PropulsionUnit propulsionUnit, Booster booster, CarGun carGun, Speedometer speedometer)
        : base(keyboardInputMethod, gyroscope, stateSetterPropulsionUnit, propulsionUnit, booster, speedometer)
    {
        _keyboardInputMethod = keyboardInputMethod;
        _carGun = carGun;
    }

    public override void Update()
    {
        base.Update();
        if (_carGun != null)
        {
            if (_keyboardInputMethod.CheckPressFire() && _carGun.Ammo > 0)
            {
                _carGun.SetShootKey(true);
            }
            else
            {
                _carGun.SetShootKey(false);
            }

            if (_keyboardInputMethod.CheckPressNextTarget())
            {
                _carGun.ChangeIndex();
            }
        }
    }
    public override void TryTurnOffCheckBooster()
    {
        _keyboardInputMethod.TurnOffCheckBooster();
    }
    public override void TryTurnOffCheckGun()
    {
        _keyboardInputMethod.TurnOffCheckGun();
    }
}