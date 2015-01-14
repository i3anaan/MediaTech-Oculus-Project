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
                    child.rigidbody.velocity = new Vector3();
                    child.lightFuse();
                    Vector3 forceDir = Random.onUnitSphere;
                    forceDir.y = Mathf.Abs(forceDir.y);
                    child.GetComponent<Rigidbody>().AddForce(forceDir * explosionForce);
                }
            }
            this.audio.PlayOneShot(explosionSound, 1f);
            this.rigidbody.velocity = new Vector3();
        }
        exploded = true;
    }

    public void lightFuse()
    {
        fuseLighted = true;
        particleSystem.Play(false);
		this.rigidbody.velocity = this.transform.up*velocityDuringFuse;
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
