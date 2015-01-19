using UnityEngine;
using System.Collections;

public class OVRCustomController : OptiTrackCorrector {

    public OVRCameraRig ovrCameraRigToCorrect;
    public bool oculusForceOverrideRotation;
	public bool oculusForceSubdueRotation;
    public float oculusOverrideRotationDelay;
    private bool oculusOverrideRotationActive;
    private Quaternion previousRotation;
    private float lastOptiTrackUpdate;

    /**
     * Oculus correct roteren bij setup van optitrack, dan altijd oculus rotatie gebruiken.
     *      Waarschijnlijk door recenterPose() on load uit te vinken.
     * 
     * RecenterPose() wanneer van override naar subdue
     * 
     */

    void Update()
    {
        if (!previousRotation.Equals(this.transform.rotation))
        {
            if (oculusOverrideRotationActive)
            {
                //From Override to subdue
                //OVRManager.display.RecenterPose();
                //TODO: try this
            }
            lastOptiTrackUpdate = Time.time;
            oculusOverrideRotationActive = false;
        }

        if (Time.time - lastOptiTrackUpdate > oculusOverrideRotationDelay)
        {
            oculusOverrideRotationActive = true;
        }
        if (oculusForceOverrideRotation || !oculusForceSubdueRotation)
        {
            oculusOverrideRotationActive = true;
        }
		if (!oculusForceOverrideRotation || oculusForceSubdueRotation)
		{
			oculusOverrideRotationActive = false;
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
