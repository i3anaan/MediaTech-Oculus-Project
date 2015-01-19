using UnityEngine;
using System.Collections;

public class FireWorkLauncher : MonoBehaviour, Lightable {

    public FireWork[] fireWorkToLaunch;
    public bool forceLaunch;
    public float launchEveryXSeconds;

    void Awake()
    {
        if (launchEveryXSeconds > 0)
        {
            Invoke("launchTimedFireWork", launchEveryXSeconds);
        }
    }

    void Update()
    {
        if (forceLaunch)
        {
            launchFireWork();
            forceLaunch = false;
        }
    }

    private void launchTimedFireWork(){
        launchFireWork();
        Invoke("launchTimedFireWork", launchEveryXSeconds);
    }

    private void launchFireWork()
    {
        FireWork newFireWork = Instantiate(fireWorkToLaunch[Random.Range(0, fireWorkToLaunch.Length)], this.transform.position, this.transform.rotation) as FireWork;
        newFireWork.lightFuse();
    }


    public void turnOnActive()
    {
        Debug.Log("FireWorkLauncher hit by fireball");
        launchFireWork();
    }

    public void turnOffActive()
    {
    }

    public bool toggleStatus()
    {
        return false;
    }
}
