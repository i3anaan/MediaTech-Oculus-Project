using UnityEngine;
using System.Collections;

public class BulletShooter : MonoBehaviour, Lightable
{

    public ExpiringBullet bullet;
    public int fireRate;
    private int fireCharge;
    public float bulletSpeed;
    public float offsetPercentage;
    public float firingTime;
    public bool alwaysShootAtPlayer;
    public Transform player;
    private int firingTicks;

    public bool firing;


    void FixedUpdate()
    {
        if (firing)
        {
            firingTicks++;
            fireCharge++;

            int diff = fireCharge - fireRate;

            while (diff > 0)
            {
                fireBullet();
                diff--;
            }
            if (firingTime>0 && firingTicks * Time.fixedDeltaTime > firingTime)
            {
                firing = false;
            }
        }
    }

    private void fireBullet()
    {
        fireCharge = 0;
        if (alwaysShootAtPlayer)
        {
            this.transform.LookAt(player.position);
        }
        Quaternion rotation = Quaternion.Lerp(this.transform.rotation, Random.rotation, offsetPercentage);
        ExpiringBullet newBullet = Instantiate(bullet, this.transform.position, rotation) as ExpiringBullet;
        newBullet.transform.rotation = newBullet.transform.rotation*Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
        newBullet.rigidbody.velocity = newBullet.transform.forward * bulletSpeed;
        newBullet.transform.parent = this.transform;
        this.gameObject.audio.Play();
    }

    public void turnOnActive()
    {
        firing = true;
        firingTicks = 0;
    }

    public void turnOffActive()
    {
        firing = false;
    }

    public bool toggleStatus()
    {
        firing = !firing;
        return firing;
    }
}
