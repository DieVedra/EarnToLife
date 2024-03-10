using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICorutineStarter
{
    public void EngageCorutine(IEnumerator enumerator);
}
