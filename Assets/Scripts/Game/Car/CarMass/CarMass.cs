using NaughtyAttributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class CarMass : MonoBehaviour
{
    [SerializeField, BoxGroup("Bumpers"), HorizontalLine(color:EColor.Yellow)] private Transform _frontBumperStandart;
    [SerializeField, BoxGroup("Bumpers")] private Transform _backBumperStandart;
    [SerializeField, BoxGroup("Bumpers")] private int _bumperStandartMass;
    [SerializeField, BoxGroup("Bumpers")] private Transform _frontBumperArmored;
    [SerializeField, BoxGroup("Bumpers")] private Transform _backBumperArmored;
    [SerializeField, BoxGroup("Bumpers")] private int _bumperArmoredMass;

    [SerializeField, BoxGroup("Doors"), HorizontalLine(color:EColor.Pink)] private Transform _frontDoorStandart;
    [SerializeField, BoxGroup("Doors")] private Transform _backDoorStandart;
    [SerializeField, BoxGroup("Doors")] private int _doorStandartMass;
    [SerializeField, BoxGroup("Doors")] private Transform _frontDoorArmored;
    [SerializeField, BoxGroup("Doors")] private Transform _backDoorArmored;
    [SerializeField, BoxGroup("Doors")] private int _doorArmoredMass;

    [SerializeField, BoxGroup("Frame"), HorizontalLine(color:EColor.Indigo)] private Transform _frontFrame;
    [SerializeField, BoxGroup("Frame")] private Transform _backFrame;
    [SerializeField, BoxGroup("Frame")] private Transform _bottomFrame;
    [SerializeField, BoxGroup("Frame")] private Transform _roofFrame;
    [SerializeField, BoxGroup("Frame")] private int _framePartMass;

    [SerializeField, BoxGroup("Booster"), HorizontalLine(color:EColor.Orange)] private int _boosterMass;
    [SerializeField, BoxGroup("Gun"), HorizontalLine(color:EColor.White)] private int _gunMass;
    [SerializeField, BoxGroup("HotWheel"), HorizontalLine(color:EColor.Blue)] private int _hotWheelMass;

    [SerializeField, BoxGroup("Wheels"), HorizontalLine(color:EColor.Violet)] private Transform _wheelStandart;
    [SerializeField, BoxGroup("Wheels")] private int _wheelStandartMass;
    [SerializeField, BoxGroup("Wheels")] private Transform _wheelMiddle;
    [SerializeField, BoxGroup("Wheels")] private int _wheelMiddleMass;
    [SerializeField, BoxGroup("Wheels")] private Transform _wheelMax;
    [SerializeField, BoxGroup("Wheels")] private int _wheelMaxMass;

    [SerializeField, BoxGroup("Corpus"), HorizontalLine(color:EColor.Black)] private int _corpusMass;
    private Transform _boosterTransform;
    private Transform _gunTransform;
    private Transform _hotWheel;
    private BoosterFuelTank _boosterFuelTank;
    private FuelTank _fuelTank;
    private float _mass;
    [ShowNativeProperty] public float TotalMass => Application.isPlaying ? Mass : 0f;
    [ShowNativeProperty] public float MassWithoutFuel => _mass;
    private int _halfCorpusMass => _corpusMass / 2;
    public float Mass
    {
        get
        {
            if (_boosterFuelTank == null)
            {
                return _mass + _fuelTank.FuelQuantity;
            }
            else
            {
                return _mass + _boosterFuelTank.FuelQuantity + _fuelTank.FuelQuantity;
            }
        }
    }

    public void Construct(Transform boosterTransform, Transform gunTransform, Transform hotWheel, Booster booster, FuelTank fuelTank)
    {
        _boosterTransform = boosterTransform;
        _gunTransform = gunTransform;
        _hotWheel = hotWheel;
        if (booster != null)
        {
            _boosterFuelTank = booster.BoosterFuelTank;
        }
        _fuelTank = fuelTank;
        CalculateMassAndSubscribe();
    }

    private void CalculateMassAndSubscribe()
    {
        CalculateBumperMassAndSubscribeToChange();
        CalculateDoorsMassAndSubscribeToChange();
        CalculateFrameMassAndSubscribeToChange();
        CalculateBoosterMassAndSubscribeToChange();
        CalculateGunMassAndSubscribeToChange();
        CalculateHotWheelMassAndSubscribeToChange();
        CalculateWheelsMass();
        _mass += _corpusMass;
    }
    #region CalculateMass
    private void CalculateBumperMassAndSubscribeToChange()
    {
        if (CheckActivePart(_frontBumperArmored))
        {
            AddMass(_bumperArmoredMass);
            SubscribeToChange(_frontBumperArmored, _bumperArmoredMass);
        }
        else
        {
            AddMass(_bumperStandartMass);
            SubscribeToChange(_frontBumperStandart, _bumperStandartMass);

        }
        if (CheckActivePart(_backBumperArmored))
        {
            AddMass(_bumperArmoredMass);
            SubscribeToChange(_backBumperArmored, _bumperArmoredMass);
        }
        else
        {
            AddMass(_bumperStandartMass);
            SubscribeToChange(_backBumperStandart, _bumperStandartMass);
        }
    }
    private void CalculateDoorsMassAndSubscribeToChange()
    {
        if (CheckActivePart(_frontDoorArmored))
        {
            AddMass(_doorArmoredMass);
            SubscribeToChange(_frontDoorArmored, _doorArmoredMass);
        }
        else
        {
            AddMass(_doorStandartMass);
            SubscribeToChange(_frontDoorStandart, _doorStandartMass);

        }
        if (CheckActivePart(_backDoorArmored))
        {
            AddMass(_doorArmoredMass);
            SubscribeToChange(_backDoorArmored, _doorArmoredMass);

        }
        else
        {
            AddMass(_doorStandartMass);
            SubscribeToChange(_backDoorStandart, _doorStandartMass);

        }
    }
    private void CalculateFrameMassAndSubscribeToChange()
    {
        if (CheckActivePart(_frontFrame))
        {
            AddMass(_framePartMass);
            SubscribeToChange(_frontFrame, _framePartMass);
        }
        if (CheckActivePart(_backFrame))
        {
            AddMass(_framePartMass);
            SubscribeToChange(_backFrame, _framePartMass);
        }
        if (CheckActivePart(_bottomFrame))
        {
            AddMass(_framePartMass);
            SubscribeToChange(_bottomFrame, _framePartMass);
        }
        if (CheckActivePart(_roofFrame))
        {
            AddMass(_framePartMass);
            SubscribeToChange(_roofFrame, _framePartMass);
        }
    }
    private void CalculateBoosterMassAndSubscribeToChange()
    {
        if (CheckActivePart(_boosterTransform))
        {
            AddMass(_boosterMass);
            SubscribeToChange(_boosterTransform, _boosterMass);
        }
    }
    private void CalculateGunMassAndSubscribeToChange()
    {
        if (CheckActivePart(_gunTransform))
        {
            AddMass(_gunMass);
            SubscribeToChange(_gunTransform, _gunMass);
        }
    }
    private void CalculateHotWheelMassAndSubscribeToChange()
    {
        if (CheckActivePart(_hotWheel))
        {
            AddMass(_hotWheelMass);
            SubscribeToChange(_hotWheel, _hotWheelMass);
        }
    }
    private void CalculateWheelsMass()
    {
        if (CheckActivePart(_wheelStandart))
        {
            AddMass(_wheelStandartMass * 2);
        }
        else if (CheckActivePart(_wheelMiddle))
        {
            AddMass(_wheelMiddleMass * 2);
        }
        else
        {
            AddMass(_wheelMaxMass * 2);
        }
    }
    #endregion

    public void ChangeMassOnCarBrokenIntoTwoParts()
    {
        if (CheckActivePart(_wheelStandart))
        {
            SubtractMass(_wheelStandartMass);
        }
        else if (CheckActivePart(_wheelMiddle))
        {
            SubtractMass(_wheelMiddleMass);
        }
        else
        {
            SubtractMass(_wheelMaxMass);
        }

        SubtractMass(_halfCorpusMass);
    }
    private void AddMass(int value)
    {
        _mass += value;
    }

    private void SubtractMass(int value)
    {
        _mass -= value;
    }
    private bool CheckActivePart(Transform part)
    {
        if (part.gameObject.activeSelf == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SubscribeToChange(Transform part, int mass)
    {
        part.gameObject.OnTransformParentChangedAsObservable()
            .Subscribe(_ =>
        {
            SubtractMass(mass);
        }).AddTo(part);
    }
}