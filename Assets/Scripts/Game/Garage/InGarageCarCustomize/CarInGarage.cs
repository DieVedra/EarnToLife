using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CarInGarage : Car
{
    public void Cunstruct(CarConfiguration carConfiguration)
    {
        CustomizeCar ??= GetComponent<CustomizeCar>();
        CustomizeCar.OnSetWheels += InitWheelsAndSuspension;
        CustomizeCar.Construct(carConfiguration);
    }
    private void FixedUpdate()
    {
        FrontSuspension?.Calculate();
        BackSuspension?.Calculate();
    }

    private void OnDisable()
    {
        CustomizeCar.OnSetWheels -= InitWheelsAndSuspension;

    }
}
