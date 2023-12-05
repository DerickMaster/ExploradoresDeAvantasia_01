using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBucketBehaviour : GrabbableObject
{
    [SerializeField] private GameObject paintPrefab;
    private GameObject player;

    private new void Start()
    {
        obj_Grab_Drop.AddListener(ActivatePaint);
    }

    public override void Interact(InteractionController interactor)
    {
        base.Interact(interactor);
        
    }

    private void ActivatePaint(GrabbableObject obj)
    {
        this.grabbed = grabbed;
        if (grabbed) 
        {
            if (!player) player = GetComponentInParent<StarterAssets.ThirdPersonController>().gameObject;
;           StartCoroutine(SpawnPaint());
        } 
    }

    //private bool grabbed;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float paintDelay;
    private IEnumerator SpawnPaint()
    {
        RaycastHit hit;
        LayerMask puzzleObjLayer = LayerMask.NameToLayer("PuzzleObject");
        while(grabbed)
        {
            if(Physics.Raycast(player.transform.position, Vector3.down, out hit, 0.5f , mask) && !(hit.collider.gameObject.layer == puzzleObjLayer))
            {
                Instantiate(paintPrefab, hit.point, player.transform.rotation);
            }
            yield return new WaitForSeconds(paintDelay);
        }
    }
}
