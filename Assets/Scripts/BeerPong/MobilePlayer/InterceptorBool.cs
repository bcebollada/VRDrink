using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class InterceptorBool : RealtimeComponent<InterceptorBoolModel>
{
    public bool playAnimation;

    protected override void OnRealtimeModelReplaced(InterceptorBoolModel previousModel, InterceptorBoolModel currentModel)
    {
        base.OnRealtimeModelReplaced(previousModel, currentModel);

        if (previousModel != null)
        {
            // Unregister from events
            previousModel.playAnimationDidChange -= BoolDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
                currentModel.playAnimation = playAnimation;

            // Update the mesh render to match the new model
            UpdateBool();

            // Register for events so we'll know if the color changes later
            currentModel.playAnimationDidChange += BoolDidChange;
        }
    }

    private void BoolDidChange(InterceptorBoolModel mode, bool boolParameter)
    {
        UpdateBool();
    }

    private void UpdateBool()
    {
        playAnimation = model.playAnimation;
    }

    public void SetBool(bool boolParameter)
    {
        model.playAnimation = boolParameter;
    }


}
