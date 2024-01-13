using System;
using Microsoft.Xna.Framework;

namespace mg_pong;

public class Actor: GameObject {
    private float xRemainder;
    private float yRemainder;

    protected ActorType actorType;

    public ActorType ActorType { get { return actorType; } }

    public Actor(): base(GameObjectType.Actor) {
        xRemainder = 0;
        yRemainder = 0;
    }

    public virtual Snapshot Snapshot() {
        return null;
    }

    public virtual void OnCollideActor(Snapshot other, float deltaTime) {}

    public void MoveX(float amount, Action<GameObject, Solid> onCollideSolid) {
        xRemainder += amount;
        int move = (int)xRemainder;

        if (move != 0) {
            xRemainder -= move;
            int sign = Math.Sign(move);

            while (move != 0) {
                X += sign;

                var solid = GetCollidingSolid();

                if (solid != null) { // Collided
                    X -= sign; // Roll back movement

                    if (onCollideSolid != null) {
                        onCollideSolid(this, solid);
                    }
                    break;
                }
                else 
                {
                    X += sign;
                    move -= sign;
                }
            }
        }
    }

    public void MoveY(float amount, Action<GameObject, Solid> onCollideSolid) {
        yRemainder += amount;
        int move = (int)yRemainder;

        if (move != 0) {
            yRemainder -= move;
            int sign = Math.Sign(move);

            while (move != 0) {
                Y += sign;

                var solid = GetCollidingSolid();

                if (solid != null) { // Collided
                    Y -= sign; // Roll back movement

                    if (onCollideSolid != null) {
                        onCollideSolid(this, solid);
                    }
                    break;
                }
                else 
                {
                    Y += sign;
                    move -= sign;
                }
            }
        }
    }

    private Solid GetCollidingSolid() {
        var solids = GameObjectManager.Solids;

        foreach (var solid in solids) {
            if (solid.Collides(Bounds)) {
                return solid;
            }
        }

        return null;
    }
}
