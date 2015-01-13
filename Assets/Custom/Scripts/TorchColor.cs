using UnityEngine;
using System.Collections;
using UnityEditor;

public class TorchColor : Torch
{

    public GameObject[] torches;

    public int colorTicksMax;
    public int colorTicksLeft;

    public virtual void Awake()
    {
        base.Awake();
        torches = GameObject.FindGameObjectsWithTag("Torch");
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
        if (!base.fireStatus)
        {
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
    }
}
