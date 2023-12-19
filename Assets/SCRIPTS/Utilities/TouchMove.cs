using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TouchMove : MonoBehaviour, IPointerClickHandler,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    private Coroutine movementCoroutine;
    [SerializeField] LayerMask groundMask;
    StarterAssets.StarterAssetsInputs _charInput;
    Vector2 curScreenPosition;
    bool holding = false;

    private void Update()
    {
        _charInput = CharacterManager.Instance.GetCurrentCharacter().GetComponent<StarterAssets.StarterAssetsInputs>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        /*
        RaycastHit hit;
        Vector3 pos = new Vector3(eventData.pointerCurrentRaycast.screenPosition.x, eventData.pointerCurrentRaycast.screenPosition.y, 10f);
        Vector3 direction =  Camera.main.ScreenToWorldPoint(pos) - Camera.main.transform.position;
        //Debug.DrawRay(Camera.main.transform.position, direction * 100f, Color.blue,10f);
        //Debug.Log(Camera.main.ScreenToWorldPoint(pos));
        if (Physics.Raycast(Camera.main.transform.position, direction,out hit, 100f, groundMask))
        {
            direction = (hit.point - CharacterManager.Instance.GetCurrentCharacter().transform.position);

            if (movementCoroutine != null) StopCoroutine(movementCoroutine);
            movementCoroutine = StartCoroutine(MovementCoroutine(new Vector2(direction.x, direction.z).normalized, hit.point)); 

           //GetComponent<MoveCharacterInPath>().CalculatePath(hit.point);
        }

        IEnumerator MovementCoroutine(Vector2 direction, Vector3 endPoint)
        {
            _charInput = CharacterManager.Instance.GetCurrentCharacter().GetComponent<StarterAssets.StarterAssetsInputs>();
            while(Vector3.Distance(CharacterManager.Instance.GetCurrentCharacter().transform.position, endPoint) > 0.5f)
            {
                _charInput.move = direction;
                yield return null;
            }

            _charInput.move = Vector2.zero;
        }
         */
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        holding = true;
        curScreenPosition = eventData.pointerCurrentRaycast.screenPosition;
        StartCoroutine(MoveCharacter());
    }

    IEnumerator MoveCharacter()
    {
        Vector2 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
        Vector2 direction;
        while (holding)
        {
            direction =  curScreenPosition - screenCenter;
            _charInput.move = direction.normalized;
            yield return null;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        holding = false;
        _charInput.move = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        curScreenPosition = eventData.pointerCurrentRaycast.screenPosition;
    }
}
