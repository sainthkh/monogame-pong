using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mg_pong;

public class World: Scene {

    private Paddle2Player player;
    private Paddle2Enemy enemy;
    private Ball2 ball;

    private RenderTarget2D playArea;

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

        playArea = new RenderTarget2D(
            SharedResource.GraphicsDevice,
            GameBounds.X,
            GameBounds.Y,
            false,
            SharedResource.GraphicsDevice.PresentationParameters.BackBufferFormat,
            DepthFormat.Depth24
        );
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

        player.UseItem(deltaTime);
        MoveActors(deltaTime);
        HandleCollsions(deltaTime);
    }

    private void MoveActors(float deltaTime) {
        player.Move(deltaTime);
        enemy.Move(deltaTime);
        ball.Move(deltaTime);
        ItemManager.Move(deltaTime);
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
        ItemManager.CheckCollision(player);

        CollisionManager.HandleCollisions(deltaTime);

        ItemManager.Remove();
    }

    public override void Draw(GameTime gameTime)
    {
        DrawToPlayArea();

        Xna.GraphicsDevice.Clear(Color.Black);

        Xna.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

        Render.Rectangle(new Rectangle(25, 25, GameBounds.X + 25 * 2, GameBounds.Y + 25 * 2), Color.White);
        Xna.SpriteBatch.Draw(playArea, new Vector2(50, 50), Color.White);

        DrawUI();

        Xna.SpriteBatch.End();
    }

    private void DrawToPlayArea() 
    {
        Xna.GraphicsDevice.SetRenderTarget(playArea);
        Xna.GraphicsDevice.Clear(Color.Black);

        Xna.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

        player.Draw();
        enemy.Draw();
        ball.Draw();

        BrickManager.Draw();
        ItemManager.Draw();

        Xna.SpriteBatch.End();

        Xna.GraphicsDevice.SetRenderTarget(null);
    }

    private void DrawUI()
    {
        Render.Text("Progress", new Vector2(650, 50), Color.White);

        Render.Text("Point", new Vector2(650, 150), Color.White);

        Render.Text("Life", new Vector2(650, 250), Color.White);

        Render.Text("Items", new Vector2(650, 350), Color.White);

    }
}
