using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mg_pong;

public class Level6: World1 {
    protected override void AddMoreBlocks()
    {
        
    }

    public override void OnClickNextLevel()
    {
        SceneManager.LoadLevel(7);
    }
}
