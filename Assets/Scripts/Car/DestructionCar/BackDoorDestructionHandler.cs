using UnityEngine;

public class BackDoorDestructionHandler : DestructionHandler
{
    public readonly int StrengthDoor;
    private readonly Transform _doorNormal;
    private readonly Transform _doorDamaged1;
    private readonly Transform _doorDamaged2;
    private readonly Transform[] _parts;
    private bool _isBroken = false;

    public BackDoorDestructionHandler(DoorRef doorRef, DestructionHandlerContent destructionHandlerContent) 
        : base(doorRef, destructionHandlerContent, doorRef.StrengthDoors)
    {
        _doorNormal = doorRef.DoorNormal;
        _doorDamaged1 = doorRef.DoorDamaged1;
        _doorDamaged2 = doorRef.DoorDamaged2;
        StrengthDoor = doorRef.StrengthDoors;
        _parts = new[] {_doorNormal, _doorDamaged1, _doorDamaged2};
    }
    public void DestructionMode1()
    {
        if (_isBroken == false)
        {
            _doorNormal.gameObject.SetActive(false);
            _doorDamaged1.gameObject.SetActive(true);
        }
    }
    public void TryDestructionMode2()
    {
        if (_isBroken == false)
        {
            _doorNormal.gameObject.SetActive(false);
            _doorDamaged1.gameObject.SetActive(false);
            _doorDamaged2.gameObject.SetActive(true);
        }
    }
    // public void DestructionMode3()
    // {
    //     CompositeDisposable.Clear();
    // }

    public void TryThrowDoor()
    {
        if (_isBroken == false)
        {
            _isBroken = true;
            SetParentDebris();
            for (int i = 0; i < _parts.Length; i++)
            {
                if (_parts[i].gameObject.activeSelf == true)
                {
                    _parts[i].gameObject.AddComponent<Rigidbody2D>();
                }
            }
        }
    }
}