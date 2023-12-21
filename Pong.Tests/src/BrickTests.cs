using mg_pong;
using Microsoft.Xna.Framework;
using static CheckCraft;

public static class BrickTests {
    public static void Run() {
        Describe("Brick", () => {
            It("should be alive when created", () => {
                Brick brick = new Brick(0, 0, 15, 15);
                Expect(brick.IsAlive).ToBe(true);
            });

            It("should be dead when hit", () => {
                Brick brick = new Brick(0, 0, 15, 15);
                brick.OnHit(new Ball());
                Expect(brick.IsAlive).ToBe(false);
            });
        });

        Describe("RegenratableBrick", () => {
            It("should be alive when created", () => {
                RegeneratableBrick brick = new RegeneratableBrick(0, 0, 15, 15);
                Expect(brick.IsAlive).ToBe(true);
            });

            It("should be dead when hit", () => {
                RegeneratableBrick brick = new RegeneratableBrick(0, 0, 15, 15);
                brick.OnHit(new Ball());
                Expect(brick.IsAlive).ToBe(false);
            });

            It("should be alive after respawn time", () => {
                RegeneratableBrick brick = new RegeneratableBrick(0, 0, 15, 15);
                brick.OnHit(new Ball());
                Expect(brick.IsAlive).ToBe(false);

                var gameTime = new GameTime();
                gameTime.ElapsedGameTime = new System.TimeSpan(0, 0, (int)(RegeneratableBrick.RESPAWN_TIME + 1));
                brick.Update(gameTime);
                
                Expect(brick.IsAlive).ToBe(true);
            });
        });
    }
}