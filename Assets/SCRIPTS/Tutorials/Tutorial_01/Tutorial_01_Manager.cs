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
    [SerializeField] Tutorial_01_ZoneTrigger _firstTrigger;
    [SerializeField] TutorialTrigger[] _triggers;
    [SerializeField] int _tutorialIndex = -1;
    [SerializeField] float _textTimer;
    [SerializeField] string _boxInstruction, _boxInstructionMobile;
    [SerializeField] GameObject _box;
    [SerializeField] string _gearInstruction, _gearInstructionMobile;
    [SerializeField] GameObject _gear;
    private DeviceType _myDevice;
    private void Start()
    {
        _myDevice = SystemInfo.deviceType;
        _canvas = CanvasBehaviour.Instance;

        _firstTrigger._enteredZoneEvent.AddListener(AdvanceTutorial);
        foreach (var item in _triggers)
        {
            item.myObject._enteredZoneEvent.AddListener(AdvanceTutorial);
        }
        _box.GetComponent<BreakableBox>().unitKilled.AddListener(AdvanceTutorial);
    }

    public void AdvanceTutorial()
    {
        _tutorialIndex += 1;
        
        if (_tutorialIndex == _triggers.Length)
        {
            _box.SetActive(true);
            if(_myDevice == DeviceType.Handheld)
                _canvas.SetActiveTempText(_boxInstructionMobile, _textTimer);
            else
                _canvas.SetActiveTempText(_boxInstruction, _textTimer);
        }else if(_tutorialIndex > _triggers.Length)
        {
            _gear.SetActive(true);
            if (_myDevice == DeviceType.Handheld)
                _canvas.SetActiveTempText(_boxInstructionMobile, _textTimer);
            else
                _canvas.SetActiveTempText(_gearInstruction, _textTimer);
        }
        else
        {
            ActivateObject();
        }
    }

    public void ActivateObject()
    {
        _triggers[_tutorialIndex].myObject.gameObject.SetActive(true);
        if (_myDevice == DeviceType.Handheld)
            _canvas.SetActiveTempText(_triggers[_tutorialIndex].MobileText, _textTimer);
        else
            _canvas.SetActiveTempText(_triggers[_tutorialIndex].Text, _textTimer);
    }
}
