using UnityEngine.UI;
using UnityEngine;

public class Tutorial_01_Manager : MonoBehaviour
{
    struct TutorialTrigger
    {
        public GameObject myObject;
        public string Text, MobileText;
    }

    CanvasBehaviour _canvas;
    TutorialTrigger[] _triggers;
    int _tutorialIndex = -1;
    [SerializeField] float _textTimer;
    private void Start()
    {
        _canvas = CanvasBehaviour.Instance;
    }

    public void AdvanceTutorial()
    {
        _tutorialIndex += 1;
        EnableObject();
    }

    public void EnableObject()
    {
        _triggers[_tutorialIndex].myObject.SetActive(true);
#if UNITY_ANDROID
        _canvas.SetActiveTempText(_triggers[_tutorialIndex].MobileText, 2f);
#endif
        _canvas.SetActiveTempText(_triggers[_tutorialIndex].Text, 2f);
    }
}
