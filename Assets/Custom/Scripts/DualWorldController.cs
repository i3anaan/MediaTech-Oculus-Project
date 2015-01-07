using UnityEngine;
using System.Collections;

public class DualWorldController : MonoBehaviour {

    public Camera mainCam;
    public Camera secondaryCam;
    public Camera missUsedCam;
    public GameObject referencePoint;
    public bool showDebugCut;

	// Update is called once per frame
	void Update () {
        secondaryCam.transform.localPosition = mainCam.transform.localPosition;
        missUsedCam.transform.position = mainCam.transform.position;

        secondaryCam.transform.localRotation = mainCam.transform.localRotation;
        missUsedCam.transform.rotation = mainCam.transform.rotation;

        Debug.Log(getCamScissorPercentage());

        Rect mainRect = new Rect();
        mainRect.x = 0;
        mainRect.y = 0;
        mainRect.width = getCamScissorPercentage();
        mainRect.height = 1;
        SetScissorRect(mainCam, mainRect);

        Rect secondaryRect = new Rect();
        secondaryRect.x = getCamScissorPercentage() + (showDebugCut ? 0.002f : 0f) ;
        secondaryRect.y = 0;
        secondaryRect.width = 1 - getCamScissorPercentage();
        secondaryRect.height = 1;
        SetScissorRect(secondaryCam, secondaryRect);
	}

    private float getCamScissorPercentage()
    {

        Vector3 deltaPos = referencePoint.transform.position - mainCam.transform.position;
        deltaPos = mainCam.transform.InverseTransformPoint(referencePoint.transform.position);
        Debug.Log("DeltaPos:  " + deltaPos);
        float angX = Mathf.Atan(deltaPos.x / deltaPos.z) /(2*Mathf.PI) * 360;
        Debug.Log("angX:  "+angX+ "\ncamRotationY:  " + mainCam.transform.eulerAngles.y);
        //return ((angX) / 90/2) + 0.5f;


        return missUsedCam.WorldToViewportPoint(referencePoint.transform.position).x;
    }


    public static void SetScissorRect(Camera cam, Rect r)
    {
        if (r.x < 0)
        {
            r.width += r.x;
            r.x = 0;
        }

        if (r.y < 0)
        {
            r.height += r.y;
            r.y = 0;
        }

        r.width = Mathf.Min(1 - r.x, r.width);
        r.height = Mathf.Min(1 - r.y, r.height);

        cam.rect = new Rect(0, 0, 1, 1);
        cam.ResetProjectionMatrix();
        Matrix4x4 m = cam.projectionMatrix;
        cam.rect = r;
        Matrix4x4 m1 = Matrix4x4.TRS(new Vector3(r.x, r.y, 0), Quaternion.identity, new Vector3(r.width, r.height, 1));
        Matrix4x4 m2 = Matrix4x4.TRS(new Vector3((1 / r.width - 1), (1 / r.height - 1), 0), Quaternion.identity, new Vector3(1 / r.width, 1 / r.height, 1));
        Matrix4x4 m3 = Matrix4x4.TRS(new Vector3(-r.x * 2 / r.width, -r.y * 2 / r.height, 0), Quaternion.identity, Vector3.one);
        cam.projectionMatrix = m3 * m2 * m;
    }
}
