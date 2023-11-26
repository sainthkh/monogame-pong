using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace mg_pong;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Point GameBounds = new Point(450, 800); //window resolution

    public Texture2D Texture;

    private SpriteFont font;
    private SpriteFont resultMessageFont;
    private SpriteFont buttonFont;

    private Scene currentScene;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = GameBounds.X;
        _graphics.PreferredBackBufferHeight = GameBounds.Y;
        IsMouseVisible = true;
        currentScene = new Level4();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        SharedResource.GraphicsDevice = GraphicsDevice;
        SharedResource.SpriteBatch = _spriteBatch;

        font = Content.Load<SpriteFont>("Content/Score");
        resultMessageFont = Content.Load<SpriteFont>("Content/GameResult");
        buttonFont = Content.Load<SpriteFont>("Content/ButtonText");

        Texture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
        Texture.SetData<Color>(new Color[] { Color.White });

        SharedResource.Font = font;
        SharedResource.Texture = Texture;
        SharedResource.SoundFX = new AudioSource();
        SharedResource.GameBounds = GameBounds;
        SharedResource.ResultMessageFont = resultMessageFont;
        SharedResource.ButtonFont = buttonFont;

        currentScene.Load();
    }

    protected override void Update(GameTime gameTime)
    {
        currentScene.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        currentScene.Draw(gameTime);

        base.Draw(gameTime);
    }
}
