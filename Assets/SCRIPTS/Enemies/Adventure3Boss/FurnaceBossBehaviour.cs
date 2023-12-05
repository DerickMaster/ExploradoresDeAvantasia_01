using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FurnaceBossBehaviour : EnemyBehaviour
{
    private LinkedList<GameObject> projectiles;
    private LinkedListNode<GameObject> curProjectile;

    [SerializeField] private int projectileAmount;
    [SerializeField] private StarterAssets.ThirdPersonController player;
    [SerializeField] GameObject FireBallPrefab;
    [SerializeField] GameObject mouth;

    [HideInInspector] public UnityEvent rageModeFinished;
    [HideInInspector] public UnityEvent pushFinished;

    [SerializeField] Transform knobackPoint; 

    new

        // Start is called before the first frame update
        void Start()
    {
        _myAnimator = GetComponent<Animator>();

        projectiles = new LinkedList<GameObject>();
        for (int i = 0; i < projectileAmount; i++)
        {
            GameObject projectile = Instantiate(FireBallPrefab, transform);
            projectiles.AddLast(projectile);
            projectile.SetActive(false);
        }
        curProjectile = projectiles.First;
        player = InteractionController.Instance.GetComponentInParent<StarterAssets.ThirdPersonController>();

        materialController = GetComponentInChildren<ObjectMaterialController>();
    }

    public void FinishedCooling()
    {
        player.TakeKnockback(knobackPoint.position - player.transform.position, 2f, 25, 0.4f);
        rageModeFinished.Invoke();
    }

    public void SetRageMode(bool raging)
    {
        _myAnimator.SetBool("Rage", raging);
    }

    public void StartShooting()
    {
        StartCoroutine(ShootingSequence(amount, 1));
    }
    
    public void StartPushing()
    {
        PushPlayer();
    }

    public float delay;
    public int additionalProjectiles;
    public int maxWaves;
    private IEnumerator ShootingSequence(float amount, int sequence)
    {
        this.amount = amount;

        yield return new WaitForSeconds(delay);
        _myAnimator.Play("Shoot");

        angle = 45f;
        angle += 4f * additionalProjectiles;
        if (sequence >= maxWaves) 
        {
            SetRageMode(false);
        }
        else StartCoroutine(ShootingSequence(amount + additionalProjectiles, sequence + 1));
    }

    public float angle;
    public float amount;
    public void ShootFireball()
    {
        
        for (int i = 0; i < amount; i++)
        {
            curProjectile = curProjectile.Next ?? projectiles.First;
            curProjectile.Value.SetActive(false);

            Quaternion rotation = Quaternion.LookRotation( Quaternion.AngleAxis( Mathf.Lerp(-angle, angle, i / (amount - 1) ) , Vector3.up) * transform.forward);
            curProjectile.Value.transform.rotation = new Quaternion(0f, rotation.y, 0f, rotation.w);
            curProjectile.Value.transform.position = mouth.transform.position;
            curProjectile.Value.SetActive(true);
        }
    }

    [ContextMenu("Push Player")]
    public void PushPlayer()
    {
        player.TakeKnockback(knobackPoint.position - player.transform.position, 2f, 25, 0.4f);
    }

    public void TakeDamage()
    {
        StartCoroutine(TakeDamageCoroutine());
    }

    private IEnumerator TakeDamageCoroutine()
    {
        materialController.AddMaterial(damagedMat);
        yield return new WaitForSeconds(2f);
        materialController.RemoveMaterial(damagedMat.name);
    }
}
