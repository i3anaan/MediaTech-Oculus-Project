using UnityEngine;
using System.Collections;

public class FireWork : MonoBehaviour {

    public float fuseTime;
    public Color color;

    public bool fuseLighted;
    private int fuseLightedTicks;
    public float explosionForce;
    private ParticleSystem particleSystem;

    public bool lightFuseOnStart;

    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.startColor = color;
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
        }
    }

    private void explode()
    {
        FireWork[] children = GetComponentsInChildren<FireWork>();
        foreach (FireWork child in children)
        {
			Debug.Log ("Child transform: "+child.transform.parent);

            if (child != this && child.transform.parent != this.transform)
            {
				Debug.Log ("Child: "+child);
                child.lightFuse();
                child.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * explosionForce);
            }
        }
		//Destroy (particleSystem);
		Destroy (this);
    }

    public void lightFuse()
    {
        fuseLighted = true;
        particleSystem.Play(false);
    }
}
