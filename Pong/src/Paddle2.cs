using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace mg_pong;

public class Paddle2: Actor {
    public const int INITIAL_WIDTH = 60;
    public const int HEIGHT = 20;

    public float Speed { get; set; }

    public Paddle2(int y) {

        X = GameBounds.X / 2 - INITIAL_WIDTH / 2;
        Y = y;

        Bounds = new Rectangle(X, Y, INITIAL_WIDTH, HEIGHT);
    }
}

public class PlayerSnapshot: Snapshot {
    public List<float> CornerDegrees;

    public PlayerSnapshot(Paddle2Player player): base(player) {}
}

public class Paddle2Player: Paddle2 {
    private List<float> cornerDegrees;

    public Paddle2Player(): base(GameBounds.Y - 80) {
        Speed = 250;
        actorType = ActorType.Player;

        cornerDegrees = MathUtil.CornerDegrees(Bounds);
    }

    public override Snapshot Snapshot()
    {
        var snapshot = new PlayerSnapshot(this);
        snapshot.CornerDegrees = cornerDegrees;
        
        return snapshot;
    }

    public void Move(float deltaTime) {
        KeyboardState keyboardState = Keyboard.GetState();
        
        if (keyboardState.IsKeyDown(Keys.Left)) {
            MoveX(-Speed * deltaTime, OnCollide);
        }
        else if (keyboardState.IsKeyDown(Keys.Right)) {
            MoveX(Speed * deltaTime, OnCollide);
        }
    }

    public void Render() {
        Draw.Rectangle(Bounds, Color.White);
    }

    public void OnCollide(GameObject paddle, Solid solid) {

    }
}
