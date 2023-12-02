using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mg_pong;

public class Level1: World1 {
    public override void OnClickNextLevel()
    {
        SceneManager.LoadLevel(2);
    }
}