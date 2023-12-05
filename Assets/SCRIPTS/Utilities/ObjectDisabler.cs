using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisabler : MonoBehaviour
{
    public GameObject[] _objects;

    public void DisableObjects()
    {
        foreach (var _object in _objects)
        {
            _object.SetActive(false);
        }
    }
}
