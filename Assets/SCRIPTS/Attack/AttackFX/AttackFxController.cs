using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFxController : MonoBehaviour
{

    public void Hide()
    {
        transform.position = new Vector3(0,-99,0);    
    }
}
