using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EndgameScreenActivator : MonoBehaviour
{
    [SerializeField] private EndgameManager endgameManager;
    [SerializeField] private IEndgame[] enders;

    public CanvasBehaviour _canvasBehaviour;

    private void Start()
    {
        _canvasBehaviour = FindObjectOfType<CanvasBehaviour>();
    }

    public void ActivateEndgameScreen()
    {
        _canvasBehaviour.SetActivePlayerUI(false);
        _canvasBehaviour.SetActiveBaseMobileUI(false);
        StartCoroutine(AppearDelay());
    }

    [SerializeField] float _delay = 3f;
    private IEnumerator AppearDelay()
    {
        yield return new WaitForSeconds(_delay);
        endgameManager.gameObject.SetActive(true);
    }

    [SerializeField] GameObject _victoryRibbon, _defeatRibbon, _homeButton, _continueButton, _reloadButton; 
    public void DefeatScreen()
    {
        _victoryRibbon.SetActive(false);
        _continueButton.SetActive(false);

        _defeatRibbon.SetActive(true);
        _homeButton.SetActive(true);
        _reloadButton.SetActive(true);
    }
}
