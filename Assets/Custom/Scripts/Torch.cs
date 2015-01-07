using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour,Lightable {

    public bool toggleFire;
    private bool fireStatus;
    public ParticleSystem fireParticles;
    public Light torchLight;

	void FixedUpdate () {
        if (toggleFire)
        {
            toggleStatus();
            toggleFire = false;
        }
	}

    public void turnOn()
    {
        fireParticles.Play(true);
        torchLight.gameObject.SetActive(true);
    }

    public void turnOff()
    {
        fireParticles.Stop(true);
        torchLight.gameObject.SetActive(false);
    }

    public bool toggleStatus()
    {
        if (fireParticles.isPlaying)
        {
            turnOff();
            return false;
        }
        else
        {
            turnOn();
            return true;
        }
    }
}
