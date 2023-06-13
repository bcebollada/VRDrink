using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;


public class PlayersNumberCommunicator : RealtimeComponent<PlayersNumberModel>
{
    public int numberOfPlayers;

    protected override void OnRealtimeModelReplaced(PlayersNumberModel previousModel, PlayersNumberModel currentModel)
    {
        base.OnRealtimeModelReplaced(previousModel, currentModel);

        if (previousModel != null)
        {
            // Unregister from events
            previousModel.numberOfPlayersDidChange -= NumberDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.numberOfPlayers = numberOfPlayers;

            }

            // Update the mesh render to match the new model
            UpdateNumbers();

            // Register for events so we'll know if the color changes later
            currentModel.numberOfPlayersDidChange += NumberDidChange;
        }
    }

    private void NumberDidChange(PlayersNumberModel mode, int intParameter)
    {
        UpdateNumbers();
    }

    private void UpdateNumbers()
    {
        numberOfPlayers = model.numberOfPlayers;
    }

    public void SetNumber(int number)
    {
        model.numberOfPlayers = number;
    }
}
