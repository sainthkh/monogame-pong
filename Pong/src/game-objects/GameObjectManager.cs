using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace mg_pong;

public static class GameObjectManager {
    private static readonly List<Actor> actors = new List<Actor>();
    private static readonly List<Solid> solids = new List<Solid>();

    public static void AddActor(Actor actor) {
        actors.Add(actor);
    }

    public static void RemoveActor(Actor actor) {
        actors.Remove(actor);
    }

    public static List<Actor> Actors {
        get {
            return actors;
        }
    }

    public static void AddSolid(Solid solid) {
        solids.Add(solid);
    }

    public static void RemoveSolid(Solid solid) {
        solids.Remove(solid);
    }

    public static List<Solid> Solids {
        get {
            return solids;
        }
    }
}
