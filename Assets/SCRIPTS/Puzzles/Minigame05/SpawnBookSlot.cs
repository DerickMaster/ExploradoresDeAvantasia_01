using UnityEngine;
using UnityEngine.Events;

public class SpawnBookSlot : MonoBehaviour
{
    [SerializeField] private GameObject bookPilePrefab;
    [SerializeField] private Material bookMat;
    [SerializeField] private int bookId;

    [HideInInspector] public UnityEvent<bool,int> bookPickedUpEvent;

    public void SpawnPrefab(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(bookPilePrefab, transform.position, Quaternion.identity);
            obj.GetComponentInChildren<MeshRenderer>().material = bookMat;
            GrabbableObject objRef = obj.GetComponent<GrabbableObject>();

            objRef.objectID = bookId;
            objRef.objDestroyedEvent.AddListener(ObjRespawn);
            objRef.obj_Grab_Drop.AddListener(ActivateSlotEvent);
        }
    }

    private void ObjRespawn(GameObject obj)
    {
        SpawnPrefab(1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position, Vector3.one);
    }

    private void ActivateSlotEvent(GrabbableObject obj)
    {
        bookPickedUpEvent.Invoke(obj.grabbed,obj.objectID);
    }
}
