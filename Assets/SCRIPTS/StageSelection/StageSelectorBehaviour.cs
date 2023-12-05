using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StageSelection
{
    [System.Serializable]
    public struct StageLoadInformation
    {
        public Sprite stageImage;
        public string sceneName;
        public string stageName;
        public string sceneSlug;
        public bool hasIntro;
        public bool hide;
        public bool hasHardMode;
    }

    public class StageSelectorBehaviour : MonoBehaviour
    {
        [SerializeField] GameObject buttonsListParent;
        [SerializeField] GameObject buttonPrefab;

        [SerializeField] CategoryInformation[] categories;
        private StageButtonBehaviour[] buttons;
        private GameObject lastClicked;
        private EventSystem eventSystem;

        private void Start()
        {

            eventSystem = EventSystem.current;
            buttons = new StageButtonBehaviour[9];
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = Instantiate(buttonPrefab, buttonsListParent.transform).GetComponent<StageButtonBehaviour>();
                buttons[i].btnClicked.AddListener(StageButtonClicked);
                //buttons[i].gameObject.SetActive(false);
            }

            categories = GetComponentsInChildren<CategoryInformation>();
            foreach (var category in categories)
            {
                category.categoryClicked.AddListener(LoadCategoriesStages);
            }
            //ChangeButtonsInfo(GetComponentInChildren<CategoryInformation>()); 
            //AdvPressed();
            Invoke(nameof(FirstInitialize), Time.deltaTime);
        }

        [ContextMenu("TestMoveAnimation")]
        public void DebugMoveButtons()
        {
            //StartCoroutine(MoveStageButtons());
        }

        private void FirstInitialize()
        {
            ChangeButtonsInfo(GetComponentInChildren<CategoryInformation>());
            AdvPressed();
        }

        private void LoadCategoriesStages(CategoryInformation categoryClicked)
        {
            if (categoryClicked.gameObject != _activeBtn.gameObject) StartCoroutine(SwitchStagesAnimation(categoryClicked));
        }

        IEnumerator SwitchStagesAnimation(CategoryInformation categoryClicked)
        {
            if(lastClicked != null) lastClicked.GetComponent<StageButtonBehaviour>().ReturnGameScore();
            //buttonsListParent.GetComponent<HorizontalLayoutGroup>().enabled = false;
            eventSystem.enabled = false;

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].gameObject.activeInHierarchy)
                {
                    StartCoroutine(ChangeSize(buttons[i].gameObject, true) );
                }
                else break;
            }

            yield return new WaitForSeconds(shrinkTime);

            ChangeButtonsInfo(categoryClicked);

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].gameObject.activeInHierarchy)
                {
                    StartCoroutine(ChangeSize(buttons[i].gameObject, false));
                }
                else break;
            }

            yield return new WaitForSeconds(shrinkTime);

            //buttonsListParent.GetComponent<HorizontalLayoutGroup>().enabled = true;
            eventSystem.enabled = true;
            lastClicked = null;
        }


        [SerializeField] float shrinkTime;
        IEnumerator ChangeSize(GameObject button, bool shrink)
        {
            float timeElapsed = 0f;
            float initialSize = (shrink ? 1f : 0f);
            float finalSize = (shrink ? 0f : 1f);
            while(timeElapsed < shrinkTime)
            {
                yield return null;
                float size = Mathf.Lerp(initialSize, finalSize, timeElapsed / shrinkTime);
                button.transform.localScale = Vector3.one * size;
                timeElapsed += Time.deltaTime;
            }
        }

        private void ChangeButtonsInfo(CategoryInformation categoryClicked)
        {
            LeaveHardMode();
            StageLoadInformation[] stagesToLoad = categoryClicked.GetStagesInfo();
            for (int i = 0; i < buttons.Length; i++)
            {
                try
                {
                    buttons[i].gameObject.SetActive(true);
                    buttons[i].SetStageInformation(stagesToLoad[i]);
                    buttons[i].SetActiveLockScreen(stagesToLoad[i].hide);
                                    }
                catch (System.IndexOutOfRangeException)
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }

        private void StageButtonClicked(StageButtonBehaviour buttonClicked)
        {
            if (lastClicked != null)
            {
                if(lastClicked == buttonClicked.gameObject)
                {
                    buttonClicked.GetComponent<ObjectSounds>().playObjectSound(1);
                    LoadScene(buttonClicked);
                }
                else
                {
                    lastClicked.transform.localScale = Vector3.one;
                    lastClicked.GetComponent<StageButtonBehaviour>().ReturnGameScore();
                }
            }
            if(lastClicked != buttonClicked.gameObject) buttonClicked.GetComponent<ObjectSounds>().playObjectSound(0);
            lastClicked = buttonClicked.gameObject;
            lastClicked.transform.localScale = Vector3.one * 1.05f;
        }

        private void LoadScene(StageButtonBehaviour buttonClicked)
        {
            if (buttonClicked.hasIntro)
            {
                GameManager.instance._cinematicInfo = new GameManager.CinematicInfo { _sceneId = buttonClicked.sceneName, _opening = true };
                SceneManager.LoadScene("BookStoryTeller");
            }
            else SceneManager.LoadScene(buttonClicked.sceneName);
        }

        #region Categories buttons

        public Animator _advBtn, _miniBtn, _bossBtn;
        public Animator _activeBtn;

        public void AdvPressed()
        {
            if(_activeBtn != null && _activeBtn != _advBtn) _activeBtn.SetTrigger("Disappear");
            if(_activeBtn != _advBtn)
            {
                _hardModeBtn.SetActive(false);
                _advBtn.SetTrigger("Appear");
                _activeBtn = _advBtn;
            }
        }

        public void MiniPressed()
        {
            if (_activeBtn != null && _activeBtn != _miniBtn) _activeBtn.SetTrigger("Disappear");
            if(_activeBtn != _miniBtn)
            {
                _hardModeBtn.SetActive(true);
                _miniBtn.SetTrigger("Appear");
                _activeBtn = _miniBtn;
            }
        }

        public void BossPressed()
        {
            if (_activeBtn != null && _activeBtn != _bossBtn) _activeBtn.SetTrigger("Disappear");
            if(_activeBtn != _bossBtn)
            {
                _hardModeBtn.SetActive(false);
                _bossBtn.SetTrigger("Appear");
                _activeBtn = _bossBtn;
            }
        }

        [SerializeField] GameObject _hardModeBtn;
        bool _hardMode = false;
        public void MiniHardPressed()
        {
            _hardMode = !_hardMode;
            foreach (var stage in buttons)
            {
                if (_hardMode) stage.EnterHardMode();
                else stage.LeaveHardMode();
            }
        }

        void EnterHardMode()
        {
            foreach (var stage in buttons)
            {
                stage.EnterHardMode();
            }
        }

        void LeaveHardMode()
        {
            _hardMode = false;
            foreach (var stage in buttons)
            {
                stage.LeaveHardMode();
            }
        }

        #endregion
    }

}