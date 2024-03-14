using UnityEngine;

public class GroundAnalyzer
{
    private readonly LayerMask _ground;
    private readonly CarWheel _frontWheel;
    private readonly CarWheel _backWheel;
    public GroundAnalyzer(CarWheel frontWheel, CarWheel backWheel, LayerMask ground)
    {
        _backWheel = backWheel;
        _frontWheel = frontWheel;
        _ground = ground;
    }
    public bool CheckGroundContact()
    {
        if (CheckCircle(_backWheel))
        {
            return true;
        }
        else if (CheckCircle(_frontWheel))
        {
            return true;
        }
        else return false;
    }
    private bool CheckCircle(CarWheel wheel)
    {
        if (Physics2D.OverlapCircle(wheel.Position, wheel.Radius, _ground.value))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}