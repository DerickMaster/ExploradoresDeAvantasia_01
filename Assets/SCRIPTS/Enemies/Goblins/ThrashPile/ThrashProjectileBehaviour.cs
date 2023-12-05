using UnityEngine;

public class ThrashProjectileBehaviour : MonoBehaviour
{
    private readonly int dmg = 2;
    private float lifetime;
    public float speed;

    private void OnEnable()
    {
        lifetime = 0f;
    }

    private void OnDisable()
    {
        transform.position = Vector3.zero;
    }

    private void Update()
    {
        transform.position = transform.position + speed * Time.deltaTime * transform.forward;
        lifetime += Time.deltaTime;
        if (lifetime > 5f) gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            other.gameObject.GetComponent<HurtBox>().TakeDamage(dmg, gameObject);
        }
        gameObject.SetActive(false);
    }
}