using System;
using Cysharp.Threading.Tasks;

public class ControlCarUI : ControlCar
{
    public readonly UIInputMethod UIInputMethod;
    public ControlCarUI(UIInputMethod uIInputMethod, Gyroscope gyroscope, IStateSetter stateSetterPropulsionUnit,
        PropulsionUnit propulsionUnit, Booster booster, Speedometer speedometer) 
        : base(uIInputMethod, gyroscope, stateSetterPropulsionUnit, propulsionUnit, booster, speedometer)
    {
        UIInputMethod = uIInputMethod;
    }
    
}