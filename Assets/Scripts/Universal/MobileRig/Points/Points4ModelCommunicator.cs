using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class Points4ModelCommunicator : RealtimeComponent<Points4Model>
{
    public int player4Points;

    protected override void OnRealtimeModelReplaced(Points4Model previousModel, Points4Model currentModel)
    {
        base.OnRealtimeModelReplaced(previousModel, currentModel);

        if (previousModel != null)
        {
            // Unregister from events
            previousModel.player4PointsDidChange -= PointsDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.player4Points = player4Points;

            }

            // Update the mesh render to match the new model
            UpdatePoints();

            // Register for events so we'll know if the color changes later
            currentModel.player4PointsDidChange += PointsDidChange;
        }
    }

    private void PointsDidChange(Points4Model mode, int intParameter)
    {
        UpdatePoints();
    }

    private void UpdatePoints()
    {
        player4Points = model.player4Points;
    }

    public void SetPoints(int pointsParameter)
    {
        model.player4Points = pointsParameter;
    }
}
