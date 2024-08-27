using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct JSONpy{
    string ID;
    public Vector3 position;
    public string rotation;
    public (string direction, string objectLabel)[] resultTuple;
    public JSONpy(string _ID,Vector3 _position, string _rotation, (string, string)[] _resultTuple){
        ID = _ID;
        position = _position;
        rotation = _rotation;
        resultTuple = _resultTuple;
    }
}
