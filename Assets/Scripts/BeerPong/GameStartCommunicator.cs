using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class GameStartCommunicator : RealtimeComponent<GameStartModel>
{
    public bool gameStarted;

    protected override void OnRealtimeModelReplaced(GameStartModel previousModel, GameStartModel currentModel)
    {
        base.OnRealtimeModelReplaced(previousModel, currentModel);

        if (previousModel != null)
        {
            // Unregister from events
            previousModel.gameStartedDidChange -= BoolDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.gameStarted = gameStarted;

            }

            // Update the mesh render to match the new model
            UpdateBool();

            // Register for events so we'll know if the color changes later
            currentModel.gameStartedDidChange += BoolDidChange;
        }
    }

    private void BoolDidChange(GameStartModel mode, bool boolParameter)
    {
        UpdateBool();
    }

    private void UpdateBool()
    {
        gameStarted = model.gameStarted;
    }

    public void SetBool(bool boolParameter)
    {
        model.gameStarted = boolParameter;
    }
}
