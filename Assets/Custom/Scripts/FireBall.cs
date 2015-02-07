using UnityEngine;
using System.Collections;

public class FireBall : ExpiringBullet
{
    public void OnTriggerEnter(Collider collider)
    {
        foreach(MonoBehaviour mb in collider.GetComponentsInChildren<MonoBehaviour>())
        { 
            if (mb is Lightable)
            {
                ((Lightable)mb).turnOnActive();
            }
        }
        foreach (MonoBehaviour mb in collider.GetComponentsInParent<MonoBehaviour>())
        {
            if (mb is Lightable)
            {
                ((Lightable)mb).turnOnActive();
            }
        }
    }
}
