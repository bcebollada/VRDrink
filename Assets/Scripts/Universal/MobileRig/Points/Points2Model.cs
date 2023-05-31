using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;
using Normal.Realtime;

[RealtimeModel]

public partial class Points2Model
{
    [RealtimeProperty(1, true, true)]
    private int _player2Points;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class Points2Model : RealtimeModel {
    public int player2Points {
        get {
            return _player2PointsProperty.value;
        }
        set {
            if (_player2PointsProperty.value == value) return;
            _player2PointsProperty.value = value;
            InvalidateReliableLength();
            FirePlayer2PointsDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(Points2Model model, T value);
    public event PropertyChangedHandler<int> player2PointsDidChange;
    
    public enum PropertyID : uint {
        Player2Points = 1,
    }
    
    #region Properties
    
    private ReliableProperty<int> _player2PointsProperty;
    
    #endregion
    
    public Points2Model() : base(null) {
        _player2PointsProperty = new ReliableProperty<int>(1, _player2Points);
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        _player2PointsProperty.UnsubscribeCallback();
    }
    
    private void FirePlayer2PointsDidChange(int value) {
        try {
            player2PointsDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _player2PointsProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _player2PointsProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.Player2Points: {
                    changed = _player2PointsProperty.Read(stream, context);
                    if (changed) FirePlayer2PointsDidChange(player2Points);
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
        _player2Points = player2Points;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */