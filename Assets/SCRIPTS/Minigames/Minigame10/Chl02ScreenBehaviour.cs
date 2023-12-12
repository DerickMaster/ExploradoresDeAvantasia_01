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

        public void SetExpectedWord(Material mat, string word, int initialId)
        {
            _animator.SetBool("Active", true);
            Material[] mats = render.materials;
            mats[1] = mat;
            render.materials = mats;
            if(initialId == 0)
            {
                textMesh.text = "__" + word.Substring(2,2);
            }
            else
            {
                textMesh.text = word.Substring(0, 2) + "__";
            }
            
        }

        public void CompleteWord(string completeWord)
        {
            _animator.SetBool("Active", false);

            textMesh.text = completeWord;
        }
    }

}
