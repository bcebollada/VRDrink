using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class Points3ModelCommunicator : RealtimeComponent<Points3Model>
{
    public int player3Points;

    protected override void OnRealtimeModelReplaced(Points3Model previousModel, Points3Model currentModel)
    {
        base.OnRealtimeModelReplaced(previousModel, currentModel);

        if (previousModel != null)
        {
            // Unregister from events
            previousModel.player3PointsDidChange -= PointsDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.player3Points = player3Points;

            }

            // Update the mesh render to match the new model
            UpdatePoints();

            // Register for events so we'll know if the color changes later
            currentModel.player3PointsDidChange += PointsDidChange;
        }
    }

    private void PointsDidChange(Points3Model mode, int intParameter)
    {
        UpdatePoints();
    }

    private void UpdatePoints()
    {
        player3Points = model.player3Points;
    }

    public void SetPoints(int pointsParameter)
    {
        model.player3Points = pointsParameter;
    }
}
