using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Av2BossStart : MonoBehaviour
{
    BossAdv02Behaviour _bossAv2;
    void Start()
    {
        _bossAv2 = GameObject.FindObjectOfType<BossAdv02Behaviour>();
        _bossAv2.StartFight();
    }
}
