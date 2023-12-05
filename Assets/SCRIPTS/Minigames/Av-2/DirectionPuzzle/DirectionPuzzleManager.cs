using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct ZoneDirectionTuple
{
    public int _id;
    public string _direction;
}

public class DirectionPuzzleManager : MonoBehaviour, IMakeMistakes
{
    public List<int> correctZoneIDs;
    public int currentId = 0;
    public int[] _1stPathIds;
    public int[] _2ndPathIds;
    public int[] _3rdPathIds;

    private DirectionZoneBehaviour[] directionZones;
    [SerializeField] private ZoneDirectionTuple[] _1stPathZones;
    [SerializeField] private ZoneDirectionTuple[] _2ndPathZones;
    [SerializeField] private ZoneDirectionTuple[] _3rdPathZones;


    [SerializeField] private GrassWallBehaviour[] walls;

    public FlowersGateBehaviour flowerGate;

    [HideInInspector] public UnityEvent<string,MistakeData> _wrongDirection;
    public string _puzzleName;
    public MistakeData _mistakeDescription;

    private void Start()
    {
        FindZones();
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].touched.AddListener(ZoneTouched);
            walls[i].zoneID = i;
        }
        flowerGate.flowerInputed.AddListener(FlowerInputted);
        correctZoneIDs.AddRange(_1stPathIds);
        ActivateDirectionZones(_1stPathZones);

        GrabbableObject[] flowers = GetComponentsInChildren<GrabbableObject>();
        foreach (GrabbableObject flower in flowers)
        {
            flower.obj_Grab_Drop.AddListener(DeactivateZones);
        }
    }

    private void ActivateFlowerSocket(GrabbableObject obj)
    {
        if (obj.grabbed)
        {

        }
    }

    [ContextMenu("Find zones")]
    public void FindZones()
    {
        directionZones = GetComponentsInChildren<DirectionZoneBehaviour>();
        for (int i = 0; i < directionZones.Length; i++)
        {
            directionZones[i]._zoneID = i;
        }
    }

    private void DeactivateZones(GrabbableObject obj)
    {
        if (!obj.grabbed) return;

        foreach (DirectionZoneBehaviour zone in directionZones)
        {
            zone.gameObject.SetActive(false);
        }
    }

    private void ActivateDirectionZones(ZoneDirectionTuple[] zones)
    {
        int count = 0;
        foreach(DirectionZoneBehaviour zone in directionZones)
        {
            if(count < zones.Length && zone._zoneID == zones[count]._id)
            {
                zone.gameObject.SetActive(true);
                zone.direction = zones[count]._direction;
                count++;
            }
            else
            {
                zone.gameObject.SetActive(false);
            }
        }
    }

    private void FlowerInputted()
    {
        foreach(GrassWallBehaviour wall in walls)
        {
            wall.SetWall(false);
        }

        correctZoneIDs.Clear();
        currentId++;
        switch (currentId)
        {
            case 1:
                correctZoneIDs.AddRange(_2ndPathIds);
                ActivateDirectionZones(_2ndPathZones);
                break;
            case 2:
                correctZoneIDs.AddRange(_3rdPathIds);
                ActivateDirectionZones(_3rdPathZones);
                break;
            case 3:
                GetComponent<ObjectSounds>().playObjectSound(0);
                break;
            default:
                break;
        }
    }

    public Sprite _portrait;
    private void ZoneTouched(BlockedZoneBehaviour zone)
    {
        if(correctZoneIDs.Contains(zone.zoneID))
        {
            zone.allowPassage = true;
            Debug.Log("Lado correto pode continuar");
        }
        else
        {
            _wrongDirection.Invoke(_puzzleName, _mistakeDescription);
            CanvasBehaviour.Instance.SetActiveTempText("Lado incorreto tente novamente", 2f, _portrait);
        }
    }

    [ContextMenu("Find walls")]
    private void FindWalls()
    {
        walls = GetComponentsInChildren<GrassWallBehaviour>();
    }

    private bool[] currentTargetWalls;
    [ContextMenu("Raisings walls")]
    private void SetWalls()
    {

        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].SetWall(currentTargetWalls[i]);
        }
    }

    [ContextMenu("SetIDWalls")]
    private void IDWalls()
    {
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].gameObject.name = "ForestWall_" + i.ToString();
            walls[i].zoneID = i;
        }
    }

    public UnityEvent<string, MistakeData> GetMistakeEvent()
    {
        return _wrongDirection;
    }
}
