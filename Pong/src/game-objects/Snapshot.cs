using System;
using Microsoft.Xna.Framework;

namespace mg_pong;

public class Snapshot {
    
    public float X;
    public float Y;
    public float Width;
    public float Height;

    protected ActorType actorType;

    public ActorType Type { get { return actorType; } }

    public Snapshot(Actor actor) {
        actorType = actor.ActorType;

        X = actor.X;
        Y = actor.Y;
        Width = actor.Width;
        Height = actor.Height;
    }
}
