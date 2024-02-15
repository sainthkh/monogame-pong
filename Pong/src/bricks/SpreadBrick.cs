// using System;
// using Microsoft.Xna.Framework;

// namespace mg_pong;

// enum SpreadBrickState {
//     Stop,
//     Expand,
//     Shrink,
// }

// public class SpreadBrick: Brick {
// //     public float ShortRadius { get; set; }
// //     public float LongRadius { get; set; }
// //     public float Interval { get; set; }
// //     private float currentRadius;
// //     private float speed;
// //     public Point Pivot { get; set; }
// //     public float Angle { get; set; }

// //     public float ElapsedTime { get; set; }

// //     private FiniteStateMachine<SpreadBrickState> fsm;

// //     public SpreadBrick(int width, int height, float shortRadius, float longRadius, Point pivot, float angle) 
// //         : base(-300, -300, width, height) { 
// //         Interval = 3.0f;
// //         ShortRadius = shortRadius;
// //         LongRadius = longRadius;
// //         Pivot = pivot;
// //         Angle = angle;
// //         speed = (LongRadius - ShortRadius) / Interval;
// //         currentRadius = LongRadius;

// //         rec.X = (int)(Pivot.X + Math.Cos(Angle * Math.PI / 180) * currentRadius);
// //         rec.Y = (int)(Pivot.Y + Math.Sin(Angle * Math.PI / 180) * currentRadius);

// //         fsm = new FiniteStateMachine<SpreadBrickState>();
// //         fsm.CurrentState = SpreadBrickState.Stop;

// //         fsm.AddState(SpreadBrickState.Stop, (gameTime) => {
// //             ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

// //             if (ElapsedTime >= Interval) {
// //                 ElapsedTime = 0;

// //                 if (Math.Abs(currentRadius - ShortRadius) < 0.1f) {
// //                     fsm.CurrentState = SpreadBrickState.Expand;
// //                 } else {
// //                     fsm.CurrentState = SpreadBrickState.Shrink;
// //                 }
// //             }
// //         });

// //         fsm.AddState(SpreadBrickState.Expand, (gameTime) => {
// //             currentRadius += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

// //             rec.X = (int)(Pivot.X + Math.Cos(Angle * Math.PI / 180) * currentRadius);
// //             rec.Y = (int)(Pivot.Y + Math.Sin(Angle * Math.PI / 180) * currentRadius);

// //             if (currentRadius >= LongRadius) {
// //                 currentRadius = LongRadius;
// //                 fsm.CurrentState = SpreadBrickState.Stop;
// //             }
// //         });

// //         fsm.AddState(SpreadBrickState.Shrink, (gameTime) => {
// //             currentRadius -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

// //             rec.X = (int)(Pivot.X + Math.Cos(Angle * Math.PI / 180) * currentRadius);
// //             rec.Y = (int)(Pivot.Y + Math.Sin(Angle * Math.PI / 180) * currentRadius);

// //             if (currentRadius <= ShortRadius) {
// //                 currentRadius = ShortRadius;
// //                 fsm.CurrentState = SpreadBrickState.Stop;
// //             }
// //         });
// //     }

// //     public override void Update(GameTime gameTime)
// //     {
// //         fsm.Update(gameTime);
// //     }
// // }
