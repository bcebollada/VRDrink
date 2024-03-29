using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;
using Normal.Realtime;

[RealtimeModel]

public partial class Points1Model
{
    [RealtimeProperty(1, true, true)]
    private int _player1Points;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class Points1Model : RealtimeModel {
    public int player1Points {
        get {
            return _player1PointsProperty.value;
        }
        set {
            if (_player1PointsProperty.value == value) return;
            _player1PointsProperty.value = value;
            InvalidateReliableLength();
            FirePlayer1PointsDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(Points1Model model, T value);
    public event PropertyChangedHandler<int> player1PointsDidChange;
    
    public enum PropertyID : uint {
        Player1Points = 1,
    }
    
    #region Properties
    
    private ReliableProperty<int> _player1PointsProperty;
    
    #endregion
    
    public Points1Model() : base(null) {
        _player1PointsProperty = new ReliableProperty<int>(1, _player1Points);
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        _player1PointsProperty.UnsubscribeCallback();
    }
    
    private void FirePlayer1PointsDidChange(int value) {
        try {
            player1PointsDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _player1PointsProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _player1PointsProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.Player1Points: {
                    changed = _player1PointsProperty.Read(stream, context);
                    if (changed) FirePlayer1PointsDidChange(player1Points);
                    break;
                }
                default: {
                    stream.SkipProperty();
                    break;
                }
            }
            anyPropertiesChanged |= changed;
        }
        if (anyPropertiesChanged) {
            UpdateBackingFields();
        }
    }
    
    private void UpdateBackingFields() {
        _player1Points = player1Points;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */
