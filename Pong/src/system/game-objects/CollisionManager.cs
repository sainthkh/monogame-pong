using System;
using System.Collections.Generic;

namespace mg_pong;

public struct Collision {
    public Actor a;
    public Actor b;
}

public static class CollisionManager {
    private static Dictionary<int, Snapshot> snapshots = new Dictionary<int, Snapshot>();
    private static List<Collision> collisions = new List<Collision>();

    public static void AddCollision(Actor a, Actor b) {
        AddSnapshot(a);
        AddSnapshot(b);

        collisions.Add(new Collision {
            a = a,
            b = b,
        });
    }

    public static void HandleCollisions(float deltaTime) {
        foreach (var collision in collisions) {
            collision.a.OnCollideActor(snapshots[collision.b.Id], deltaTime);
            collision.b.OnCollideActor(snapshots[collision.a.Id], deltaTime);
        }
    }

    public static void Clear() {
        snapshots.Clear();
        collisions.Clear();
    }

    private static void AddSnapshot(Actor actor) {
        snapshots[actor.Id] = actor.Snapshot();
    }
}
