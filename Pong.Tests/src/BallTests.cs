using mg_pong;
using Microsoft.Xna.Framework;
using static CheckCraft;

public static class BallTests {
    public static void Run() {
        SharedResource.GameBounds = new Point(450, 800);

        Describe("Ball", () => {
            It("moves correctly", () => {
                var ball = new Ball();
                ball.Direction = new Vector2(0.707f, 0.707f);
                ball.Speed = 10;
                ball.Move();

                Expect(ball.X).ToBe(7); // 7.07 -> 7
                Expect(ball.Y).ToBe(7);
            });
        });

        Describe("Ball Collision with Walls", () => {
            It("ball hits top wall", () => {
                var ball = new Ball();
                ball.HitTopWall += (ball) => {};
                ball.Direction = new Vector2(-0.2f, -0.8f);
                ball.X = 200;
                ball.Y = 10;
                ball.Speed = 20;
                ball.Move();
                ball.CheckWallCollision();

                Expect(ball.Y).ToBeGreaterThan(0);
                Expect(ball.DirectionY).ToBeGreaterThan(0);
                Expect(ball.DirectionX).ToBeLessThan(0);
            });

            It("ball hits bottom wall", () => {
                var ball = new Ball();
                ball.HitBottomWall += (ball) => {};
                ball.Direction = new Vector2(-0.2f, 0.8f);
                ball.X = 200;
                ball.Y = 790;
                ball.Speed = 20;
                ball.Move();
                ball.CheckWallCollision();

                Expect(ball.Y).ToBeLessThan(800);
                Expect(ball.DirectionY).ToBeLessThan(0);
                Expect(ball.DirectionX).ToBeLessThan(0);
            });

            It("ball hits left wall", () => {
                var ball = new Ball();
                ball.Direction = new Vector2(-0.8f, -0.2f);
                ball.X = 10;
                ball.Y = 200;
                ball.Speed = 20;
                ball.Move();
                ball.CheckWallCollision();

                Expect(ball.X).ToBeGreaterThan(0);
                Expect(ball.DirectionX).ToBeGreaterThan(0);
                Expect(ball.DirectionY).ToBeLessThan(0);
            });

            It("ball hits right wall", () => {
                var ball = new Ball();
                ball.Direction = new Vector2(0.8f, -0.2f);
                ball.X = 440;
                ball.Y = 200;
                ball.Speed = 20;
                ball.Move();
                ball.CheckWallCollision();

                Expect(ball.X).ToBeLessThan(440);
                Expect(ball.DirectionX).ToBeLessThan(0);
                Expect(ball.DirectionY).ToBeLessThan(0);
            });
        });
    
        Describe("Ball Collision with Player", () => {
            It("Ball hit Top Right + Ball moves to left", () => {
                var ball = new Ball();
                var paddle = new PaddlePlayer();

                ball.Direction = new Vector2(-0.5f, 1);
                ball.HitPlayer += (ball, paddle) => {};
                ball.X = 355;
                ball.Y = 690;
                ball.Speed = 15;

                paddle.X = 300;
                paddle.Y = 700;

                ball.Move();
                ball.CheckPlayerCollision(paddle);

                Expect(ball.DirectionY).ToBeLessThan(0);
                Expect(ball.DirectionX).ToBeLessThan(0);
                Expect(MathF.Abs(ball.DirectionY) * 2).ToBeGreaterThan(MathF.Abs(ball.DirectionX));
            });

            It("Ball hit Top Right + Ball moves to right", () => {
                var ball = new Ball();
                var paddle = new PaddlePlayer();

                ball.Direction = new Vector2(0.5f, 1);
                ball.HitPlayer += (ball, paddle) => {};
                ball.X = 350;
                ball.Y = 690;
                ball.Speed = 15;

                paddle.X = 300;
                paddle.Y = 700;

                ball.Move();
                ball.CheckPlayerCollision(paddle);

                Expect(ball.DirectionY).ToBeLessThan(0);
                Expect(ball.DirectionX).ToBeGreaterThan(0);
                Expect(MathF.Abs(ball.DirectionY) * 2).ToBeGreaterThan(MathF.Abs(ball.DirectionX));
            });

            It("Ball hit Top Left + Ball moves to left", () => {
                var ball = new Ball();
                var paddle = new PaddlePlayer();

                ball.Direction = new Vector2(-0.5f, 1);
                ball.HitPlayer += (ball, paddle) => {};
                ball.X = 315;
                ball.Y = 690;
                ball.Speed = 15;

                paddle.X = 300;
                paddle.Y = 700;

                ball.Move();
                ball.CheckPlayerCollision(paddle);

                Expect(ball.DirectionY).ToBeLessThan(0);
                Expect(ball.DirectionX).ToBeLessThan(0);
                Expect(MathF.Abs(ball.DirectionY) * 2).ToBeGreaterThan(MathF.Abs(ball.DirectionX));
            });

            It("Ball hit Top Left + Ball moves to right", () => {
                var ball = new Ball();
                var paddle = new PaddlePlayer();

                ball.Direction = new Vector2(0.5f, 1);
                ball.HitPlayer += (ball, paddle) => {};
                ball.X = 300;
                ball.Y = 690;
                ball.Speed = 15;

                paddle.X = 300;
                paddle.Y = 700;

                ball.Move();
                ball.CheckPlayerCollision(paddle);

                Expect(ball.DirectionY).ToBeLessThan(0);
                Expect(ball.DirectionX).ToBeGreaterThan(0);
                Expect(MathF.Abs(ball.DirectionY) * 2).ToBeGreaterThan(MathF.Abs(ball.DirectionX));
            });

            It("Ball hit Right", () => {
                var ball = new Ball();
                var paddle = new PaddlePlayer();

                ball.Direction = new Vector2(-0.5f, 1);
                ball.HitPlayer += (ball, paddle) => {};
                ball.X = 365;
                ball.Y = 695;
                ball.Speed = 15;

                paddle.X = 300;
                paddle.Y = 700;

                ball.Move();
                ball.CheckPlayerCollision(paddle);

                Expect(ball.DirectionY).ToBeLessThan(0);
                Expect(ball.DirectionX).ToBeGreaterThan(0);
                Expect(MathF.Abs(ball.DirectionY) * 2).ToBeGreaterThan(MathF.Abs(ball.DirectionX));
            });

            It("Ball hit Left", () => {
                var ball = new Ball();
                var paddle = new PaddlePlayer();

                ball.Direction = new Vector2(0.5f, 1);
                ball.HitPlayer += (ball, paddle) => {};
                ball.X = 295;
                ball.Y = 695;
                ball.Speed = 15;

                paddle.X = 300;
                paddle.Y = 700;

                ball.Move();
                ball.CheckPlayerCollision(paddle);

                Expect(ball.DirectionY).ToBeLessThan(0);
                Expect(ball.DirectionX).ToBeLessThan(0);
                Expect(MathF.Abs(ball.DirectionY) * 2).ToBeGreaterThan(MathF.Abs(ball.DirectionX));
            });

            It("Ball hit Bottom", () => {
                var ball = new Ball();
                var paddle = new PaddlePlayer();
                var hit = false;

                ball.Direction = new Vector2(0.5f, -1);
                ball.HitPlayer += (ball, paddle) => {
                    hit = true;
                };
                ball.X = 320;
                ball.Y = 730;
                ball.Speed = 15;

                paddle.X = 300;
                paddle.Y = 700;

                ball.Move();
                ball.CheckPlayerCollision(paddle);

                Expect(hit).ToBeTrue();
                Expect(ball.DirectionY).ToBeGreaterThan(0);
                Expect(ball.DirectionX).ToBeGreaterThan(0);
            });
        });

        Describe("Ball Collision with Enemy", () => {
            It("Ball moves toward the enemy", () => {
                var ball = new Ball();
                var paddle = new PaddleEnemy();

                ball.Direction = new Vector2(-0.5f, -1);
                ball.HitEnemy += (ball, paddle) => {};
                ball.X = 320;
                ball.Y = 80;
                ball.Speed = 15;

                paddle.X = 300;

                ball.Move();
                ball.CheckEnemyCollision(paddle);

                Expect(ball.DirectionY).ToBeGreaterThan(0);
            });
        });

        Describe("Ball Collision with Brick", () => {
            It("Ball hit Top", () => {
                var ball = new Ball();
                var brick = new Brick(100, 100, 20, 20);

                ball.Direction = new Vector2(-0.5f, 1);
                ball.X = 120;
                ball.Y = 105;
                ball.Speed = 15;

                ball.Move();

                Expect(brick.Collides(ball)).ToBeTrue();

                ball.OnCollideBrick(brick);

                Expect(ball.DirectionY).ToBeLessThan(0);
            });

            It("Ball hit Bottom", () => {
                var ball = new Ball();
                var brick = new Brick(100, 100, 20, 20);

                ball.Direction = new Vector2(-0.5f, -1);
                ball.X = 120;
                ball.Y = 125;
                ball.Speed = 15;

                ball.Move();

                Expect(brick.Collides(ball)).ToBeTrue();

                ball.OnCollideBrick(brick);

                Expect(ball.DirectionY).ToBeGreaterThan(0);
            });

            It("Ball hit Right", () => {
                var ball = new Ball();
                var brick = new Brick(100, 100, 20, 20);

                ball.Direction = new Vector2(-1, -0.5f);
                ball.X = 125;
                ball.Y = 120;
                ball.Speed = 15;

                ball.Move();

                Expect(brick.Collides(ball)).ToBeTrue();

                ball.OnCollideBrick(brick);

                Expect(ball.DirectionX).ToBeGreaterThan(0);
            });

            It("Ball hit Left", () => {
                var ball = new Ball();
                var brick = new Brick(100, 100, 20, 20);

                ball.Direction = new Vector2(1, -0.5f);
                ball.X = 95;
                ball.Y = 120;
                ball.Speed = 15;

                ball.Move();

                Expect(brick.Collides(ball)).ToBeTrue();

                ball.OnCollideBrick(brick);

                Expect(ball.DirectionX).ToBeLessThan(0);
            });
        });
    }
}
