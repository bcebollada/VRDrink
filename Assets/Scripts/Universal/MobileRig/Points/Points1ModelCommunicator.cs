using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class Points1ModelCommunicator : RealtimeComponent<Points1Model>
{
    public int player1Points;

    protected override void OnRealtimeModelReplaced(Points1Model previousModel, Points1Model currentModel)
    {
        base.OnRealtimeModelReplaced(previousModel, currentModel);

        if (previousModel != null)
        {
            // Unregister from events
            previousModel.player1PointsDidChange -= PointsDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.player1Points = player1Points;

            }

            // Update the mesh render to match the new model
            UpdatePoints();

            // Register for events so we'll know if the color changes later
            currentModel.player1PointsDidChange += PointsDidChange;
        }
    }

    private void PointsDidChange(Points1Model mode, int intParameter)
    {
        UpdatePoints();
    }

    private void UpdatePoints()
    {
        player1Points = model.player1Points;
    }

    public void SetPoints(int pointsParameter)
    {
        model.player1Points = pointsParameter;
    }
}
