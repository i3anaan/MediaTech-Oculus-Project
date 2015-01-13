using UnityEngine;
using System.Collections;

public class FireWork : MonoBehaviour {
    public float fuseTime;
    public bool fuseLighted;
    private int fuseLightedTicks;
    public float explosionForce;
    public bool lightFuseOnStart;
    public float velocityDuringFuse;
    public AudioClip explosionSound;
    private ParticleSystem particleSystem;
    private bool exploded = false;    

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
                    child.GetComponent<Rigidbody>().AddForce(forceDir*explosionForce);
                }
            }
            particleSystem.Stop(false);
            this.audio.PlayOneShot(explosionSound,1f);
        }
        exploded = true;
		//Destroy (this);
    }

    public void lightFuse()
    {
        fuseLighted = true;
        particleSystem.Play(false);
    }
}
