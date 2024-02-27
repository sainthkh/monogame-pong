using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace mg_pong;

public enum ArrangementType {
    RandomGrid,
    MovingInCircle,
    InOut,
    InRows,
}

public enum BlockType {
    Whole,
    Row2,
    Row3,
    Col2,
    Grid2x2,
}

public enum BrickGenerationType {
    Random,
    Tetris,
}

public static class BrickManager {
    private static bool removeAll = false;
    private static float removeAllTimer = 0.0f;
    private static float removeAllTime = 4.0f;

    private static List<Brick2> bricks = new List<Brick2>();
    private static List<GuardBrick> enemyGuardBricks = new List<GuardBrick>();
    private static List<GuardBrick> playerGuardBricks = new List<GuardBrick>();
    private static List<Brick2> updatableBricks = new List<Brick2>();

    public delegate void FinishRemove();
    public static FinishRemove OnFinishRemove;

    public static void AddBrick(Brick2 brick) {
        bricks.Add(brick);
    }

    public static void AddUpdatableBrick(Brick2 brick) {
        if(!updatableBricks.Contains(brick)) {
            updatableBricks.Add(brick);   
        }
    }

    // Generate Bricks
    public static void Generate() {
        ArrangementType arrangementType = (ArrangementType)Xna.Rand.Next(0, System.Enum.GetValues(typeof(ArrangementType)).Length);

        switch(arrangementType) {
            case ArrangementType.RandomGrid:
                GenerateGridedRandom();
                break;
            case ArrangementType.MovingInCircle:
                GenerateBricksMovingInCircle();
                break;
            case ArrangementType.InRows:
                GenerateBricksInRows();
                break;
            case ArrangementType.InOut:
                GenerateBricksMovingInAndOut();
                break;
        }
    }

    public static void GenerateGridedRandom() {
        BlockType blockType = (BlockType)Xna.Rand.Next(0, System.Enum.GetValues(typeof(BlockType)).Length);
        var blockBounds = GetBlockBounds(blockType);

        foreach(var bounds in blockBounds) {
            var genType = (BrickGenerationType)Xna.Rand.Next(0, System.Enum.GetValues(typeof(BrickGenerationType)).Length);            
            switch(genType) {
                case BrickGenerationType.Random:
                    GenerateRandomBrick(bounds);
                    break;
                case BrickGenerationType.Tetris:
                    GenerateTetrisBricks(bounds);
                    break;
            }
        }
    }

    private static List<Rectangle> GetBlockBounds(BlockType blockType) {
        const int padding = 100;

        switch(blockType) {
            case BlockType.Whole:
                return new List<Rectangle>() { 
                    new Rectangle(0, padding, GameBounds.X, GameBounds.Y - padding * 2),
                };
            case BlockType.Row2:
                return new List<Rectangle>() { 
                    new Rectangle(0, padding, GameBounds.X, GameBounds.Y / 2 - padding),
                    new Rectangle(0, GameBounds.Y / 2, GameBounds.X, GameBounds.Y / 2 - padding),
                };
            case BlockType.Row3:
                return new List<Rectangle>() { 
                    new Rectangle(0, padding, GameBounds.X, (GameBounds.Y - 2 * padding) / 3 ),
                    new Rectangle(0, GameBounds.Y / 3, GameBounds.X, (GameBounds.Y - 2 * padding) / 3 ),
                    new Rectangle(0, GameBounds.Y / 3 * 2, GameBounds.X, (GameBounds.Y - 2 * padding) / 3 ),
                };
            case BlockType.Col2:
                return new List<Rectangle>() { 
                    new Rectangle(0, padding, GameBounds.X / 2, GameBounds.Y - padding * 2),
                    new Rectangle(GameBounds.X / 2, padding, GameBounds.X / 2, GameBounds.Y - padding * 2),
                    };
            case BlockType.Grid2x2:
                return new List<Rectangle>() { 
                    new Rectangle(0, padding, GameBounds.X / 2, GameBounds.Y / 2 - padding),
                    new Rectangle(GameBounds.X / 2, padding, GameBounds.X / 2, GameBounds.Y / 2 - padding),
                    new Rectangle(0, GameBounds.Y / 2, GameBounds.X / 2, GameBounds.Y / 2 - padding),
                    new Rectangle(GameBounds.X / 2, GameBounds.Y / 2, GameBounds.X / 2, GameBounds.Y / 2 - padding),
                };
        }

        return null;
    }

    public static void GenerateTetrisBricks(Rectangle bounds) {
        List<TetrisBrickGroup> tetrisBrickGroups = new List<TetrisBrickGroup>();

        for(int i = 0; i < 3; i++) {
            while(true) {
                var tetris = new TetrisBrickGroup();
                tetris.Generate();

                var x = Xna.Rand.Next(bounds.X, bounds.X + bounds.Width - tetris.Bounds.Width);
                var y = Xna.Rand.Next(bounds.Y, bounds.Y + bounds.Height - tetris.Bounds.Height);

                tetris.Left = x;
                tetris.Top = y;

                bool intersects = false;
                foreach(var t in tetrisBrickGroups) {
                    if (t.Bounds.Intersects(tetris.Bounds)) {
                        intersects = true;
                        break;
                    }
                }

                if (!intersects) {
                    tetrisBrickGroups.Add(tetris);
                    bricks.AddRange(tetris.Bricks);
                    break;
                }
            }
        }
    }

    public static void GenerateRandomBrick(Rectangle bounds) {
        for(int i = 0; i < 5; i++) {
            var width = Xna.Rand.Next(10, 30);
            var height = Xna.Rand.Next(10, 30);
            var x = Xna.Rand.Next(bounds.X, bounds.X + bounds.Width - width);
            var y = Xna.Rand.Next(bounds.Y, bounds.Y + bounds.Height - height);
            var brick = GenerateBrick(x, y, width, height);

            bricks.Add(brick);
        }
    }

    public static void GenerateBricksMovingInCircle() {
        int numBricks = 12;

        for(int i = 0; i < numBricks; i++) {
            var brick = new Brick2();
            brick.Width = 15;
            brick.Height = 15;
            brick.MoveType = BrickMoveType.Circular;

            var move = (BrickMoveCircular)brick.BrickMove;

            move.Pivot = new Point(GameBounds.X / 2, GameBounds.Y / 2);
            move.Radius = 100;
            move.Rotation = 360 / numBricks * i;

            bricks.Add(brick);
        }
    }

    public static void GenerateBricksMovingInAndOut() {
        Point pivot = new Point(
            GameBounds.X / 2,
            GameBounds.Y / 2
        );
        int[] vars = new int[] { 10, 40 };
        int[] vars2 = new int[] { 40, 140 };

        for(int i = 0; i < 10; i++) {
            var brick = new Brick2();
            brick.Width = 15;
            brick.Height = 15;
            brick.MoveType = BrickMoveType.InOut;

            var move = (BrickMoveInOut)brick.BrickMove;
            move.Pivot = pivot;
            move.ShortRadius = vars[0];
            move.LongRadius = vars[1];
            move.Angle = 36 * i;
            move.Interval = 3;
            move.Initialize();
            
            bricks.Add(brick);
        }

        for(int i = 0; i < 10; i++) {
            var brick = new Brick2();
            brick.Width = 15;
            brick.Height = 15;
            brick.MoveType = BrickMoveType.InOut;

            var move = (BrickMoveInOut)brick.BrickMove;
            move.Pivot = pivot;
            move.ShortRadius = vars2[0];
            move.LongRadius = vars2[1];
            move.Angle = 36 * i;
            move.Interval = 3;
            move.Initialize();
            
            bricks.Add(brick);
        }
    }

    public static void GenerateBricksInRows() {
        List<Point> startingPoints = new List<Point>() {
            new Point(80, 100),
            new Point(150, 250),
            new Point(80, 400),
            new Point(150, 550),
        };

        List<Point> points = new List<Point>();

        foreach(Point startingPoint in startingPoints) {
            int x = startingPoint.X;
            int y = startingPoint.Y;

            points.Add(new Point(x, y));

            for(int i = 0; i < 6; i++) {
                points.Add(new Point(x + 45 * (i + 1), y));
            }
        }

        int brickIndex = 0;

        foreach(Point point in points) {
            List<Point> p = new List<Point>();

            var hitType = brickIndex % 3 == 0 ? BrickOnHitType.Revive : BrickOnHitType.Break;

            Brick2 brick = GenerateBrick(point.X, point.Y, 15, 15, BrickMoveType.None, hitType);

            bricks.Add(brick);

            brickIndex++;
        }
    }

    public static Brick2 GenerateBrick(int x, int y, int width, int height) {
        var brick = new Brick2();
        brick.X = x;
        brick.Y = y;
        brick.Width = width;
        brick.Height = height;

        return brick;
    }

    public static Brick2 GenerateBrick(int x, int y, int width, int height, BrickMoveType moveType, BrickOnHitType onHitType) {
        var brick = new Brick2();
        brick.X = x;
        brick.Y = y;
        brick.Width = width;
        brick.Height = height;
        brick.MoveType = moveType;
        brick.OnHitType = onHitType;

        if (onHitType == BrickOnHitType.Generate) {
            var hitType = (BrickOnHitGenerate) brick.BrickOnHit;
            hitType.AddBricks();
        }

        return brick;
    }

    // Game Loop

    public static void RemoveAll() {
        updatableBricks.Clear();

        removeAll = true;

        foreach(var brick in bricks) {
            brick.IsRemoving = true;
        }
    }

    public static void UpdateBricks(float deltaTime) {
        RegenerateBricks(deltaTime);

        foreach(var brick in updatableBricks) {
            brick.Update(deltaTime);
        }
    }

    private static void RegenerateBricks(float deltaTime) {
        foreach(var brick in enemyGuardBricks) {
            brick.Regenerate(deltaTime);
        }

        foreach(var brick in playerGuardBricks) {
            brick.Regenerate(deltaTime);
        }
    }

    public static void Move(float deltaTime) {
        foreach(var brick in bricks) {
            brick.Move(deltaTime);
        }

        if (removeAll) {
            removeAllTimer += deltaTime;

            if (removeAllTimer > removeAllTime) {
                removeAll = false;
                removeAllTimer = 0.0f;
                bricks.Clear();
                OnFinishRemove?.Invoke();
                Generate();
            }
        }
    }

    public static void CheckCollision(Ball2 ball) {
        foreach(var brick in enemyGuardBricks) {
            if (brick.IsAlive && brick.Collides(ball)) {
                CollisionManager.AddCollision(ball, brick);
                break;
            }
        }

        foreach(var brick in playerGuardBricks) {
            if (brick.IsAlive && brick.Collides(ball)) {
                CollisionManager.AddCollision(ball, brick);
                break;
            }
        }

        if (removeAll) {
            return;
        }

        foreach(var brick in bricks) {
            if (brick.IsAlive && brick.Collides(ball)) {
                CollisionManager.AddCollision(ball, brick);
                break;
            }
        }
    }

    public static void Draw() {
        foreach(var brick in bricks) {
            brick.Draw();
        }

        foreach(var brick in enemyGuardBricks) {
            brick.Draw();
        }

        foreach(var brick in playerGuardBricks) {
            brick.Draw();
        }
    }

    // Guard Bricks

    public static void InitializeGuardBricks() {
        const int bricksPerRow = 6;
        int brickWidth = GameBounds.X / bricksPerRow;

        const float ENEMY_REGEN_TIME = 60.0f;
        const float PLAYER_REGEN_TIME = 40.0f;

        // Enemy guard bricks
        for (int i = 0; i < bricksPerRow; i++)
        {
            GuardBrick brick = new GuardBrick(new Rectangle(i * brickWidth, 5, brickWidth, 10), ENEMY_REGEN_TIME);
            brick.Color = i % 2 == 0 ? Color.DarkGray : Color.Gray;
            enemyGuardBricks.Add(brick);
        }

        // Player guard bricks
        for (int i = 0; i < bricksPerRow; i++)
        {
            GuardBrick brick = new GuardBrick(new Rectangle(i * brickWidth, GameBounds.Y - 20, brickWidth, 10), PLAYER_REGEN_TIME);
            brick.Color = i % 2 == 1 ? Color.DarkGray : Color.Gray;
            playerGuardBricks.Add(brick);
        }
    }

    public static int RegeneratePlayerGuardBricks() {
        int life = 2;

        for(int i = 0; i < 2; i++) {
            if(IsAllPlayerGuardBrickAlive()) {
                break;
            }

            life--;
            bool success = false;
            do {
                int next = Xna.Rand.Next(0, playerGuardBricks.Count);
                
                if (!playerGuardBricks[next].IsAlive) {
                    playerGuardBricks[next].IsAlive = true;
                    success = true;
                }
            } while(success == false);
        }
        
        return life;
    }

    private static bool IsAllPlayerGuardBrickAlive() {
        foreach(var brick in playerGuardBricks) {
            if (!brick.IsAlive) {
                return false;
            }
        }

        return true;
    }
}
