using UnityEngine;
using System.Collections;

public class ExpiringBullet : MonoBehaviour {

    public float lifeTime;
    private float aliveTicks = 0;
    public void FixedUpdate()
    {
        aliveTicks++;
        if (aliveTicks * Time.fixedDeltaTime > lifeTime)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
