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

public class Item: Actor {
    public float Speed { get; set; }
    public Vector2 Direction { get; set; }

    public ItemType ItemType { get; set; }

    public Item() {
        Bounds = new Rectangle(0, 0, 20, 20);
    }

    public void Move(float deltaTime) {
        MoveX(Direction.X * Speed * deltaTime, OnCollideSolid);
        MoveY(Direction.Y * Speed * deltaTime, OnCollideSolid);
    }

    public void Draw() {
        ItemRenderer.Draw(ItemType, Bounds);
    }

    public void OnCollideSolid(GameObject other, Solid solid) {
        
    }

    public override void OnCollideActor(Snapshot other, float deltaTime)
    {
        
    }
}

public class ItemManager {
    public static List<Item> items = new List<Item>();

    public static void AddNew(int x, int y) {
        var item = new Item();
        item.X = x;
        item.Y = y;
        item.Speed = 200;
        item.Direction = new Vector2(0, 1);
        item.ItemType = EnumUtil.Next<ItemType>();

        items.Add(item);
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
