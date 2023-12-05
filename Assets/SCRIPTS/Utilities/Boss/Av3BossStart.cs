using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Av3BossStart : MonoBehaviour
{
    Adventure3BossManager _advBoss;
    void Start()
    {
        _advBoss = GameObject.FindObjectOfType<Adventure3BossManager>();
        _advBoss.StartBossFight();
    }
}
