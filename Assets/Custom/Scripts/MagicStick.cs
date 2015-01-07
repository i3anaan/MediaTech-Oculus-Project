using UnityEngine;
using System.Collections;

public class MagicStick : MonoBehaviour
{

    public enum WandStates { Fired, Charging, Moving, Aiming };

    public FireBall fireBallPrefab;
    public bool forceSpawnFireBall;
    public WandStates wandState = WandStates.Fired;
    public int stabilizationDuration = 5;
    public int stabilizationPrecision = 5;
    public int maxFireBallSpeed = 1000;
    public int minFireBallSpeed = 100;
    public int maxCharge = 10;
    public float upness = 0.95F;
    public float horizontallness = 0.4F;

    private int tickCounter;
    private int charge;
    
    private Vector3[] prevAngles;

    void Awake()
    {
        prevAngles = new Vector3[stabilizationDuration];
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
            }
        }
        if (wandState == WandStates.Charging)
        {
            charge = Mathf.Min(maxCharge, charge + 1);
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
            charge--;
            if (isStabilized(prevAngles, stabilizationPrecision))
            {
                wandState = WandStates.Fired;
                float speed = (((float)charge) / ((float)maxCharge)) * (maxFireBallSpeed - minFireBallSpeed) + minFireBallSpeed;
                //Debug.Log("Launching fireball with speed: "+speed);
                spawnFireBall(speed);
                charge = 0;
            }
        }


        if (forceSpawnFireBall)
        {
            Debug.Log("Force shoot fireball");
            spawnFireBall(maxFireBallSpeed);
            forceSpawnFireBall = false;
        }
    }

    private bool isStabilized(Vector3[] arr, float precision)
    {
        bool output = true;
        for (int i = 0; i < arr.Length && output; i++)
        {
            if (Mathf.Abs(arr[i].z - arr[(i + 1) % stabilizationDuration].z) > precision)
            {
                output = false;
            }
        }
        return output;
    }

    public void spawnFireBall(float velocity)
    {
        Debug.Log("Fireball Speed: " + velocity);
        Debug.Log("Fireball shooting angle: "+vec3Average(prevAngles));
        FireBall newFireBall = Instantiate(fireBallPrefab, this.transform.position, Quaternion.Euler(vec3Average(prevAngles))) as FireBall;
        newFireBall.GetComponent<Rigidbody>().AddForce(this.transform.up * velocity);
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
