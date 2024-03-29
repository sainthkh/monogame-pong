using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mg_pong;

public class World: Scene {

    private Paddle2Player player;
    private Paddle2Enemy enemy;
    private Ball2 ball;

    private const int ENEMY_HP = 5;

    private int enemyHP;
    private int playerHP;
    private int playerShield;

    private RenderTarget2D playArea;

    private int progress = 1;

    public override void Load()
    {
        LoadWalls();

        ball = new Ball2();
        ball.X = GameBounds.X / 2;
        ball.Y = GameBounds.Y / 2;

        player = new Paddle2Player();
        enemy = new Paddle2Enemy();
        enemy.Ball = ball;

        enemyHP = ENEMY_HP;
        playerHP = 5;
        playerShield = 0;

        BrickManager.Generate();
        BrickManager.InitializeGuardBricks();
        BrickManager.OnFinishRemove += () => {
            enemyHP = ENEMY_HP;
            playerHP += 2;

            int remainder = BrickManager.RegeneratePlayerGuardBricks();
            playerHP += remainder;

            progress++;
        };

        playArea = new RenderTarget2D(
            SharedResource.GraphicsDevice,
            GameBounds.X,
            GameBounds.Y,
            false,
            SharedResource.GraphicsDevice.PresentationParameters.BackBufferFormat,
            DepthFormat.Depth24
        );

        ball.HitTopWall += (Ball2 ball) => {
            enemyHP--;

            if(enemyHP == 0) {
                BrickManager.RemoveAll();
            }
        };

        ball.HitBottomWall += (Ball2 ball) => {
            playerHP--;
        };
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
        BrickManager.UpdateBricks(deltaTime);
        MoveActors(deltaTime);
        HandleCollsions(deltaTime);
    }

    private void MoveActors(float deltaTime) {
        player.Move(deltaTime);
        enemy.Move(deltaTime);
        ball.Move(deltaTime);
        BrickManager.Move(deltaTime);
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
        Render.Text($"{progress}/100", new Vector2(900, 50), Color.White);

        Render.Text("Enemy", new Vector2(650, 120), Color.White);

        for (int i = 0; i < enemyHP; i++) {
            Render.Rectangle(new Rectangle(750 + i * 20, 120, 10, 25), enemy.Color);
        }

        Render.Text("Player", new Vector2(650, 160), Color.White);

        for (int i = 0; i < playerHP; i++) {
            Render.Rectangle(new Rectangle(750 + i * 20, 160, 10, 25), player.Color);
        }

        Render.Text("Items", new Vector2(650, 230), Color.White);

        for (int i = 0; i < player.MaxItems; i++) {
            Render.Rectangle(new Rectangle(650 + i * 50, 270, 36, 36), Color.White);
            Render.Rectangle(new Rectangle(650 + i * 50 + 3, 270 + 3, 30, 30), Color.Black);
        }

        for (int i = 0; i < player.Items.Count; i++) {
            var rect = new Rectangle(650 + i * 50 + 8, 270 + 8, 20, 20);
            ItemRenderer.Draw(player.Items[i], rect);
        }

        for (int i = 0; i < ItemEffect.Activated.Count; i++) {
            var itemType = ItemEffect.Activated[i];

            var rect = new Rectangle(650, 320 + i * 35, 20, 20);
            ItemRenderer.Draw(itemType, rect);

            for (int j = 0; j < ItemEffect.Charges[itemType]; j++) {
                Render.Rectangle(new Rectangle(650 + 30 + j * 20, 320 + i * 35, 10, 20), Color.White);
            }
        }
    }
}
