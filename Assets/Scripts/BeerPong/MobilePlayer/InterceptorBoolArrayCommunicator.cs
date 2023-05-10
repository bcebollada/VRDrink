using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class InterceptorBoolArrayCommunicator : RealtimeComponent<InterceptorBoolArrayModel>
{
    public bool activateInterceptor1;
    public bool activateInterceptor2;
    public bool activateInterceptor3;

    protected override void OnRealtimeModelReplaced(InterceptorBoolArrayModel previousModel, InterceptorBoolArrayModel currentModel)
    {
        base.OnRealtimeModelReplaced(previousModel, currentModel);

        if (previousModel != null)
        {
            // Unregister from events
            previousModel.activateInterceptor1DidChange -= BoolDidChange;
            previousModel.activateInterceptor2DidChange -= BoolDidChange;
            previousModel.activateInterceptor3DidChange -= BoolDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                currentModel.activateInterceptor1 = activateInterceptor1;
                currentModel.activateInterceptor2 = activateInterceptor2;
                currentModel.activateInterceptor3 = activateInterceptor3;

            }

            // Update the mesh render to match the new model
            UpdateBool();

            // Register for events so we'll know if the color changes later
            currentModel.activateInterceptor1DidChange += BoolDidChange;
            currentModel.activateInterceptor2DidChange += BoolDidChange;
            currentModel.activateInterceptor3DidChange += BoolDidChange;
        }
    }

    private void BoolDidChange(InterceptorBoolArrayModel mode, bool boolParameter)
    {
        UpdateBool();
    }

    private void UpdateBool()
    {
        activateInterceptor1 = model.activateInterceptor1;
        activateInterceptor2 = model.activateInterceptor2;
        activateInterceptor3 = model.activateInterceptor3;
    }

    public void SetBool(bool boolParameter, int interceptor)
    {
        if (interceptor == 1) model.activateInterceptor1 = boolParameter;
        else if (interceptor == 2) model.activateInterceptor2 = boolParameter;
        else if (interceptor == 3) model.activateInterceptor3 = boolParameter;
    }


}


