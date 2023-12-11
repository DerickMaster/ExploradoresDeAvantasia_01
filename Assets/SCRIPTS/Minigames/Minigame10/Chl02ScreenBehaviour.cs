using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Challenge_10
{
    public class Chl02ScreenBehaviour : MonoBehaviour
    {
        Animator _animator;
        TextMeshPro textMesh;
        SkinnedMeshRenderer render;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            textMesh = GetComponentInChildren<TextMeshPro>();
            render = GetComponentInChildren<SkinnedMeshRenderer>();
        }

        public void SetExpectedWord(Material mat, string word)
        {
            _animator.SetBool("Active", true);
            Material[] mats = render.materials;
            mats[1] = mat;
            render.materials = mats;

            textMesh.text = word.Substring(0, 2);
            textMesh.text += "_ _";
        }

        public void CompleteWord(string syllab)
        {
            _animator.SetBool("Active", false);
            textMesh.text = textMesh.text.Substring(0, 2);
            textMesh.text += syllab;
        }
    }

}
