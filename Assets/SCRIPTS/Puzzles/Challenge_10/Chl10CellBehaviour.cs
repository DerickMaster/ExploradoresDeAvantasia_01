using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Challenge_10
{
    public class Chl10CellBehaviour : MonoBehaviour
    {
        [SerializeField] int value;
        TMPro.TextMeshPro tmp;

        private void Start()
        {
            tmp = GetComponentInChildren<TMPro.TextMeshPro>();
            tmp.text = value.ToString();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<FollowPlayerBehaviour>())
            {

            }
        }
    }
}