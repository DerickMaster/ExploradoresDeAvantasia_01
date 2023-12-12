using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] string[] randomSyllabs;

        Chl02ScreenBehaviour screen;
        HeavyButtonBehaviour[] buttons;
        Case curCase;
        int curPair;
        string expectedSyllab;

        private void Start()
        {
            screen = GetComponentInChildren<Chl02ScreenBehaviour>();
            buttons = GetComponentsInChildren<HeavyButtonBehaviour>();
            foreach (HeavyButtonBehaviour buttonBehaviour in buttons)
            {
                buttonBehaviour.pressedEvent.AddListener(CheckSyllab);
            }
            SetCase(cases[0]);
        }

        private void SetCase(Case newCase)
        {
            curCase = newCase;
            curPair = 0;
            SetPair();
        }

        private void SetPair()
        {
            randomSyllabs = randomSyllabs.Shuffle().ToArray();
            screen.SetExpectedWord(curCase.pairs[curPair].material, curCase.pairs[curPair].word);
            expectedSyllab = curCase.pairs[curPair].word.Substring(2, 2);
            RandomizeSyllabs();
        }

        private void RandomizeSyllabs()
        {
            int randId = Random.Range(0, buttons.Length);
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i == randId) buttons[i].SetText(expectedSyllab);
                else buttons[i].SetText(randomSyllabs[i]);
            }
        }

        private void CheckSyllab(string syllab)
        {
            if (syllab.Equals(curCase.pairs[curPair].word.Substring(2)))
            {
                screen.CompleteWord(curCase.pairs[curPair].word.Substring(2));
                DeactivateButtons();

                Invoke(nameof(CheckNextPair), 2f);
            }
        }

        private void CheckNextPair()
        {
            curPair++;
            if(curPair >= curCase.pairs.Length)
            {
                Victory();
            }
            else SetPair();
        }

        private void Victory()
        {
            DeactivateButtons();
        }

        private void DeactivateButtons()
        {
            foreach (HeavyButtonBehaviour buttonBehaviour in buttons)
            {
                buttonBehaviour.Deactivate();
            }
        }
    }
}

