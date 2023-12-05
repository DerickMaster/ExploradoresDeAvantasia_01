using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LorePageBehaviour : MonoBehaviour
{
    [HideInInspector]public UnityEvent _passPage;

    public void PassPage()
    {
        _passPage.Invoke();
    }
}
