using UnityEngine;

public class ShadowController : MonoBehaviour
{
    [SerializeField]
    private float InitialScale;
    [SerializeField]
    public float FinalScale;
    public Vector3 scale;
    public Vector3 offset;

    public CharacterController character;
    // Start is called before the first frame update
    void Start()
    {
        character = gameObject.transform.parent.GetComponent<CharacterController>();
        scale = new Vector3(0, 0, transform.localScale.z);
        hit = new RaycastHit();
    }

    private RaycastHit hit;
    private float newDistance = 0f;
    private float newScale = 0f;

    public float rate;
    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(character.gameObject.transform.position, gameObject.transform.forward * 10f, out hit))
        {
            this.gameObject.transform.position = hit.point - offset;
            newDistance = Vector3.Distance(character.gameObject.transform.position, hit.point);
            ChangeScale(newDistance);
        } 
        //Debug.DrawRay(character.gameObject.transform.position, gameObject.transform.forward * 10f, Color.red);
    }

    public void ChangeScale(float newDistance)
    {
        rate = newDistance / 10f;
        newScale = Mathf.Lerp(FinalScale, InitialScale, rate);
        scale.x = newScale;
        scale.y = newScale;

        this.gameObject.transform.localScale = scale;
    }
}
