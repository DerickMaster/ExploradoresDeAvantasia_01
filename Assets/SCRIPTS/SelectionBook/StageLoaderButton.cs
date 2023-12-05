using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StageLoaderButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] string sceneName;
    [SerializeField] Material m_Material;
    public bool _hasIntro = false;

    public Outline m_Outline;
    public bool locked = false;
    [HideInInspector] public UnityEvent<string> btnClicked;

    private void Start()
    {
        m_Outline = GetComponent<Outline>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_hasIntro)
        {
            GameManager.instance._cinematicInfo._sceneId = sceneName;
            GameManager.instance._cinematicInfo._opening = true;
            btnClicked.Invoke("BookStoryTeller");
        }
        else
        {
            btnClicked.Invoke(sceneName);
        }
    }

    public string GetSceneName()
    {
        return sceneName;
    }

    public void SetLockButton( bool locked,Material lockedMat = null)
    {
        if (locked)
            GetComponentInChildren<Renderer>().material = lockedMat;
        else
            GetComponentInChildren<Renderer>().material = m_Material;
        this.locked = locked;
    }
}