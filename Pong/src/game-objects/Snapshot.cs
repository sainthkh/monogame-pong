using System;
using Microsoft.Xna.Framework;

namespace mg_pong;

public class Snapshot {
    protected ActorType actorType;

    public ActorType Type { get { return actorType; } }
}
