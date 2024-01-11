using System;
using Microsoft.Xna.Framework;

namespace mg_pong;

public enum WallType {
    Top,
    Bottom,
    Left,
    Right,
}

public class Wall: Solid {
    public WallType WallType { get; set; }

    public Wall(WallType wallType): base() {
        WallType = wallType;
    }
}
