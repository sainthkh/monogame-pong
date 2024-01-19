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
    public BrickMoveType MoveType { get; set; }

    public BrickMove(Brick2 brick) {
        brick2 = brick;
    }

    public abstract void Move(float deltaTime);

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

    public override void Move(float deltaTime) {
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
        brick.IsAlive = false;
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

    public void Render() {
        if (!IsAlive) {
            return;
        }
        Draw.Rectangle(Bounds, Color);
    }

    public override void OnCollideActor(Snapshot other, float deltaTime)
    {
        if (other.Type is ActorType.Ball) {
            onHit.OnHit(other, deltaTime);
        }
    }
}
