using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

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

public class Paddle2Player: Paddle2 {
    public Paddle2Player(): base(GameBounds.Y - 80) {
        Speed = 250;
    }

    public void Update(GameTime gameTime) {
        KeyboardState keyboardState = Keyboard.GetState();
        
        if (keyboardState.IsKeyDown(Keys.Left)) {
            MoveX(-Speed * (float)gameTime.ElapsedGameTime.TotalSeconds, OnCollide);
        }
        else if (keyboardState.IsKeyDown(Keys.Right)) {
            MoveX(Speed * (float)gameTime.ElapsedGameTime.TotalSeconds, OnCollide);
        }
    }

    public void Render() {
        Draw.Rectangle(Bounds, Color.White);
    }

    public void OnCollide(GameObject paddle, Solid solid) {

    }
}
