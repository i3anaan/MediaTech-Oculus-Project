using UnityEngine;
using System.Collections;

public class MagicStick : MonoBehaviour
{

    public enum WandStates { Fired, Charging, Moving, Aiming };
    public enum BulletTypes {Water, Fire};

    public ExpiringBullet fireBallPrefab;
    public ExpiringBullet waterBallPrefab;
    public bool forceSpawnFireBall;
    public WandStates wandState = WandStates.Fired;
    public int stabilizationDuration = 5;
    public int stabilizationPrecision = 5;
    public int maxFireBallSpeed = 1000;
    public int minFireBallSpeed = 100;
    public int chargeSpeed = 3;
    public int maxCharge = 10;
    public float upness = 0.95F;
    public float horizontallness = 0.4F;

    private int tickCounter;
    private int charge;
    private BulletTypes bulletType;
    
    private Vector3[] prevAngles;

    void Awake()
    {
        prevAngles = new Vector3[100];
    }



    void FixedUpdate()
    {
        tickCounter++;
        prevAngles[tickCounter % stabilizationDuration] = this.transform.eulerAngles;
        if (wandState == WandStates.Fired)
        {
            if (this.transform.up.y > upness)
            {
                wandState = WandStates.Charging;
                charge = Mathf.Max(charge,0);
                bulletType = BulletTypes.Fire;
            }
            if (this.transform.up.y < -upness)
            {
                wandState = WandStates.Charging;
                charge = Mathf.Max(charge, 0);
                bulletType = BulletTypes.Water;
            }
        }
        if (wandState == WandStates.Charging)
        {
            charge = Mathf.Min(maxCharge, charge + chargeSpeed);
            if (Mathf.Abs(this.transform.up.y) < upness)
            {
                wandState = WandStates.Moving;
            }
        }
        if (wandState == WandStates.Moving)
        {
            if (Mathf.Abs(this.transform.up.y) < horizontallness)
            {
                wandState = WandStates.Aiming;
            }
            else if (Mathf.Abs(this.transform.up.y) > upness)
            {
                wandState = WandStates.Charging;
            }
        }
        if (wandState == WandStates.Aiming)
        {
            if (charge > 0)
            {
                charge--;
            }
            if (isStabilized(prevAngles, stabilizationPrecision))
            {
                wandState = WandStates.Fired;
                float speed = (((float)charge) / ((float)maxCharge)) * (maxFireBallSpeed - minFireBallSpeed) + minFireBallSpeed;
                Debug.Log("Fireball speed: " + speed);
                if (bulletType == BulletTypes.Fire)
                {
                    spawnExpiringBulet(fireBallPrefab,speed);
                }
                else if (bulletType == BulletTypes.Water)
                {
                    spawnExpiringBulet(waterBallPrefab, speed);
                }
                charge = 0;
            }
        }


        if (forceSpawnFireBall)
        {
            Debug.Log("Force shoot fireball");
            spawnExpiringBulet(fireBallPrefab,maxFireBallSpeed);
            forceSpawnFireBall = false;
        }
    }

    private bool isStabilized(Vector3[] arr, float precision)
    {
        bool output = true;
        for (int i = 0; i < stabilizationDuration && output; i++)
        {
            if (Mathf.Abs(arr[i].z - arr[(i + 1) % stabilizationDuration].z) > precision)
            {
                output = false;
            }
        }
        return output;
    }

    public void spawnExpiringBulet(ExpiringBullet bullet, float velocity)
    {
        ExpiringBullet newBullet = Instantiate(bullet, this.transform.position, Quaternion.Euler(vec3Average(prevAngles))) as ExpiringBullet;
        newBullet.GetComponent<Rigidbody>().AddForce(this.transform.up * velocity);
    }

    public static Vector3 vec3Average(Vector3[] arr)
    {
        Vector3 output = new Vector3();
        foreach (Vector3 v in arr)
        {
            output = output + v;
        }
        return output / arr.Length;
    }
}
