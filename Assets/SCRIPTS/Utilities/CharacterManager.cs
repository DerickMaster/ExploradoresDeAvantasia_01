using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    [System.Serializable]
    private struct ActiveCharacter
    {
        public bool active;
        public GameObject charPrefab;
    }

    [SerializeField] private ActiveCharacter[] _characters;
    [SerializeField] private bool UseSpawnPoint = false;
    [SerializeField] private Vector3 spawnPosition;
    public StarterAssets.StarterAssetsInputs[] _input { get; private set; }
    public StarterAssets.StarterAssetsInputs _curInput { get; private set; }
    [HideInInspector] StarterAssets.ThirdPersonController[] _playableCharacters;
    public CinemachineVirtualCamera _virtualCamera;
    private int _playerIndex = 0;
    
    [HideInInspector] public HpModifiedEvent hpModified;
    [HideInInspector] public UnityEvent charChanged;

    Camera _mainCamera;

    private void Awake()
    {
#if UNITY_EDITOR
        Debug.Log("Playing on editor, ignoring spawn point if set as such");
#else
        Debug.Log("Playing on build, using spawn point");
        UseSpawnPoint = true;
#endif
        if (!Instance) Instance = this;
        if (UseSpawnPoint) transform.position = spawnPosition;

        SpawnCharacters();

        hpModified = new HpModifiedEvent();
        _mainCamera = Camera.main;
        StartCoroutine(GetCameraAfterAFrame());
        _input = GetComponentsInChildren<StarterAssets.StarterAssetsInputs>();
        
        foreach (StarterAssets.ThirdPersonController character in _playableCharacters)
        {
            character.gameObject.SetActive(false);
            character.gameObject.GetComponent<HealthManager>().hpModified.AddListener(CharacterHpModified);
            
            Component copy = character.gameObject.AddComponent<CinemachineImpulseSource>();
            System.Type type = gameObject.GetComponent<CinemachineImpulseSource>().GetType();
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(gameObject.GetComponent<CinemachineImpulseSource>()));
            }
        }

        _playerIndex = _playableCharacters.Length -1;
        ChangeCharacter();

        _mainCamera.GetComponent<FMODUnity.StudioListener>().attenuationObject = GetCurrentCharacter();
    }

    IEnumerator GetCameraAfterAFrame()
    {
        yield return null;
        _virtualCamera = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera as CinemachineVirtualCamera;
        _virtualCamera.Follow = _playableCharacters[_playerIndex].CinemachineCameraTarget.transform;
    }

    [ContextMenu("Save Spawn Position")]
    public void SavePositionAsSpawn()
    {
        spawnPosition = transform.position;
    }

    private void SpawnCharacters()
    {
        List<GameObject> charList = new List<GameObject>();
        foreach(ActiveCharacter chara in _characters)
        {
            if(chara.active) charList.Add(Instantiate(chara.charPrefab, transform.position, Quaternion.identity, this.transform));
        }
        _playableCharacters = new StarterAssets.ThirdPersonController[charList.Count];
        for (int i = 0; i < charList.Count; i++)
        {
            _playableCharacters[i] = charList[i].GetComponent<StarterAssets.ThirdPersonController>();
        }
    }

    private void LateUpdate()
    {
        if (_input[_playerIndex].changePressed && _playableCharacters.Length > 1 && _input[_playerIndex].GetComponent<HurtBox>().canSwitch)
        {
            _input[_playerIndex].changePressed = false;
            ChangeCharacter();
            ResetMovement();
        }
    }

    [ContextMenu("Change Character")]
    public void ChangeCharacter()
    {
        Vector3 position = _playableCharacters[_playerIndex].gameObject.transform.position;
        Quaternion rotation = _playableCharacters[_playerIndex].gameObject.transform.rotation;
        int curHealth = _playableCharacters[_playerIndex].GetComponent<HealthManager>().curHealthAmount;
        _playableCharacters[_playerIndex].gameObject.SetActive(false);
        _playerIndex++;
        if (_playerIndex >= _playableCharacters.Length)
        {
            _playerIndex = 0;
        }
        _playableCharacters[_playerIndex].transform.position = position;
        _playableCharacters[_playerIndex].transform.rotation = rotation;
        _playableCharacters[_playerIndex].GetComponent<HealthManager>().curHealthAmount = curHealth;
        _playableCharacters[_playerIndex].gameObject.SetActive(true);

        if(_virtualCamera)
            _virtualCamera.Follow = _playableCharacters[_playerIndex].CinemachineCameraTarget.transform;
        _mainCamera.GetComponent<FMODUnity.StudioListener>().attenuationObject = _playableCharacters[_playerIndex].gameObject;

        _curInput = _input[_playerIndex];
        hpModified.Invoke(GetCurrentHp());
        charChanged.Invoke();
    }

    private void CharacterHpModified(int amount)
    {
        hpModified.Invoke(amount);
    }

    public int GetCurrentHp()
    {
        return _playableCharacters[_playerIndex].GetComponent<HealthManager>().curHealthAmount;
    }

    void ResetMovement()
    {
        _input[_playerIndex].move =  new Vector2(0, 0);
        _input[_playerIndex].attackHold = 0f;
    }

    public GameObject GetCurrentCharacter()
    {
        return _curInput.gameObject;
    }

    public int GetCurrentCharacterIndex()
    {
        return CharacterIndex(_curInput.gameObject.name);
    }

    public int GetNextCharacterIndex()
    {
        if( _playerIndex + 1 >= _playableCharacters.Length) { return CharacterIndex(_playableCharacters[0].gameObject.name); }
        else { return CharacterIndex(_playableCharacters[_playerIndex + 1].gameObject.name); }
    }

    int CharacterIndex(string name)
    {
        if (name == "Laura_prefab(Clone)")
        {
            return 0;
        }
        else if (name == "Tino(Clone)")
        {
            return 1;
        }
        else if (name == "Bia_prefab(Clone)")
        {
            return 2;
        }
        else if (name == "Caua_prefab(Clone)")
        {
            return 3;
        }
        else
        {
            Debug.Log("Falhou");
            return 999;
        };
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position, Vector3.one);
    }

    public StarterAssets.ThirdPersonController[] GetAllCharacters()
    {
        return _playableCharacters;
    }
}