using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    public GameObject _myObject;
    private void OnEnable()
    {
        _myObject.SetActive(true);
    }
}
