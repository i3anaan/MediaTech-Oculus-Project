using UnityEngine;
using System.Collections;

public class Fireball : ExpiringBullet
{
    public void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collider: " + collider.gameObject.name);
        MonoBehaviour mb = collider.GetComponent<MonoBehaviour>();
        if (mb is Lightable)
        {
            Debug.Log("First MB is Lightable: " + mb);
            ((Lightable)mb).turnOnActive();
        }
        else
        {
            Debug.Log("First MB was not Lightable: " + mb);
            mb = collider.GetComponentInParent<MonoBehaviour>();
            if (mb is Lightable)
            {
                ((Lightable)mb).turnOnActive();
            }
        }
    }
}
