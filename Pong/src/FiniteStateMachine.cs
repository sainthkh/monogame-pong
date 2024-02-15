using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace mg_pong;

public class FiniteStateMachine<T> {
    public Dictionary<T, Action<float>> States { get; set; }
    public T CurrentState { get; set; }

    public FiniteStateMachine() {
        States = new Dictionary<T, Action<float>>();
    }

    public void AddState(T name, Action<float> update) {
        States.Add(name, update);
    }

    public void Update(float deltaTime) {
        States[CurrentState](deltaTime);
    }
}
