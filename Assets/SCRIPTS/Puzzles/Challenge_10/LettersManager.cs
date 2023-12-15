using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Challenge_10
{
    public class LettersManager : MonoBehaviour
    {
        [SerializeField] private Mesh[] lettersMeshes;
        [SerializeField] int randomAmount;
        [SerializeField] GameObject letterCubeParent;
        [SerializeField] GameObject letterLinePrefab;

        private Minigame08LetterCube[] letterCubes;
        private MeshFilter[] slotsMeshFilters;
        private Dictionary<char, Mesh> lettersMeshDict;
        readonly string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        int lastId;
        int curRandomId = 0;
        int[] randomIds;
        int lettersAmount = 7;

        private void Start()
        {
            lettersMeshDict = new Dictionary<char, Mesh>();
            for (int i = 0; i < lettersMeshes.Length; i++)
            {
                lettersMeshDict.Add(letters[i], lettersMeshes[i]);
            }

            letterCubes = letterCubeParent.GetComponentsInChildren<Minigame08LetterCube>();
            foreach (var letterCube in letterCubes)
            {
                letterCube.refUnitKilled.AddListener(LetterCubeKilled);
            }

            SetCase();
        }

        public void SetLetters(char[] letters)
        {
            for (int i = 0; i < slotsMeshFilters.Length; i++)
            {
                try
                {
                    slotsMeshFilters[i].mesh = lettersMeshDict[letters[i]];
                    slotsMeshFilters[i].gameObject.SetActive(true);
                }
                catch (System.IndexOutOfRangeException)
                {
                    slotsMeshFilters[i].gameObject.SetActive(false);
                }
            }
        }

        private void SetRandomLetters()
        {
            randomIds = Enumerable.Range(1, lettersAmount-1).ToArray();
            randomIds = randomIds.Shuffle().Take(randomAmount).OrderBy(x => x).ToArray();

            for (int i = 0; i < randomAmount; i++)
            {
                slotsMeshFilters[randomIds[i]].gameObject.SetActive(false);
            }
        }

        private void SetLetterInCubes()
        {
            char[] copy = letters.Shuffle().ToArray();
            string correctLetter = letters.Substring(randomIds[curRandomId], 1);
            int rngId = Random.Range(0, letterCubes.Length);

            for (int i = 0; i < letterCubes.Length; i++)
            {
                if(i == rngId)
                    letterCubes[i].UpdateChar(correctLetter, lettersMeshDict[correctLetter[0]], true);
                else
                {
                    string letter = copy[i].ToString();

                    if (letter.Equals(correctLetter)) i++;

                    letterCubes[i].UpdateChar(copy[i].ToString(), lettersMeshDict[copy[i]], false);
                }
            }
        }

        private void LetterCubeKilled(Minigame08LetterCube letterCube)
        {
            DeactivateCrystals();

            if (letterCube.myChar.Equals(letters.Substring(randomIds[curRandomId],1)))
            {
                slotsMeshFilters[randomIds[curRandomId]].gameObject.SetActive(true);
                curRandomId++;
            }
            if (curRandomId >= randomAmount) Invoke(nameof(SetCase), 2f);
            else Invoke(nameof(ActivateCrystals), 2f);
        }

        private void DeactivateCrystals()
        {
            foreach (var item in letterCubes)
            {
                item.DisableCube();
            }
        }

        private void ActivateCrystals()
        {
            foreach (var item in letterCubes)
            {
                item.ActivateCrystal();
            }
            SetLetterInCubes();
        }

        /*
        private IEnumerator MoveCurrentLine()
        {

        }
        */

        private void SetCase()
        {
            if (lastId == 21) lettersAmount = 5;
            else if (lastId == 26)
            {
                Victory();
            }

            Instantiate(letterLinePrefab, transform);
            slotsMeshFilters = GetComponentsInChildren<MeshFilter>();

            SetLetters(letters.ToCharArray(lastId, lettersAmount));
            SetRandomLetters();
            ActivateCrystals();

            lastId += lettersAmount;
        }

        private void Victory()
        {
            Debug.Log("Finished");
        }
    }
}

