using System;
using System.Collections;
using System.Collections.Generic;

using CinderUtils.Events;


public struct GameplayEvent : IEvent {
    public int id;
    public EventKind kind;
}

public enum EventKind : byte {
    NONE        = 0,
    SETUP       = 1,
    OBJECTIVE   = 2,
    COLLECTIBLE = 3,
}
