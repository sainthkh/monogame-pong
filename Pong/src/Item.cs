using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace mg_pong;

public enum ItemType {
    SpeedUp,
    // SpeedDown,
    // SizeUp,
    // SizeDown,
}

public struct ItemRender {
    public Color Color { get; set; }
    public string Text { get; set; }
}

public static class ItemRenderer {
    private static Dictionary<ItemType, ItemRender> renders = new Dictionary<ItemType, ItemRender>();

    static ItemRenderer() {
        renders.Add(ItemType.SpeedUp, new ItemRender() {
            Color = Color.Red,
            Text = "S"
        });
    }

    public static void Draw(ItemType type, Rectangle Bounds) {
        var render = renders[type];
        Render.Rectangle(Bounds, render.Color);
        Render.Text(render.Text, new Vector2(Bounds.X, Bounds.Y), Color.Black);
    }
}

public class ItemSnapshot: Snapshot {
    public ItemType ItemType { get; set; }

    public ItemSnapshot(Item item): base(item) {
        ItemType = item.ItemType;
    }
}

public class Item: Actor {
    public float Speed { get; set; }
    public Vector2 Direction { get; set; }

    public ItemType ItemType { get; set; }

    public Item() {
        Bounds = new Rectangle(0, 0, 20, 20);
    }

    public override Snapshot Snapshot()
    {
        return new ItemSnapshot(this);
    }

    public void Move(float deltaTime) {
        MoveX(Direction.X * Speed * deltaTime, OnCollideSolid);
        MoveY(Direction.Y * Speed * deltaTime, OnCollideSolid);
    }

    public void Draw() {
        ItemRenderer.Draw(ItemType, Bounds);
    }

    public void OnCollideSolid(GameObject other, Solid solid) {
        if (solid is Wall) {
            var wall = (Wall)solid;

            if (wall.WallType == WallType.Bottom) {
                ItemManager.ToBeRemoved(this);
            }
        }
    }

    public override void OnCollideActor(Snapshot other, float deltaTime)
    {
        if(other.Type == ActorType.Player) {
            ItemManager.ToBeRemoved(this);
        }
    }
}

public class ItemManager {
    private static List<Item> items = new List<Item>();
    private static List<Item> itemsToRemove = new List<Item>();

    public static void AddNew(int x, int y) {
        var item = new Item();
        item.X = x;
        item.Y = y;
        item.Speed = 200;
        item.Direction = new Vector2(0, 1);
        item.ItemType = EnumUtil.Next<ItemType>();

        items.Add(item);
    }

    public static void ToBeRemoved(Item item) {
        itemsToRemove.Add(item);
    }

    public static void Remove() {
        foreach(var item in itemsToRemove) {
            items.Remove(item);
        }

        itemsToRemove.Clear();
    }

    public static void Move(float deltaTime) {
        foreach(var item in items) {
            item.Move(deltaTime);
        }
    }

    public static void Draw() {
        foreach(var item in items) {
            item.Draw();
        }
    }

    public static void CheckCollision(Paddle2Player player) {
        foreach(var item in items) {
            if (player.Collides(item)) {
                CollisionManager.AddCollision(player, item);
                break;
            }
        }
    }
}

public static class ItemEffect {
    public static Paddle2Player player;
    public static Paddle2Enemy enemy;
    public static Ball2 ball;

    private static float ballSpeed;

    public static void On(ItemType type) {
        switch(type) {
            case ItemType.SpeedUp:
                SpeedUpOn();
                break;
        }
    }

    public static void Off(ItemType type) {
        switch(type) {
            case ItemType.SpeedUp:
                SpeedUpOff();
                break;
        }
    }

    private static void SpeedUpOn() {
        ballSpeed = ball.Speed;
        ball.Speed = ballSpeed * 1.5f;
    }

    private static void SpeedUpOff() {
        ball.Speed = ballSpeed;     
    }
}
