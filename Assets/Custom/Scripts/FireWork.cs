using UnityEngine;
using System.Collections;

public class FireWork : MonoBehaviour, Lightable  {
    public float fuseTime;
    public bool fuseLighted;
    private int fuseLightedTicks;
    public float explosionForce;
    public bool lightFuseOnStart;
    public float velocityDuringFuse;
    public AudioClip explosionSound;
    private ParticleSystem particleSystem;
    private bool exploded = false;

    public float particleEmitDuration;
    public float particleEmitFadeStartPercentage;

    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Stop(true);
        if (lightFuseOnStart)
        {
            lightFuse();
        }
        this.rigidbody.velocity = this.transform.up*velocityDuringFuse;
    }

    void FixedUpdate()
    {
        if (fuseLighted)
        {
            fuseLightedTicks++;
            if (fuseLightedTicks * Time.fixedDeltaTime > fuseTime)
            {
                explode();
            }


            float percentage = (fuseLightedTicks * Time.fixedDeltaTime) / particleEmitDuration;
            float adjustedPercentage = (percentage - particleEmitFadeStartPercentage) / (1 - particleEmitFadeStartPercentage);
            //Debug.Log("Percentage: " + percentage + "  AdjustedPercentage: " + adjustedPercentage);
            if (adjustedPercentage > 0)
            {
                Color color = particleSystem.startColor;
                color.a = 1 - adjustedPercentage;
                particleSystem.startColor = color;
            }
            if (percentage > 2 && exploded)
            {
                Destroy(particleSystem);
                Destroy(this);
            }
        }

        
    }

    private void explode()
    {
        if (!exploded)
        {
            FireWork[] children = GetComponentsInChildren<FireWork>();
            foreach (FireWork child in children)
            {
                if (child != this && child.transform.parent == this.transform)
                {
                    
                    child.transform.localPosition = new Vector3();
                    child.lightFuse();
                    Vector3 forceDir = Random.onUnitSphere;
                    forceDir.y = Mathf.Abs(forceDir.y);

                    Debug.Log(this.transform + "Lighting fuse for: " + child + " Dir: "+forceDir);
                    child.GetComponent<Rigidbody>().AddForce(forceDir * explosionForce);
                }
            }
            particleSystem.Stop(false);
            this.audio.PlayOneShot(explosionSound, 1f);
        }
        exploded = true;
    }

    public void lightFuse()
    {
        fuseLighted = true;
        particleSystem.Play(false);
    }

    public void turnOnActive()
    {
        lightFuse();
    }

    public void turnOffActive()
    {
    }

    public bool toggleStatus()
    {
        return false;
    }
}
