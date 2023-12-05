using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionZoneBehaviour : MonoBehaviour
{
    public int _zoneID;
    public string direction;
    [SerializeField] float messageTime;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player"))
            ShowMessage();
    }

    private void ShowMessage()
    {
        CanvasBehaviour.Instance.SetActiveTempText("Va para " + direction, messageTime);
    }

}
