using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Av4BossStart : MonoBehaviour
{
    Adventure4BossManager _advBoss;
    void Start()
    {
        _advBoss = GameObject.FindObjectOfType<Adventure4BossManager>();
        _advBoss.StartBossFight();
    }
}
