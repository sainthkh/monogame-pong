using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace mg_pong;

public class FiniteStateMachine<T> {
    public Dictionary<T, Action<GameTime>> States { get; set; }
    public T CurrentState { get; set; }

    public FiniteStateMachine() {
        States = new Dictionary<T, Action<GameTime>>();
    }

    public void AddState(T name, Action<GameTime> update) {
        States.Add(name, update);
    }

    public void Update(GameTime gameTime) {
        States[CurrentState](gameTime);
    }
}
