using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class testcoup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Dot(transform.up, Vector3.down) > 0)
        {
            Debug.Log(        $"               11            {Vector3.Dot(transform.up, Vector3.down)}");
        }
        else
        {
            Debug.Log(         $"        22                  {Vector3.Dot(transform.up, Vector3.down)}");

        }
    }
}
