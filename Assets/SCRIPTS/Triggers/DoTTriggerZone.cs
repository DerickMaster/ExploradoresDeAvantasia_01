using System.Collections;
using UnityEngine;

public class DoTTriggerZone : TriggerZone
{
    [SerializeField] Color debugAreaColor;
    [SerializeField] bool debugShowArea;

    [SerializeField] string dotName;
    [SerializeField] float stepBuildupMulti;
    [SerializeField] float maxBuildup;
    [SerializeField] int buildupDmg;

    IEnumerator _countdown;
    private Collider _col;

    private void Start()
    {
        _col = GetComponent<Collider>();
    }

    public void SetActiveCollider(bool active)
    {
        _col.enabled = active;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<HurtBox>().TakeBuildUp(dotName, Time.deltaTime * stepBuildupMulti, maxBuildup, buildupDmg);
        }
    }

    private void OnDrawGizmos()
    {
        if (debugShowArea)
        {
            Gizmos.color = debugAreaColor;
            Gizmos.DrawCube(gameObject.transform.position, GetComponent<Collider>().bounds.size);
        }
            
    }
}
