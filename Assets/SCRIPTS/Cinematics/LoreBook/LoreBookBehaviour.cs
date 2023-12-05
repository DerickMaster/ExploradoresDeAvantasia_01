using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoreBookBehaviour : MonoBehaviour
{

#pragma warning disable 0649
    [System.Serializable]
    struct MaterialInfo
    {
        public string _name;
        public Material _material;
    }

    [System.Serializable]
    struct PageInfo
    {
        public GameObject _obj;
        public Animator _anim;
        public Renderer _renderer;
    }
#pragma warning restore 0649

    [SerializeField]
    MaterialInfo[] _materials;

    public StorybookMaterialObject[] _adventureMaterials;
    StorybookMaterialObject _currentAdventure;
    int _adventureIndex;

    [SerializeField] PageInfo[] _pages;
    [SerializeField]int _pageIndex = 0;
    bool _pageOne = true;

    public Dictionary<string, Material> _cinematicMaterial;

    Animator _myAnimator;
    ObjectSounds _pageSound;

    // Start is called before the first frame update
    void Start()
    {
        _myAnimator = GetComponent<Animator>();
        _myAnimator.SetBool("isOpen", !GameManager.instance._cinematicInfo._opening);
        _pageSound = GetComponent<ObjectSounds>();

        _cinematicMaterial = new Dictionary<string, Material>();
        //Popular dicionario de materiais
        foreach (var item in _materials)
        {
            _cinematicMaterial.Add(item._name, item._material);
        }
        _materials = new MaterialInfo[0];

        //Pegar as referencias dos materiais das paginas
        for (int i = 0; i < _pages.Length; i++)
        {
            _pages[i]._anim = _pages[i]._obj.GetComponent<Animator>();
            _pages[i]._renderer = _pages[i]._obj.GetComponentInChildren<Renderer>();
            _pages[i]._obj.GetComponent<LorePageBehaviour>()._passPage.AddListener(CheckNextPage);
        }

        //Transformar o sceneId de string para int para pegar o index do array de SO de dialogo
        _adventureIndex = GetAdventureId(GameManager.instance._cinematicInfo._sceneId);
        _currentAdventure = _adventureMaterials[_adventureIndex];

        GameManager.instance.FadeIn();
    }

    public void InitializeBook()
    {
        try
        {
            PlayPageTrack(_currentAdventure._audioTrack[_pageIndex]);
            _pageTime = _currentAdventure._audioTime[_pageIndex];
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("Audio track for this page not set");
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.Log("not enough audio tracks for this page");

        }
        
        _pages[0]._anim.SetBool("Active", true);
        _pages[1]._anim.SetBool("Active", false);
        ChangeMaterial(GetMaterial(_currentAdventure._materialNames[0]), _pages[0]);
        ChangeMaterial(GetMaterial(_currentAdventure._materialNames[1]), _pages[1]);
        _pageIndex++;

        if (GameManager.instance._cinematicInfo._opening)
        {
            _myAnimator.SetBool("isOpen", GameManager.instance._cinematicInfo._opening);
        }
        else StartStory();
    }

    int GetAdventureId(string sceneId)
    {
        if (GameManager.instance._cinematicInfo._opening) sceneId = sceneId + "_OP";
        else sceneId = sceneId + "_ED";
        switch (sceneId)
        {
            case ("Aventura_01_OP"):
                return 0;
            case ("Aventura_01_ED"):
                return 1;
            case ("Aventura_02_OP"):
                return 2;
            case ("Aventura_02_ED"):
                return 3;
            case ("Aventura_03_OP"):
                return 4;
            case ("Aventura_03_ED"):
                return 5;
            default:
                return 999;
        }
    }

    Material GetMaterial(string name)
    {
        return _cinematicMaterial[name];
    }

    public void StartStory()
    {
        StartCoroutine(DisplayPage());
    }

    [SerializeField] float _pageTime;
    private IEnumerator DisplayPage()
    {
        while (_pageIndex < _currentAdventure._materialNames.Length-1)
        {
            yield return new WaitForSeconds(_pageTime);
            SwitchPage();
        }
        yield return new WaitForSeconds(_pageTime);
        FaderUIController.instance._fadeEnded.AddListener(LoadStoryScene);
        if (!GameManager.instance._cinematicInfo._opening) _myAnimator.SetBool("isOpen", false);
        GameManager.instance.FadeOut();
    }

    public void SwitchPage()
    {
        _pageTime = _currentAdventure._audioTime[_pageIndex];
        _pageSound.playObjectSound(0);
        _pageOne = !_pageOne;
        _pages[0]._anim.SetBool("Active", _pageOne);
        _pages[1]._anim.SetBool("Active", !_pageOne);

        try
        {
            PlayPageTrack(_currentAdventure._audioTrack[_pageIndex]);
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("Audio track for this page not set");
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.Log("not enough audio tracks for this page");

        }
    }

    void ChangeMaterial(Material pageMaterial, PageInfo page)
    {
        Material[] materials = page._renderer.materials;
        materials[1] = pageMaterial;
        page._renderer.materials = materials;
    }

    FMODUnity.EventReference currentDialogueTrack;
    FMOD.Studio.EventInstance _eventTrack;
    public void PlayPageTrack(FMODUnity.EventReference pageTrack)
    {
        if (!pageTrack.IsNull)
        {
            currentDialogueTrack = pageTrack;
            _eventTrack.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            _eventTrack = FMODUnity.RuntimeManager.CreateInstance(currentDialogueTrack);
            _eventTrack.start();
        }
    }

    void CheckNextPage()
    {
        _pageIndex++;
        PageInfo localPage;
        if (_pageIndex >= _currentAdventure._materialNames.Length)
        {
            return;
        }
        else
        {
            if (!_pageOne)
                localPage = _pages[0];
            else
                localPage = _pages[1];

            ChangeMaterial(GetMaterial(_currentAdventure._materialNames[_pageIndex]), localPage);
        }
    }

    public void LoadStoryScene()
    {
        FaderUIController.instance._fadeEnded.RemoveListener(LoadStoryScene);
        if (GameManager.instance._cinematicInfo._opening) SceneManager.LoadScene(GameManager.instance._cinematicInfo._sceneId);
        else GameManager.instance.ReturnToStageScreen();
    }
}

