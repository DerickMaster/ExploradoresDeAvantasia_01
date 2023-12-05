using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using System.Collections;

public class SkipButtonBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    PlayableDirector[] playables;
    PlayableDirector curPlayable;
    Image btnImage;
    bool skipping = false;
    Coroutine _myCoroutine;
    [SerializeField] float _timer;
    [SerializeField]Image _chargeBar;
    [SerializeField] Image _chargeBg;
    private void Awake()
    {
        //_myCoroutine = WaitCoroutine(_timer);
        playables = FindObjectsOfType<PlayableDirector>();
        btnImage = GetComponent<Image>();
        foreach (PlayableDirector playable in playables)
        {
            playable.played += SetCurrentPlayable;
            playable.stopped += DeactivateButton;
        }
    }

    private void SetCurrentPlayable(PlayableDirector aDirector)
    {
        curPlayable = aDirector;
        btnImage.enabled = true;
        _chargeBg.enabled = true;
    }

    private void DeactivateButton(PlayableDirector aDirector)
    {
        btnImage.enabled = false;
        _chargeBg.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _chargeBar.gameObject.SetActive(true);
        _myCoroutine = StartCoroutine(WaitCoroutine(_timer));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _windUp = 0f;
        _chargeBar.fillAmount = _windUp;
        _chargeBar.gameObject.SetActive(false);
        StopCoroutine(_myCoroutine);
    }

    float _windUp = 0f;
    IEnumerator WaitCoroutine(float timer)
    {
        while (_windUp < timer)
        {
            _windUp += Time.deltaTime;
            _chargeBar.fillAmount = _windUp/timer;
            yield return null;
        }
        //_chargeBar.fillAmount = 0f;
        _chargeBar.gameObject.SetActive(false);
        OnBtnClicked();
    }

    public void OnBtnClicked()
    {
        if (skipping) return;
        
        try
        {
            double endTime = curPlayable.playableAsset.duration;
            curPlayable.time = endTime - 0.1f;
            //curPlayable.playableGraph.GetRootPlayable(0).SetSpeed(1000);
            skipping = true;
            curPlayable.stopped += CleanupCutscene;
        }
        catch
        {
            Debug.Log("Tried to skip cutscene but one wasnt being played");
        }
    }

    private void CleanupCutscene(PlayableDirector aDirector)
    {
        skipping = false;
        _windUp = 0f;

        if (DialogueBoxManager.Instance.interactorRef != null)
            DialogueBoxManager.Instance.EndCurrentDialogue();
        CharacterManager.Instance.GetCurrentCharacter().GetComponent<StarterAssets.ThirdPersonController>().SwitchControl(false);
    }
}