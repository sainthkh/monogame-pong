using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mg_pong;

public class World: Scene {

    private Paddle2Player player;
    private Paddle2Enemy enemy;
    private Ball2 ball;

    public override void Load()
    {
        LoadWalls();

        ball = new Ball2();
        ball.X = GameBounds.X / 2;
        ball.Y = GameBounds.Y / 2;

        player = new Paddle2Player();
        enemy = new Paddle2Enemy();
        enemy.Ball = ball;

        BrickManager.Generate();
    }

    private void LoadWalls() {
        var topWall = new Wall(WallType.Top);
        topWall.Bounds = new Rectangle(0, -15, GameBounds.X, 15);
        var bottomWall = new Wall(WallType.Bottom);
        bottomWall.Bounds = new Rectangle(0, GameBounds.Y, GameBounds.X, 15);
        var leftWall = new Wall(WallType.Left);
        leftWall.Bounds = new Rectangle(-15, 0, 15, GameBounds.Y);
        var rightWall = new Wall(WallType.Right);
        rightWall.Bounds = new Rectangle(GameBounds.X, 0, 15, GameBounds.Y);

        GameObjectManager.AddSolid(topWall);
        GameObjectManager.AddSolid(bottomWall);
        GameObjectManager.AddSolid(leftWall);
        GameObjectManager.AddSolid(rightWall);
    }

    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        MoveActors(deltaTime);
        HandleCollsions(deltaTime);
    }

    private void MoveActors(float deltaTime) {
        player.Move(deltaTime);
        enemy.Move(deltaTime);
        ball.Move(deltaTime);
    }

    private void HandleCollsions(float deltaTime) {
        CollisionManager.Clear();

        if(player.Collides(ball)) {
            CollisionManager.AddCollision(player, ball);
        }

        if(enemy.Collides(ball)) {
            CollisionManager.AddCollision(enemy, ball);
        }

        BrickManager.CheckCollision(ball);

        CollisionManager.HandleCollisions(deltaTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Xna.GraphicsDevice.Clear(Color.Black);

        Xna.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

        player.Render();
        enemy.Render();
        ball.Render();

        BrickManager.Render();

        Xna.SpriteBatch.End();
    }
}
