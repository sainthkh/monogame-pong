using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mg_pong;

public class World: Scene {

    private Paddle2Player player;

    public override void Load()
    {
        player = new Paddle2Player();
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
