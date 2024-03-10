using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyroscope
{
    private Rigidbody2D _rigidbodyBody;
    private float _gyroscopePower;
    public Gyroscope(Rigidbody2D rigidbodyBody, float gyroscopePower)
    {
        _rigidbodyBody = rigidbodyBody;
        _gyroscopePower = gyroscopePower;
    }
    public void Rotation(float multiplierDirection)
    {
        _rigidbodyBody.AddTorque(_gyroscopePower * multiplierDirection * _rigidbodyBody.mass, ForceMode2D.Impulse);
    }
}
