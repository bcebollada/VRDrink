using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;


public class Points2ModelCommunicator : RealtimeComponent<Points2Model>
{
    public int player2Points;

    protected override void OnRealtimeModelReplaced(Points2Model previousModel, Points2Model currentModel)
    {
        base.OnRealtimeModelReplaced(previousModel, currentModel);

        if (previousModel != null)
        {
            // Unregister from events
            previousModel.player2PointsDidChange -= PointsDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.player2Points = player2Points;

            }

            // Update the mesh render to match the new model
            UpdatePoints();

            // Register for events so we'll know if the color changes later
            currentModel.player2PointsDidChange += PointsDidChange;
        }
    }

    private void PointsDidChange(Points2Model mode, int intParameter)
    {
        UpdatePoints();
    }

    private void UpdatePoints()
    {
        player2Points = model.player2Points;
    }

    public void SetPoints(int pointsParameter)
    {
        model.player2Points = pointsParameter;
    }
}
