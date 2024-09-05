using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyroscope
{
    private readonly GroundAnalyzer _groundAnalyzer;
    private readonly Rigidbody2D _rigidbodyBody;
    private readonly CarMass _carMass;
    private readonly float _gyroscopePower;
    public Gyroscope(GroundAnalyzer groundAnalyzer, Rigidbody2D rigidbodyBody, CarMass carMass, float gyroscopePower)
    {
        _groundAnalyzer = groundAnalyzer;
        _rigidbodyBody = rigidbodyBody;
        _gyroscopePower = gyroscopePower;
        _carMass = carMass;
    }
    public void Rotation(float multiplierDirection)
    {
        _rigidbodyBody.AddTorque(_gyroscopePower * multiplierDirection * _carMass.Mass * Time.timeScale, ForceMode2D.Force);
    }
}
