using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPlacer : MonoBehaviour
{
    public void Init(Vector3 _position, Quaternion _rotation)
    {
        transform.position = _position;
        transform.rotation = _rotation;
    }
}
