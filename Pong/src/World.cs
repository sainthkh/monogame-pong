using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mg_pong;

public class World: Scene {

    private Paddle2Player player;

    public override void Load()
    {
        player = new Paddle2Player();

        LoadWalls();
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
        player.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Xna.GraphicsDevice.Clear(Color.Black);

        Xna.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

        player.Render();

        Xna.SpriteBatch.End();
    }
}
