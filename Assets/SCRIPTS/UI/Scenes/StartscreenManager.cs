using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartscreenManager : MonoBehaviour
{
    [SerializeField] private GameObject loginCanvas;
    [SerializeField] private GameObject logoObj;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject loginPanel;
    [SerializeField] GameObject offlineButton;
    [SerializeField] GameObject _resetCredentialsButton;
    [SerializeField] GameObject exitConfirmationPanel;
    public bool _skipLoginScreen = false;
    
    public void StartButtonPressed()
    {
        if(_skipLoginScreen) GameManager.instance.ReturnToStageScreen();
        else
        {
            startPanel.SetActive(false);
            StartCoroutine(MoveLogoCoroutine());
        }
    }

    [SerializeField] Vector3 offset;
    [SerializeField] private float movementTime;
    IEnumerator MoveLogoCoroutine()
    {
        offlineButton.SetActive(true);
        _resetCredentialsButton.SetActive(false);
        loginPanel.gameObject.SetActive(true);
        Image[] images = loginPanel.GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            img.color = new Color(1, 1, 1, 0);
        }
        RectTransform canvasTransform = GetComponent<RectTransform>();
        //float x = canvasTransform.position.x;
        float y = (canvasTransform.rect.yMax);
        Debug.Log(canvasTransform.rect.yMax);

        RectTransform logoTransform = logoObj.GetComponent<RectTransform>();
        Vector3 initialPos = logoTransform.localPosition;
        Vector3 targetPos = new Vector3(0f, y * 0.65f);
        Debug.Log(targetPos);

        float elapsedTime = 0f;
        while(elapsedTime < movementTime)
        {
            yield return null;
            logoTransform.localPosition = Vector3.Lerp(initialPos, targetPos, elapsedTime / movementTime);
            elapsedTime += Time.deltaTime;

            if(elapsedTime > movementTime / 2f)
            {
                foreach (Image img in images)
                {
                    img.color = new Color(1, 1, 1, (elapsedTime - movementTime / 2f) / (movementTime / 2f));
                }
            }
        }

        foreach (Image img in images)
        {
            img.color = Color.white;
        }
    }

    [SerializeField] GameObject _confirmCanvas;
    public void ShowDeleteCredentialsPanel(bool open)
    {
        if (open) _confirmCanvas.SetActive(true);
        else _confirmCanvas.SetActive(false);
    }

    public void DeleteCredentials()
    {
        GameManager.instance.DeleteCredentials();
        _confirmCanvas.SetActive(false);
    }

    public void OfflineLogin()
    {
        GameManager.instance.ReturnToStageScreen();
    }

    public void ExitButtonClicked()
    {
        exitConfirmationPanel.SetActive(true);
    }

    public void YesExitClicked()
    {
        Application.Quit();
    }
    
    public void NoExitClicked()
    {
        exitConfirmationPanel.SetActive(false);
    }
}
