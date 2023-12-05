using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame06Manager_V2 : MonoBehaviour
{
    PlatformLayerManager[] layers;

    private void Start()
    {
        layers = GetComponentsInChildren<PlatformLayerManager>();
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].id = i;
            layers[i].cycledEvent.AddListener(ActivateLayer);
        }
    }

    [ContextMenu("Start")]
    public void StartGame()
    {
        layers[0].enabled = true;
    }

    private void ActivateLayer(PlatformLayerManager lastLayer)
    {
        if (lastLayer.playerInside) lastLayer.ReactivateLayer();
        else
        {
            lastLayer.gameObject.SetActive(false);
            layers[lastLayer.id + 1].curNum = lastLayer.curNum;
            layers[lastLayer.id + 1].enabled = true;
        }
    }
}
