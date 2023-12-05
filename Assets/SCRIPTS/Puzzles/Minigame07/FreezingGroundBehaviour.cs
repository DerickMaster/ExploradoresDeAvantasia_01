using System.Collections;
using UnityEngine;

public class FreezingGroundBehaviour : MonoBehaviour
{
    [SerializeField] Vector3 resize = new Vector3(0.85f, 1f ,0.85f);
    [SerializeField] float offsetY;
    [SerializeField] LayerMask playerMask;
    
    BoxCollider boxCol;
    Animator m_Animator;
    GameObject iceFloorGO;
    //[SerializeField] GameObject iceFloorPrefab;

    private void OnValidate()
    {
        boxCol = GetComponentInChildren<BoxCollider>();
        iceFloorGO = transform.GetChild(0).gameObject;
        //_myRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        //_myRenderer.enabled = false;
        GetComponentInChildren<FreezingGroundEventListener>().disableEvent.AddListener(DisableMesh);
        m_Animator = GetComponentInChildren<Animator>();
        OnValidate();
        playerMask = LayerMask.GetMask("Character");
        iceFloorGO.SetActive(false);
    }

    [ContextMenu("FreezeTest")]
    public void FreezeTest()
    {
        Freeze(2f);
    }

    public void Freeze(float delay)
    {
        iceFloorGO.SetActive(true);
        //_myRenderer.enabled = true;
        StartCoroutine(FreezingCoroutine(delay));
    }

    float freezeDuration = 1f;
    IEnumerator FreezingCoroutine(float delay)
    {
        yield return null;

        m_Animator.SetBool("Active", true);

        yield return new WaitForSeconds(delay);

        m_Animator.SetTrigger("Charged");

        Vector3 size = Vector3.Scale((Vector3.Scale(boxCol.size, transform.localScale)),resize);

        float timeElapsed = 0f;
        while(timeElapsed < freezeDuration)
        {
            Freezing(size);
            yield return null;
            timeElapsed += Time.deltaTime;
        }

        m_Animator.SetBool("Active", false);
    }

    public void DisableMesh()
    {
        iceFloorGO.SetActive(false);
    }

    private void Freezing(Vector3 size)
    {
        Collider[] cols = Physics.OverlapBox(transform.position + boxCol.center + (Vector3.up * offsetY), size / 2f, Quaternion.identity, playerMask);
        if (cols.Length > 0)
        {
            cols[0].GetComponent<HurtBox>().TakeStun(CCType.Type.Freeze, 3f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 size = Vector3.Scale((Vector3.Scale(boxCol.size, transform.lossyScale)), resize);
        //size.y = boxCol.center.y * 2f;
        Gizmos.DrawCube(transform.position + boxCol.center + (Vector3.up * offsetY), size);
    }
}
