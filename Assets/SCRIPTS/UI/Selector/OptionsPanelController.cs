using UnityEngine;
using UnityEngine.UI;

public class OptionsPanelController : MonoBehaviour
{
    public GameObject OptionScreen;
    public SpecialConfSelector _specialConfs;
    public Slider _masterVolumeSlider;
    FMOD.Studio.Bus _masterVolume, _sfxVolume, _musicVolume;

    // Start is called before the first frame update
    void Start()
    {
        _musicVolume = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        _sfxVolume = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        _masterVolume = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        _masterVolumeSlider.onValueChanged.AddListener(delegate { MasterSliderValueChange(); });

        ChangeGameQuality(QualitySettings.GetQualityLevel());

        if (PlayerPrefs.GetInt("TouchMove") != 1) _touchMove = false;
        else _touchMove = true;
    }

    public void ShowOptionsScreen(bool open)
    {
        float volume;
        _masterVolume.getVolume(out volume);
        if (open) { OptionScreen.SetActive(true); _masterVolumeSlider.value = volume; }
        else { OptionScreen.SetActive(false); _returnToLoginPanel.SetActive(false); _specialConfs.CloseCanvas(); }
    }

    public Sprite _box, _boxChecked;
    [SerializeField] Image[] _qualityButtons;
    public void ChangeGameQuality(int type)
    {
        for (int i = 0; i < _qualityButtons.Length; i++)
        {
            if (i != type) _qualityButtons[i].sprite = _box;
            else _qualityButtons[i].sprite = _boxChecked;
        }
        QualitySettings.SetQualityLevel(type, true);
    }

    public void MasterSliderValueChange()
    {
        _masterVolume.setVolume(_masterVolumeSlider.value);
    }

    bool muted = false;
    public void MuteAudio()
    {
        muted = !muted;
        _masterVolume.setMute(muted);
    }

    public GameObject _returnToLoginPanel;
    public void ShowReturnToLoginPanel(bool open)
    {
        if (open) _returnToLoginPanel.SetActive(true);
        else _returnToLoginPanel.SetActive(false);
    }

    [SerializeField] Sprite _touchMoveButtonSprite;
    [SerializeField] Sprite[] _images;
    bool _touchMove = false;
    public void ChangeToTouchMove()
    {
        if (!_touchMove)
        {
            PlayerPrefs.SetInt("TouchMove", 1);
            _touchMoveButtonSprite = _images[1];
            _touchMove = true;
        }
        else
        {
            PlayerPrefs.SetInt("TouchMove", 0);
            _touchMoveButtonSprite = _images[0];
            _touchMove = false;
        }
    }

    public void ReturnToLoginScreen()
    {
        GameManager.instance.DeleteCredentials();
        GameManager.instance.LoadScene("LoginScreen");
    }
}
