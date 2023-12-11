using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Challenge_10
{
    public class HeavyButtonBehaviour : MonoBehaviour
    {
        TextMeshPro textMesh;

        private void Start()
        {
            textMesh = GetComponentInChildren<TextMeshPro>();
        }

        public void SetText(string text)
        {
            textMesh.text = text;
        }
    }

}