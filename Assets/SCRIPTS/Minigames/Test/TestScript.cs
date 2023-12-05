using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private Vector3[] basePos;
    bool active = false;
    [SerializeField] Vector3 offset;
    [SerializeField] LineRenderer line;
    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            for (int i = 0; i < line.positionCount; i++)
            {
                line.SetPosition(i, basePos[i] + offset);
            }
        }
    }

    [ContextMenu("base position")]
    private void GetObjBasePos()
    {
        basePos = new Vector3[line.positionCount];
        for (int i = 0; i < line.positionCount; i++)
        {
            basePos[i] = line.GetPosition(i);
        }
        active = true;
    }
}
