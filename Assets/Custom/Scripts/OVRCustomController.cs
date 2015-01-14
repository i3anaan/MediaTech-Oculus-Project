using UnityEngine;
using System.Collections;

public class OVRCustomController : OptiTrackCorrector {

    public OVRCameraRig ovrCameraRigToCorrect;
    public bool oculusForceOverrideRotation;
    public float oculusOverrideRotationDelay;
    private bool oculusOverrideRotationActive;
    private Quaternion previousRotation;
    private float lastOptiTrackUpdate;

    void Update()
    {
        if (!previousRotation.Equals(this.transform.rotation))
        {
            lastOptiTrackUpdate = Time.time;
            oculusOverrideRotationActive = false;
        }

        if (Time.time - lastOptiTrackUpdate > oculusOverrideRotationDelay)
        {
            oculusOverrideRotationActive = true;
        }
        if (oculusForceOverrideRotation)
        {
            oculusOverrideRotationActive = true;
        }
    }



    public override void LateUpdate()
    {
        applyTranslation();
        if (oculusOverrideRotationActive)
        {
            OVRPose leftEye = OVRManager.display.GetEyePose(OVREye.Left);
            OVRPose rightEye = OVRManager.display.GetEyePose(OVREye.Right);
            ovrCameraRigToCorrect.setRotation(leftEye.orientation, leftEye.orientation, rightEye.orientation);
            gameObjectToCorrect.transform.rotation = Quaternion.identity;
        }
        else
        {
            applyRotation();
        }
    }
}
