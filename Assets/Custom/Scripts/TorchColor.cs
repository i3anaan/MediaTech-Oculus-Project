using UnityEngine;
using System.Collections;
using UnityEditor;

public class TorchColor : Torch
{

    public GameObject[] torches;
    public Light light;

    public Color newColor;
    public float newIntensity;

    private Color previousColor;
    private float previousIntensity;

    public int colorTicksMax;
    public int colorTicksLeft;

    public virtual void Awake()
    {
        base.Awake();
        torches = GameObject.FindGameObjectsWithTag("Torch");
        previousColor = light.color;
        previousIntensity = light.intensity;
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

    public void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.GetComponent<Fireball>());
        if (!base.fireStatus && collider.gameObject.GetComponent<Fireball>()!=null){
            Debug.Log("Setting color 1");
            colorTicksLeft = colorTicksMax;
            setColor(1);
        }
    }


    private void setColor(int index)
    {
        Debug.Log("Setting particle system index: " + index);
        foreach (GameObject obj in torches)
        {
            Torch torch = obj.GetComponent<Torch>();
            if (torch != null)
            {
                torch.switchActiveParticles(index);
            }
        }
        if (index == 1)
        {

            light.color = newColor;
            light.intensity = newIntensity;
        }
        else if (index == 0)
        {
            light.color = previousColor;
            light.intensity = previousIntensity;
        }
    }
}
