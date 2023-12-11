using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Challenge_10
{
    public class Challenge10_2Manager : MonoBehaviour
    {
        [System.Serializable]
        struct WordImagePair
        {
            public string word;
            public Material material;
        }

        [System.Serializable]
        struct Case
        {
            public WordImagePair[] pairs;
        }

        [SerializeField] Case[] cases;
        HeavyButtonBehaviour[] buttons;
        Case curCase;
        int curPair;

        private void Start()
        {
            buttons = GetComponentsInChildren<HeavyButtonBehaviour>();
        }

        private void SetCase(Case newCase)
        {
            curCase = newCase;
            curPair = 0;
            SetMaterial(curCase.pairs[curPair].material);
        }

        private void SetMaterial(Material mat)
        {
            // set material on the screen
        }

        private void CheckSyllab(string syllab)
        {
            if (syllab.Equals(curCase.pairs[curPair].word.Substring(2)))
            {
                CheckNextPair();
            }
        }

        private void CheckNextPair()
        {
            curPair++;
            if(curPair >= curCase.pairs.Length)
            {
                Victory();
            }
            else SetMaterial(curCase.pairs[curPair].material);
        }

        private void Victory()
        {

        }
    }
}

