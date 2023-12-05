using UnityEngine.SceneManagement;
using UnityEngine;

public class Minigame06Manager : MonoBehaviour
{
    [SerializeField] GameObject[] _easyAreas;
    [SerializeField] GameObject[] _hardAreas;

    private void Start()
    {
        UIDifficultyDetector difficultyDetector = FindObjectOfType<UIDifficultyDetector>();
        difficultyDetector.easyChosenEvent.AddListener(SetAsEasy);
        difficultyDetector.hardChosenEvent.AddListener(SetAsHard);
    }

    public bool hard = false;
    public int[] _qtArea;
    public int[] _qtEasyArea;
    public int[] _qtHardArea;
    public GameObject[] _blockPosition;
    public GameObject _endgameTrigger;
    public GameObject[] _endpoints;
    
    void CreateAreas(bool hard)
    {
        if (hard) _qtArea = _qtHardArea;
        else _qtArea = _qtEasyArea;
        
        int count = 0;
        GameObject[] temp = new GameObject[0];
        foreach (int item in _qtArea)
        {
            if (item == 0)
            {
                temp = _easyAreas;
            }
            else if (item == 1)
            {
                temp = _hardAreas;
            }
            Instantiate(temp[Random.Range(0, temp.Length)], _blockPosition[count].transform);
            count++;
        }
    }
    
    void SetAsEasy()
    {
        try
        {
            //_qtarea = new int[3]{0,0,1}
            _endgameTrigger.transform.position = _endpoints[0].transform.position;
            CreateAreas(hard);
            StartGame();
        }
        catch (System.Exception)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            throw;
        }
    }

    void SetAsHard()
    {
        hard = true;
        try
        {
            //_qtarea = new int[3]{0,0,1,1}
            _endgameTrigger.transform.position = _endpoints[1].transform.position;
            CreateAreas(hard);
            StartGame();
        }
        catch (System.Exception)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            throw;
        }
    }

    // Call any needed functions to startgame
    public void StartGame()
    {

    }
}
