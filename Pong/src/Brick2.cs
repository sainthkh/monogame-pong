using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace mg_pong;

public enum BrickMoveType {
    None,
    Circular,
    Rail,
    Rectangular,
    RandomAndStop,
}

public abstract class BrickMove {
    private Brick2 brick2;
    private Vector2 removalDirection;
    private bool isRemoving;
    public BrickMoveType MoveType { get; set; }
    public bool IsRemoving { 
        get {
            return isRemoving;
        } 
        set {
            isRemoving = value;

            if (value) {
                removalDirection = new Vector2(
                    brick2.X - GameBounds.X / 2,
                    brick2.Y - GameBounds.Y / 2
                );

                if (removalDirection.Length() < 0.1) {
                    removalDirection = new Vector2(
                        Xna.Rand.RandomFloat(-1, 1),
                        Xna.Rand.RandomFloat(-1, 1)
                    );
                }

                removalDirection.Normalize();
            }

        }
    }

    public BrickMove(Brick2 brick) {
        brick2 = brick;
        IsRemoving = false;
    }

    public void Move(float deltaTime) {
        if (IsRemoving) {
            float speed = 300;

            brick2.X += (int)(removalDirection.X * speed * deltaTime);
            brick2.Y += (int)(removalDirection.Y * speed * deltaTime);
        } else {
            OwnMove(deltaTime);
        }
    }
    public abstract void OwnMove(float deltaTime);

    public static BrickMove Create(Brick2 brick, BrickMoveType moveType) {
        switch (moveType) {
            case BrickMoveType.None:
                return new BrickMoveNone(brick);
            // case BrickMoveType.Circular:
            //     return new BrickMoveCircular(brick);
            // case BrickMoveType.Rail:
            //     return new BrickMoveRail(brick);
            // case BrickMoveType.Rectangular:
            //     return new BrickMoveRectangular(brick);
            // case BrickMoveType.RandomAndStop:
            //     return new BrickMoveRandomAndStop(brick);
            default:
                return new BrickMoveNone(brick);
        }
    }
}

public class BrickMoveNone: BrickMove {
    public BrickMoveNone(Brick2 brick): base(brick) {
        MoveType = BrickMoveType.None;
    }

    public override void OwnMove(float deltaTime) {
        // Do nothing
    }
}

public enum BrickOnHitType {
    None,
    Break,
    Count,
    Revive,
    Generate,
}

public abstract class BrickOnHit {
    protected Brick2 brick;
    public BrickOnHitType OnHitType { get; set; }

    public BrickOnHit(Brick2 brick2) {
        this.brick = brick2;
    }

    public abstract void OnHit(Snapshot snapshot, float deltaTime);

    public static BrickOnHit Create(Brick2 brick, BrickOnHitType onHitType) {
        switch (onHitType) {
            case BrickOnHitType.None:
                return new BrickOnHitNone(brick);
            case BrickOnHitType.Break:
                return new BrickOnHitBreak(brick);
            default:
                return new BrickOnHitNone(brick);
        }
    }
}

public class BrickOnHitNone: BrickOnHit {
    public BrickOnHitNone(Brick2 brick): base(brick) {
        OnHitType = BrickOnHitType.None;
    }

    public override void OnHit(Snapshot snapshot, float deltaTime) {
        // Do nothing
    }
}

public class BrickOnHitBreak: BrickOnHit {
    public BrickOnHitBreak(Brick2 brick): base(brick) {
        OnHitType = BrickOnHitType.Break;
    }

    public override void OnHit(Snapshot snapshot, float deltaTime) {
        brick.Break(snapshot, deltaTime);
    }
}

public class BrickSnapshot: Snapshot {
    public List<float> CornerDegrees;

    public BrickSnapshot(Brick2 brick): base(brick) {
    }
}

public class Brick2: Actor {
    private List<float> cornerDegrees;
    private BrickMove move;
    private BrickOnHit onHit;

    public BrickMoveType MoveType { 
        get { return move.MoveType; } 
        set {
            move = BrickMove.Create(this, value);
        }
    }
    public BrickOnHitType OnHitType { 
        get { return onHit.OnHitType; } 
        set {
            onHit = BrickOnHit.Create(this, value);
        }
    }
    public Color Color { get; set; }
    public bool IsAlive { get; set; }
    public bool IsRemoving {
        get {
            return move.IsRemoving;
        }
        set {
            move.IsRemoving = value;
        }
    }

    public delegate void OnBreakEvent(Brick2 brick, Snapshot other, float deltaTime);
    public event OnBreakEvent OnBreak;

    public Brick2() {
        cornerDegrees = MathUtil.CornerDegrees(Bounds);
        IsAlive = true;
        Color = Color.White;
        actorType = ActorType.Brick;
        move = BrickMove.Create(this, BrickMoveType.None);
        onHit = BrickOnHit.Create(this, BrickOnHitType.Break);
    }

    public Brick2(Rectangle bounds): this() {
        Bounds = bounds;
    }

    public override Snapshot Snapshot()
    {
        var snapshot = new BrickSnapshot(this);
        snapshot.CornerDegrees = cornerDegrees;

        return snapshot;
    }

    public void Move(float deltaTime) {
        move.Move(deltaTime);
    }

    public void Draw() {
        if (!IsAlive) {
            return;
        }
        Render.Rectangle(Bounds, Color);
    }

    public override void OnCollideActor(Snapshot other, float deltaTime)
    {
        if (other.Type is ActorType.Ball) {
            onHit.OnHit(other, deltaTime);
        }
    }

    public void Break(Snapshot other, float deltaTime) {
        IsAlive = false;

        if (MathUtil.RandSuccess(100)) {
            ItemManager.AddNew(X, Y);
        }
        OnBreak?.Invoke(this, other, deltaTime);
    }
}

public class GuardBrick: Brick2 {
    private float counter = 0;
    private readonly float REGEN_TIME = 40;

    public GuardBrick(): base() {
    }

    public GuardBrick(Rectangle bounds, float regenTime): base(bounds) {
        REGEN_TIME = regenTime;
    }

    public void Regenerate(float deltaTime) {
        if (!IsAlive) {
            counter += deltaTime;

            if (counter > REGEN_TIME) {
                IsAlive = true;
                counter = 0;
            }
        }
    }
}

public class PlayerGuardBrick: Brick2 {
    private float counter = 0;
    private const float REGEN_TIME = 40;
    public PlayerGuardBrick(): base() {
    }

    public PlayerGuardBrick(Rectangle bounds): base(bounds) {
    }

    public void Regenerate(float deltaTime) {
        if (!IsAlive) {
            counter += deltaTime;

            if (counter > REGEN_TIME) {
                IsAlive = true;
                counter = 0;
            }
        }
    }
}

public class EnemyGuardBrick: Brick2 {
    private float counter = 0;
    private const float REGEN_TIME = 60;

    public EnemyGuardBrick(): base() {
    }

    public EnemyGuardBrick(Rectangle bounds): base(bounds) {
    }

    public void Regenerate(float deltaTime) {
        if (!IsAlive) {
            counter += deltaTime;

            if (counter > REGEN_TIME) {
                IsAlive = true;
                counter = 0;
            }
        }
    }
}
