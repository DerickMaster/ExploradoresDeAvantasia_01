using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Bomb_Door : DoorBehaviour, IMakeMistakes
{
    public string _puzzleName = "Portões";
    public MistakeData _mistakeDescription;
    public GameObject _bombFruit;

    public int _bombCount = 0;

    [HideInInspector] public UnityEvent<Bomb_Door> _doorOpenedEvent;
    [HideInInspector] public UnityEvent<string, MistakeData> _wrongDoorOpened;
    public string _rightAnswer;
    public string _mistakeMade;
    public int id = 0;

    private void Awake()
    {
        _wrongDoorOpened = new UnityEvent<string,MistakeData>();
        _bombFruit = Resources.Load<GameObject>("Prefabs/Frutabomba");
    }

    public UnityEvent<string,MistakeData> GetMistakeEvent()
    {
        return _wrongDoorOpened;
    }

    public override void Interact(InteractionController interactor)
    {
        base.Interact(interactor);
        SpawnBomb(interactor);
    }

    public void SpawnBomb(InteractionController interactor)
    {
        _doorOpenedEvent.Invoke(this);
        _wrongDoorOpened.Invoke(_puzzleName, new MistakeData { rightAnswer = _rightAnswer, mistakeMade = _mistakeMade });
        _wrongDoorOpened.Invoke(_puzzleName,_mistakeDescription);
        StartCoroutine(BombCooldown(interactor));
    }

    private IEnumerator BombCooldown(InteractionController interactor)
    {

        yield return new WaitForSeconds(1f);
        for (int i = 0; i < _bombCount; i++)
        {

            GameObject bombFruit = Instantiate(_bombFruit, transform.position + (transform.forward*2),  transform.rotation);
            bombFruit.transform.Rotate(Vector3.up * 180);
            bombFruit.GetComponent<FruitbombBehaviour>().Initialize(interactor.transform.parent.gameObject);
            bombFruit.GetComponent<NavMeshAgent>().speed = Random.Range(13, 20);
            yield return new WaitForSeconds(2f);
        }
    }
}
