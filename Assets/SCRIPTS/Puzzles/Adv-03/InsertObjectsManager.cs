using UnityEngine;

public class InsertObjectsManager : MonoBehaviour
{
    private GrabbableObject[] m_objs;
    private Vector3[] initialPositions;
    // Start is called before the first frame update
    void Start()
    {
        m_objs = GetComponentsInChildren<GrabbableObject>();
        initialPositions = new Vector3[m_objs.Length];
        for (int i = 0; i < m_objs.Length; i++)
        {
            initialPositions[i] = m_objs[i].transform.position;
        }
    }

    public void ResetObjPos(int id)
    {
        for (int i = 0; i < m_objs.Length; i++)
        {
            if(m_objs[i].objectID == id)
            {
                m_objs[i].transform.parent.SetParent(null);
                m_objs[i].transform.position = initialPositions[i];
                m_objs[i].SetPhysics(true);
            }
        }
    }

    public GrabbableObject[] GetObjs()
    {
        return m_objs;
    }
}