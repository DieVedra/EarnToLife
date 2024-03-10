using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataProvider", menuName = "Providers/PlayerDataProvider", order = 51)]
public class PlayerDataProvider : ScriptableObject
{
    public PlayerDataHandler PlayerDataHandler{ get; set;}
}
