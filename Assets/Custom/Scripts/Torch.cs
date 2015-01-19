using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour, Lightable
{

    public bool toggleFire;
    protected bool fireStatus;
    public ParticleSystem[] particles;
    public Light torchLight;
    public int activeParticleSystem;
    private Observer observer;


    public virtual void Awake()
    {
        activeParticleSystem = 0;
        particles = this.gameObject.GetComponentsInChildren<ParticleSystem>();
        turnOffAllBut(activeParticleSystem);
    }

    public void switchActiveParticles(int index)
    {
        if (index >= 0 && index < particles.Length)
        {
            activeParticleSystem = index;
            bool fireStatusStore = fireStatus;
            turnOffAllBut(activeParticleSystem);
            fireStatus = fireStatusStore;
            if (fireStatus) { turnOnActive(); } else { turnOffActive(); }
        }
        
    }

    private void turnOffAllBut(int index)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            if (i != index)
            {
                turnOff(i);
            }
        }
    }

    void FixedUpdate()
    {
        if (toggleFire)
        {
            fireStatus = toggleStatus();
            toggleFire = false;
        }
    }

    public void turnOnActive()
    {
        if (!fireStatus)
        {
            if (observer != null)
            {
                observer.update(this);
            }
        }
        turnOn(activeParticleSystem);
    }

    private void turnOn(int index)
    {
        particles[index].Play(true);
        torchLight.gameObject.SetActive(true);
        fireStatus = true;
    }

    public void setObserver(Observer obs)
    {
        this.observer = obs;
    }

    public void turnOffActive()
    {
        turnOff(activeParticleSystem);
    }

    private void turnOff(int index)
    {
        particles[index].Stop(true);
        torchLight.gameObject.SetActive(false);
        fireStatus = false;
    }

    public bool toggleStatus()
    {
        if (fireStatus)
        {
            turnOffActive();
            return false;
        }
        else
        {
            turnOnActive();
            return true;
        }
    }
}
