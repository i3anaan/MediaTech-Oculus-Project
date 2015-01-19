using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class ColorController : MonoBehaviour, Observer
{
    public GameObject[] torches;
    public Light light;
    public Color[] colors;
    public float[] intensities;
    public Torch[] torchTriggers;

    

    public int colorTicksMax;
    public int colorTicksLeft;

    public virtual void Awake()
    {
        torches = GameObject.FindGameObjectsWithTag("Torch");
        colors[0] = light.color;
        intensities[0] = light.intensity;

        foreach (Torch t in torchTriggers)
        {
            t.setObserver(this);
        }
    }

    public void FixedUpdate()
    {
        if (colorTicksLeft > 0)
        {
            colorTicksLeft--;

            if (colorTicksLeft == 0)
            {
                setColor(0);
            }
        }
    }


    private void setColor(int index)
    {
        //-1 for default colors
        Debug.Log("Setting particle system index: " + index);
        if (index > 0)
        {
            colorTicksLeft = colorTicksMax;
        }
        foreach (GameObject obj in torches)
        {
            Torch torch = obj.GetComponent<Torch>();
            if (torch != null)
            {
                torch.switchActiveParticles(index);
            }
        }
        if (index >= 0)
        {
            light.color = colors[index];
            light.intensity = intensities[index];
        }
    }

    public void update(System.Object observable)
    {
        Debug.Log("ColorController update!");
        //A trigger torch got turned on.
        Torch torch = (Torch)observable;
        int index = Array.IndexOf<Torch>(torchTriggers, torch) + 1;
        setColor(index);
    }
}
