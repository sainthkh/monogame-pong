using System;
using Microsoft.Xna.Framework;

namespace mg_pong;

public class Actor: GameObject {
    private float xRemainder;
    private float yRemainder;

    public Actor(): base(GameObjectType.Actor) {
        xRemainder = 0;
        yRemainder = 0;
    }

    public void MoveX(float amount, Action<GameObject, GameObject> onCollide) {
        xRemainder += amount;
        int move = (int)xRemainder;

        if (move != 0) {
            xRemainder -= move;
            int sign = Math.Sign(move);

            while (move != 0) {
                Bounds.X += sign;

                var obj = GetCollidingGameObject();

                if (obj != null) { // Collided
                    Bounds.X -= sign; // Roll back movement

                    if (onCollide != null) {
                        onCollide(this, obj);
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

    public void MoveY(float amount, Action<GameObject, GameObject> onCollide) {
        yRemainder += amount;
        int move = (int)yRemainder;

        if (move != 0) {
            yRemainder -= move;
            int sign = Math.Sign(move);

            while (move != 0) {
                Bounds.Y += sign;

                var obj = GetCollidingGameObject();

                if (obj != null) { // Collided
                    Bounds.Y -= sign; // Roll back movement

                    if (onCollide != null) {
                        onCollide(this, obj);
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

    public GameObject GetCollidingGameObject() {
        var objs = GameObjectManager.Objects;

        foreach (var obj in objs) {
            if (obj.Collides(Bounds)) {
                return obj;
            }
        }

        return null;
    }
}
