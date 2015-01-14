using UnityEngine;
using System.Collections;

public class WaterBall : ExpiringBullet
{
    public void OnTriggerEnter(Collider collider)
    {
        MonoBehaviour mb = collider.GetComponent<MonoBehaviour>();
        if (mb is Lightable)
        {
            ((Lightable)mb).turnOffActive();
        }
        else
        {
            mb = collider.GetComponentInParent<MonoBehaviour>();
            if (mb is Lightable)
            {
                ((Lightable)mb).turnOffActive();
            }
        }
    }
}
