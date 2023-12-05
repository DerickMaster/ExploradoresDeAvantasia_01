using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandRobotBehaviour : MonoBehaviour
{
    public GameObject handSlot;
    public GameObject moveableObj;
    public GameObject boardSlot;

    public UnityEvent activate;

    private bool finished = false;

    public void Initialize(GameObject handSlot, GameObject moveableObj, GameObject boardSlot)
    {
        this.handSlot = handSlot;
        this.moveableObj = moveableObj;
        this.boardSlot = boardSlot;

        activate = new UnityEvent();
    }

    [ContextMenu("Grab")]
    public void GrabObject()
    {
        if (finished) return;
        moveableObj.transform.SetParent(handSlot.transform);
        moveableObj.transform.localPosition = Vector3.zero;
        moveableObj.GetComponent<Outline>().enabled = true;
    }

    [ContextMenu("Drop")]
    public void DropObject()
    {
        moveableObj.transform.SetParent(boardSlot.transform);
        moveableObj.transform.localPosition = Vector3.zero;
        finished = true;
        moveableObj.GetComponent<Outline>().enabled = false;
    }

    public void ActivatePuzzle()
    {
        Debug.Log("alo");
        activate.Invoke();
    }
}
