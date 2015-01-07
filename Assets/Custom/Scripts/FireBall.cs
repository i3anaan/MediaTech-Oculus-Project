using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

    public float lifeTime;
    private float aliveTicks = 0;

    public void Awake()
    {
    }

    public void FixedUpdate()
    {
        aliveTicks++;
        if (aliveTicks * Time.fixedDeltaTime > lifeTime)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        MonoBehaviour mb = collider.GetComponent<MonoBehaviour>();
        if (mb is Lightable)
        {
            ((Lightable) mb).turnOn();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("FireBall collision: " + collision.collider);
        //GameObject.Destroy(this.gameObject);
    }
}
