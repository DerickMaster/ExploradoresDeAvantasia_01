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

    public void ReturnToLoginScreen()
    {
        GameManager.instance.DeleteCredentials();
        GameManager.instance.LoadScene("LoginScreen");
    }
}
