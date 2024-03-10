using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class test : MonoBehaviour
{
    [SerializeField] private SpriteShapeController _spriteShapeController;

    [SerializeField] private List<Vector3> _positions;

    // [Button("save")]
    public void a(List<Vector3> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            _spriteShapeController.spline.InsertPointAt(i, points[i]);
        }
    }
    // [Button("save")]
    // private void f()
    // {
    //     
    // }
    [Button("reset")]
    private void b()
    {
        _spriteShapeController.spline.Clear();
    }
}
