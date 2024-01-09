using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace mg_pong;

public static class GameObjectManager {
    private static readonly List<GameObject> gameObjects = new List<GameObject>();

    public static void Add(GameObject gameObject) {
        gameObjects.Add(gameObject);
    }

    public static void Remove(GameObject gameObject) {
        gameObjects.Remove(gameObject);
    }

    public static List<GameObject> Objects {
        get {
            return gameObjects;
        }
    }
}
