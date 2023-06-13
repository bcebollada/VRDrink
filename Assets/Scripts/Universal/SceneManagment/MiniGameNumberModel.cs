using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;
using Normal.Realtime;

[RealtimeModel]


public partial class MiniGameNumberModel
{
    [RealtimeProperty(1, true, true)]
    private int _miniGamesPlayed;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class MiniGameNumberModel : RealtimeModel {
    public int miniGamesPlayed {
        get {
            return _miniGamesPlayedProperty.value;
        }
        set {
            if (_miniGamesPlayedProperty.value == value) return;
            _miniGamesPlayedProperty.value = value;
            InvalidateReliableLength();
            FireMiniGamesPlayedDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(MiniGameNumberModel model, T value);
    public event PropertyChangedHandler<int> miniGamesPlayedDidChange;
    
    public enum PropertyID : uint {
        MiniGamesPlayed = 1,
    }
    
    #region Properties
    
    private ReliableProperty<int> _miniGamesPlayedProperty;
    
    #endregion
    
    public MiniGameNumberModel() : base(null) {
        _miniGamesPlayedProperty = new ReliableProperty<int>(1, _miniGamesPlayed);
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        _miniGamesPlayedProperty.UnsubscribeCallback();
    }
    
    private void FireMiniGamesPlayedDidChange(int value) {
        try {
            miniGamesPlayedDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _miniGamesPlayedProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _miniGamesPlayedProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.MiniGamesPlayed: {
                    changed = _miniGamesPlayedProperty.Read(stream, context);
                    if (changed) FireMiniGamesPlayedDidChange(miniGamesPlayed);
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
        _miniGamesPlayed = miniGamesPlayed;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */
