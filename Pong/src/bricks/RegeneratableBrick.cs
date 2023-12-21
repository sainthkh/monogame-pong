using Microsoft.Xna.Framework;

namespace mg_pong;

public class RegeneratableBrick: Brick {
    private float coolTime = 0;
    public const float RESPAWN_TIME = 5;

    public RegeneratableBrick(int x, int y, int width, int height = 15) : base(x, y, width, height) { }

    public override void Update(GameTime gameTime) {
        if (!IsAlive) {
            coolTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (coolTime >= RESPAWN_TIME) {
                IsAlive = true;
                coolTime = 0;
            }
        }
    }

    public override void OnHit(Ball ball) {
        IsAlive = false;
    }
}