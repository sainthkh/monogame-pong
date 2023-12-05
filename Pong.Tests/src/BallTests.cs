using mg_pong;
using Microsoft.Xna.Framework;
using static CheckCraft;

public static class BallTests {
    public static void Run() {
        Describe("Ball", () => {
            It("moves correctly", () => {
                // Expect(1 + 1).ToBe(2);
                var ball = new Ball();
                ball.Direction = new Vector2(1, 0);
                ball.Speed = 1;
                ball.Move();

                Expect(ball.X).ToBe(1);
                Expect(ball.Y).ToBe(0);
            });
        });
    }
}