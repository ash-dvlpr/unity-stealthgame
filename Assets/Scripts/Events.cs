using System;
using System.Collections;
using System.Collections.Generic;

using CinderUtils.Events;


public struct GameplayEvent : IEvent {
    public int id;
    public EventMetadata data;
}

public enum EventMetadata : byte {
    NONE        = 0,
    SETUP       = 1,
    OBJECTIVE   = 2,
    COLLECTIBLE = 3,
    TRIGGER     = 4,
}


public struct CinematicEvent : IEvent {
    public CinematicID id;
}

public enum CinematicID : byte { 
    NONE        = 0,
    VICTORY     = 1,
    DEFEAT      = 2,
}
