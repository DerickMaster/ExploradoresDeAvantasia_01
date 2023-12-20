using UnityEngine.UI;
using UnityEngine;

public class Tutorial_01_Manager : MonoBehaviour
{
    [System.Serializable]
    struct TutorialTrigger
    {
        public Tutorial_01_ZoneTrigger myObject;
        public string Text, MobileText;
    }

    CanvasBehaviour _canvas;
    [SerializeField] TutorialTrigger[] _triggers;
    int _tutorialIndex = -1;
    [SerializeField] float _textTimer;
    [SerializeField] string _gearInstruction, _gearInstructionMobile;
    [SerializeField] LauraGear _gear;
    [SerializeField] string _boxInstruction, _boxInstructionMobile;
    [SerializeField] GameObject _box;
    private void Start()
    {
        _canvas = CanvasBehaviour.Instance;
        foreach (var item in _triggers)
        {
            item.myObject._enteredZoneEvent.AddListener(AdvanceTutorial);
        }
        _box.GetComponent<BreakableBox>().unitKilled.AddListener(AdvanceTutorial);
    }

    public void AdvanceTutorial()
    {
        _tutorialIndex += 1;
        ActivateObject();
        if (_tutorialIndex == _triggers.Length - 1)
        {
            _box.SetActive(true);
            _canvas.SetActiveTempText(_boxInstruction, _textTimer);
        }else if(_tutorialIndex == _triggers.Length)
        {
            _gear.enabled = true;
            _canvas.SetActiveTempText(_gearInstruction, _textTimer);
        }
    }

    public void ActivateObject()
    {
        _triggers[_tutorialIndex].myObject.gameObject.SetActive(true);
#if UNITY_ANDROID
        _canvas.SetActiveTempText(_triggers[_tutorialIndex].MobileText, _textTimer);
#endif
        _canvas.SetActiveTempText(_triggers[_tutorialIndex].Text, _textTimer);
    }
}
