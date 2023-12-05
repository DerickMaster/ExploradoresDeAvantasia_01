using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Minigame08LetterCube : EnemyBehaviour
{
    MeshFilter _currentVowelMesh;
    public bool _correctAnswer;
    private HurtBox m_HurtBox;

    [HideInInspector] public UnityEvent<Minigame08LetterCube> refUnitKilled;
    [SerializeField] private GameObject explosion;

    private void Awake()
    {
        _currentVowelMesh = GetComponentInChildren<MeshFilter>();
    }

    private new void Start()
    {
        base.Start();
        m_HurtBox = GetComponentInChildren<HurtBox>();
        unitKilled.AddListener(DeathEvent);
        _myAnimator.SetBool("Active", true);
    }

    public string myChar = "0";
    public void UpdateChar(string newChar, Mesh newMesh, bool answer)
    {
        _correctAnswer = answer;
        myChar = newChar;
        _currentVowelMesh.mesh = newMesh;
    }

    [ContextMenu("Activate crystal")]
    public void ActivateCrystal()
    {
        _myAnimator.SetBool("Active", true);
        
        curHealthAmount = maxHealthAmount;

        StartCoroutine(ActivateHurtBox(1f));
    }

    private IEnumerator ActivateHurtBox(float delay)
    {
        yield return new WaitForSeconds(delay);
        m_HurtBox.SetOpenHurtbox(true);
    }

    private void DeathEvent()
    {
        _myAnimator.SetBool("Active", false);
        _myAnimator.SetTrigger("Explode");

        if (!_correctAnswer) Explode();

        refUnitKilled.Invoke(this);
    }

    public void DisableCube()
    {
        _myAnimator.SetBool("Active", false);
        m_HurtBox.SetOpenHurtbox(false);
        //_currentVowelMesh.gameObject.SetActive(false);
    }

    bool exploded = false;
    private void Explode()
    {
        exploded = true;
        ExplosionBehaviour instance = Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<ExplosionBehaviour>();
        instance.PlayExplosion(2, 5f);
    }

    public override void Die()
    {
        unitKilled.Invoke();
    }
}
