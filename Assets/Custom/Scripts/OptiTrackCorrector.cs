using UnityEngine;
using System.Collections;

public class OptiTrackCorrector : MonoBehaviour
{

    public GameObject gameObjectToCorrect;

    private Vector3 startPosition;
    private Quaternion startRotation;

    public bool useRelativePosition;
    public bool useRelativeRotation;
    public float smoothness; //Keep between 0 and 1


    public virtual void Awake()
    {
        startPosition = gameObjectToCorrect.transform.position;
        startRotation = gameObjectToCorrect.transform.rotation;
    }

    public virtual void LateUpdate()
    {
        applyTranslation();
        applyRotation();
    }

    protected virtual void applyTranslation()
    {
        Vector3 newPosition = this.transform.position;
        if (useRelativePosition)
        {
            newPosition = newPosition + startPosition;
        }
        gameObjectToCorrect.transform.position = Vector3.Lerp(gameObjectToCorrect.transform.position, newPosition, 1 - smoothness);
    }

    protected virtual void applyRotation()
    {
        Quaternion newRotation = this.transform.rotation;
        if (useRelativeRotation)
        {
            newRotation = newRotation * startRotation;
        }
        gameObjectToCorrect.transform.rotation = Quaternion.Slerp(gameObjectToCorrect.transform.rotation, newRotation, 1 - smoothness);
    }
}
