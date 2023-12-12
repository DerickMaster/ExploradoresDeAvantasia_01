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
            public bool missingEnd;
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
            SetCase(cases[Random.Range(0,cases.Length)]);
        }

        private void SetCase(Case newCase)
        {
            curCase = newCase;
            curPair = 0;
            SetPair();
        }

        private int initialId;
        private void SetPair()
        {
            randomSyllabs = randomSyllabs.Shuffle().ToArray();

            initialId = 0;
            if (curCase.missingEnd)
                initialId = 2;

            screen.SetExpectedWord(curCase.pairs[curPair].material, curCase.pairs[curPair].word, initialId);

            expectedSyllab = curCase.pairs[curPair].word.Substring(initialId, 2);
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
            if (syllab.Equals(expectedSyllab))
            {
                screen.CompleteWord(curCase.pairs[curPair].word);
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

