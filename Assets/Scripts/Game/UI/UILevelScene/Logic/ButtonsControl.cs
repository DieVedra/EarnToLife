using UnityEngine;
public class ButtonsControl
{
    private readonly CustomButton _buttonGas;
    private readonly CustomButton _buttonBoost;
    private readonly CustomButton _buttonStop;
    private readonly CustomButton _buttonRotateClockwise;
    private readonly CustomButton _buttonRotateAntiClockwise;
    private readonly ControlCarUI _controlCarUI;
    public ButtonsControl(ViewUILevel viewUILevel, CarInLevel carInLevel)
    {
        _buttonGas = viewUILevel.ButtonGas;
        _buttonBoost = viewUILevel.ButtonBoost;
        _buttonStop = viewUILevel.ButtonStop;
        _buttonRotateClockwise = viewUILevel.ButtonRotateClockwise;
        _buttonRotateAntiClockwise = viewUILevel.ButtonRotateCounterClockwise;
        _controlCarUI = carInLevel.ControlCarUI;
    }
    public void Activate()
    {
        Subscribe();
    }
    public void Deactivate()
    {
        Unsubscribe(); 
    }
    private void Subscribe()
    {
        _buttonGas.OnButtonDown += GasDown;
        _buttonGas.OnButtonUp += GasUp;

        if (_buttonBoost.interactable == true)
        {
            _buttonBoost.OnButtonDown += BoosterDown;
            _buttonBoost.OnButtonUp += BoosterUp;
        }

        _buttonStop.OnButtonDown += StopDown;
        _buttonStop.OnButtonUp += StopUp;

        _buttonRotateClockwise.OnButtonDown += RotateClockwise;
        _buttonRotateClockwise.OnButtonUp += StopRotate;

        _buttonRotateAntiClockwise.OnButtonDown += RotateCounterClockwise;
        _buttonRotateAntiClockwise.OnButtonUp += StopRotate;
    }
    private void Unsubscribe()
    {
        _buttonGas.OnButtonDown -= GasDown;
        _buttonGas.OnButtonUp -= GasUp;
        _buttonBoost.OnButtonDown -= BoosterDown;
        _buttonBoost.OnButtonUp -= BoosterUp;

        _buttonStop.OnButtonDown -= StopDown;
        _buttonStop.OnButtonUp -= StopUp;

        _buttonRotateClockwise.OnButtonDown -= RotateClockwise;
        _buttonRotateClockwise.OnButtonUp -= StopRotate;
;
        _buttonRotateAntiClockwise.OnButtonDown -= RotateCounterClockwise;
        _buttonRotateAntiClockwise.OnButtonUp -= StopRotate;
    }
    private void GasDown()
    {
        _controlCarUI.UIInputMethod.TurnOnGas();
    }
    private void GasUp()
    {
        _controlCarUI.UIInputMethod.TurnOffGas();
    }
    private void BoosterDown()
    {
        _controlCarUI.UIInputMethod.TurnOnBooster();
    }
    private void BoosterUp()
    {
        _controlCarUI.UIInputMethod.TurnOffBooster();
    }
    private void StopDown()
    {
        _controlCarUI.UIInputMethod.TurnOnStop();
    }
    private void StopUp()
    {
        _controlCarUI.UIInputMethod.TurnOffStop();
    }
    private void RotateClockwise()
    {
        _controlCarUI.UIInputMethod.TurnOnRotateClockwise();
    }
    private void RotateCounterClockwise()
    {
        _controlCarUI.UIInputMethod.TurnOnRotateAntiClockwise();
    }
    private void StopRotate()
    {
        _controlCarUI.UIInputMethod.TurnOffRotation();
    }
}
