using UnityEngine;
using System.Collections;

public class Fireball : ExpiringBullet
{
    public void OnTriggerEnter(Collider collider)
    {
        MonoBehaviour mb = collider.GetComponent<MonoBehaviour>();
        if (mb is Lightable)
        {
            ((Lightable)mb).turnOnActive();
        }
    }
}
