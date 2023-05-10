using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class CubeScale : RealtimeComponent<CubeScaleModel>
{
    protected override void OnRealtimeModelReplaced(CubeScaleModel previousModel, CubeScaleModel currentModel)
    {
        base.OnRealtimeModelReplaced(previousModel, currentModel);

        if (previousModel != null)
        {
            // Unregister from events
            previousModel.scaleDidChange -= ScaleDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
                currentModel.scale = transform.localScale;

            // Update the mesh render to match the new model
            UpdateScale();

            // Register for events so we'll know if the color changes later
            currentModel.scaleDidChange += ScaleDidChange;
        }
    }

    private void ScaleDidChange(CubeScaleModel mode, Vector3 scale)
    {
        UpdateScale();
    }

    private void UpdateScale()
    {
        transform.localScale = model.scale;
    }

    public void SetScale(Vector3 scale)
    {
        model.scale = scale;
    }

    private void Update()
    {
    }
}
