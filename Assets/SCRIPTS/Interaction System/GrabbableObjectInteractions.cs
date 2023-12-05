using UnityEngine;
using System.Collections;
public class GrabbableObjectInteractions
{
    // Recebe a referencia do interactor do jogador e do objeto que deve ser entregado
    // tenta entregar pro jogador usando a funcao HoldObjeto do interactor, caso consiga
    // retorna null indicando que objeto foi entregue, caso nao consiga retorna o proprio objeto 
    // indicando que nao foi possivel entregar
    public static GameObject GiveItem(InteractionController interactor, GameObject objectToGive)
    {
        if (interactor.HoldObject(objectToGive)) return null;
        else return objectToGive;
    }

    //
    public static GameObject ReceiveItem(InteractionController interactor, GameObject objectSlot = null, Vector3 posOffset = default)
    {
        GameObject HeldObject = interactor.PutDownObject();
        if (objectSlot)
        {
            HeldObject.transform.SetParent(objectSlot.transform, true);
            HeldObject.transform.localPosition = Vector3.zero + posOffset;
            HeldObject.transform.localRotation = Quaternion.identity;
        }
        else HeldObject.transform.SetParent(null);

        return HeldObject;
    }

    public static IEnumerator ThrownThenMove(GameObject obj, GameObject point, Vector3 direction, float throwForce = 500f, float delay = 1f)
    {
        if (direction.sqrMagnitude == 0) direction = Vector3.up;
            
        obj.GetComponentInChildren<GrabbableObject>().SetPhysics(true);
        obj.GetComponent<Rigidbody>().AddForce(direction * throwForce);

        float time = 0f;
        while(time < delay)
        {
            yield return null;
            time += Time.deltaTime;
        }

        obj.transform.position = point.transform.position;
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;

    }

    public static void SwallowItem(InteractionController interactor)
    {
        GameObject HeldObject = interactor.PutDownObject();
        HeldObject.GetComponent<GrabbableObject>().GotSwallowed();
        Object.Destroy(HeldObject);
    }
}
