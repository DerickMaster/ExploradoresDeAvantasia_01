using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Challenge_10
{
    public class Chl02ScreenBehaviour : MonoBehaviour
    {
        Animator _animator;
        TextMeshPro[] textMeshes;
        SkinnedMeshRenderer render;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            textMeshes = GetComponentsInChildren<TextMeshPro>();
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
                textMeshes[0].text = "__";
                textMeshes[1].text = word.Substring(2, 2);
            }
            else
            {
                textMeshes[0].text = word.Substring(0, 2);
                textMeshes[1].text = "__";
            }
            
        }

        public void CompleteWord(string completeWord)
        {
            _animator.SetBool("Active", false);

            textMeshes[0].text = completeWord.Substring(0, 2);
            textMeshes[1].text = completeWord.Substring(2, 2);
        }
    }

}
