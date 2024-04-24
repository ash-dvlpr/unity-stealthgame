using System;
using System.Collections;
using System.Collections.Generic;

using CinderUtils.Events;


// General gameplay events
public struct GameplayEvent : IEvent {
    public int id;
    public EventMetadata data;
}

public enum EventMetadata : byte {
    NONE                = 0,
    OBJECTIVE_SETUP     = 1,
    OBJECTIVE_COMPLETED = 2,
}




// Collectiles Events
public struct CollectibleEvent : IEvent {
    public Type ResourceType => collectible.ResourceType;
    public ICollectible<Resource> collectible;
}




// Events for cinematic stuff
public struct CinematicEvent : IEvent {
    public CinematicID id;
}

public enum CinematicID : byte { 
    NONE        = 0,
    VICTORY     = 1,
    DEFEAT      = 2,
}




// Events for cinematic stuff
public struct DayNightEvent : IEvent {
    public DayNight data;
}

public enum DayNight : byte {
    DAY = 0,
    NIGHT = 1,
}