using UnityEngine;

public class DustParticleController : MonoBehaviour
{
    public ParticleSystem dustParticle;

    public void CreateDust()
    {
        dustParticle.Play();
    }
}
