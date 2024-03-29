using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]

public partial class PlayersNumberModel
{
    [RealtimeProperty(1, true, true)]
    private int _numberOfPlayers;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class PlayersNumberModel : RealtimeModel {
    public int numberOfPlayers {
        get {
            return _numberOfPlayersProperty.value;
        }
        set {
            if (_numberOfPlayersProperty.value == value) return;
            _numberOfPlayersProperty.value = value;
            InvalidateReliableLength();
            FireNumberOfPlayersDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(PlayersNumberModel model, T value);
    public event PropertyChangedHandler<int> numberOfPlayersDidChange;
    
    public enum PropertyID : uint {
        NumberOfPlayers = 1,
    }
    
    #region Properties
    
    private ReliableProperty<int> _numberOfPlayersProperty;
    
    #endregion
    
    public PlayersNumberModel() : base(null) {
        _numberOfPlayersProperty = new ReliableProperty<int>(1, _numberOfPlayers);
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        _numberOfPlayersProperty.UnsubscribeCallback();
    }
    
    private void FireNumberOfPlayersDidChange(int value) {
        try {
            numberOfPlayersDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _numberOfPlayersProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _numberOfPlayersProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.NumberOfPlayers: {
                    changed = _numberOfPlayersProperty.Read(stream, context);
                    if (changed) FireNumberOfPlayersDidChange(numberOfPlayers);
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
        _numberOfPlayers = numberOfPlayers;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */
