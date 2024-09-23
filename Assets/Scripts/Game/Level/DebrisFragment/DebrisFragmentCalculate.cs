using System.Linq;
using UnityEngine;

public class DebrisFragmentCalculate
{
    private readonly float _defaultSize = 0.5f;
    private readonly Transform _fragmentTransform;
    
    public Collider2D FragmentCollider2D { get; private set; }
    public float SizeFragment { get; private set; }
    public DebrisFragmentCalculate(Transform transform)
    {
        _fragmentTransform = transform;
    }
    public TypeCollider GetSizeFragmentAndSetCollider()
    {
        TypeCollider typeCollider = TypeCollider.Other;
        if (_fragmentTransform.TryGetComponent(out PolygonCollider2D polygonCollider2D))
        {
            typeCollider = TypeCollider.Polygon;
            CalculateSizePolygonCollider(polygonCollider2D);
            SetCollider(polygonCollider2D);
        }
        else if(_fragmentTransform.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            typeCollider = TypeCollider.Box;
            CalculateSizeBoxCollider(boxCollider2D);
            SetCollider(boxCollider2D);
        }
        else if(_fragmentTransform.TryGetComponent(out CapsuleCollider2D capsuleCollider2D))
        {
            typeCollider = TypeCollider.Capsule;
            CalculateSizeCapsuleCollider(capsuleCollider2D);
            SetCollider(capsuleCollider2D);
        }
        else if(_fragmentTransform.TryGetComponent(out CircleCollider2D circleCollider2D))
        {
            typeCollider = TypeCollider.Circle;
            CalculateSizeCircleCollider(circleCollider2D);
            SetCollider(circleCollider2D);
        }
        else
        {
            typeCollider = TypeCollider.Other;
            SizeFragment = _defaultSize;
            SetCollider();
        }

        return typeCollider;
    }
    private void CalculateSizePolygonCollider(PolygonCollider2D polygonCollider2D)
    {
        Vector2[] pathPoints = polygonCollider2D.GetPath(0);
        float maxY = pathPoints.OrderByDescending(p => p.y).FirstOrDefault().y;
        float maxX = pathPoints.OrderByDescending(p => p.x).FirstOrDefault().x;

        float minY = pathPoints.OrderBy(p => p.y).FirstOrDefault().y;
        float minX = pathPoints.OrderBy(p => p.x).FirstOrDefault().x;
        SizeFragment = Vector2.Distance(new Vector2(maxX, maxY), new Vector2(minX, minY));
    }
    private void CalculateSizeBoxCollider(BoxCollider2D boxCollider2D)
    {
        SizeFragment = Mathf.Sqrt(boxCollider2D.size.x * boxCollider2D.size.x + boxCollider2D.size.y * boxCollider2D.size.y);
    }
    private void CalculateSizeCapsuleCollider(CapsuleCollider2D capsuleCollider2D)
    {
        float height = capsuleCollider2D.size.y - capsuleCollider2D.size.x;
        SizeFragment = Mathf.Sqrt(capsuleCollider2D.size.x * capsuleCollider2D.size.x + height * height);
    }
    private void CalculateSizeCircleCollider(CircleCollider2D circleCollider2D)
    {
        SizeFragment = circleCollider2D.radius;
    }
    private void SetCollider(Collider2D collider2D = null)
    {
        if (collider2D != null)
        {
            FragmentCollider2D = collider2D;
        }
        else
        {
            FragmentCollider2D = _fragmentTransform.GetComponent<Collider2D>();
        }
    }
}