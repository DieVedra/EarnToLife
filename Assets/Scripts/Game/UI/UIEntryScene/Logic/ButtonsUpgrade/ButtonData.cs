using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct ButtonData
{
    public Image Icon { get; }
    public string Name { get; }
    public string Description { get; }
    public ButtonData(Image icon, string name, string description)
    {
        Icon = icon;
        Name = name;
        Description = description;
    }
}
