using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;


public class CutTheCupPointsCommunicator : RealtimeComponent<CutTheCupPointsModel>
{
    public int mobilePoints;
    public int VRPoints;

    protected override void OnRealtimeModelReplaced(CutTheCupPointsModel previousModel, CutTheCupPointsModel currentModel)
    {
        base.OnRealtimeModelReplaced(previousModel, currentModel);

        if (previousModel != null)
        {
            // Unregister from events
            previousModel.mobilePointsDidChange -= PointsDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.mobilePoints = mobilePoints;
                currentModel.vrPoints = VRPoints;

            }

            // Update the mesh render to match the new model
            UpdateBool();

            // Register for events so we'll know if the color changes later
            currentModel.mobilePointsDidChange += PointsDidChange;
            currentModel.vrPointsDidChange += PointsDidChange;
        }
    }

    private void PointsDidChange(CutTheCupPointsModel mode, int points)
    {
        UpdateBool();
    }

    private void UpdateBool()
    {
        mobilePoints = model.mobilePoints;
        VRPoints = model.vrPoints;
    }

    public void AddMobPoints(int mobilePoints)
    {
        model.mobilePoints += mobilePoints;
    }

    public void AddPointsVR(int points)
    {
        model.vrPoints += points;
    }
}
