using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class MiniGamesPlayedCommunicator : RealtimeComponent<MiniGameNumberModel>
{
    public int miniGamesPlayed;

    protected override void OnRealtimeModelReplaced(MiniGameNumberModel previousModel, MiniGameNumberModel currentModel)
    {
        base.OnRealtimeModelReplaced(previousModel, currentModel);

        if (previousModel != null)
        {
            // Unregister from events
            previousModel.miniGamesPlayedDidChange -= SceneDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.miniGamesPlayed = miniGamesPlayed;

            }

            // Update the mesh render to match the new model
            UpdateScene();

            // Register for events so we'll know if the color changes later
            currentModel.miniGamesPlayedDidChange += SceneDidChange;
        }
    }

    private void SceneDidChange(MiniGameNumberModel mode, int intParameter)
    {
        UpdateScene();
    }

    private void UpdateScene()
    {
        miniGamesPlayed = model.miniGamesPlayed;
    }

    public void SetMiniGamesPlayed(int miniGamesPlayed)
    {
        model.miniGamesPlayed = miniGamesPlayed;
    }
}
