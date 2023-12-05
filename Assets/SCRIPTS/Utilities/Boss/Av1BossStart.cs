using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Av1BossStart : MonoBehaviour
{
    Adv01_BossFightManager _av1Boss;

    private void Start()
    {
        _av1Boss = GameObject.FindObjectOfType<Adv01_BossFightManager>();
        _av1Boss.StartFight();
    }
}
