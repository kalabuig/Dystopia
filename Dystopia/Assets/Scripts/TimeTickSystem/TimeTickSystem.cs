using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class TimeTickSystem
{
    public static event EventHandler<OnTickEventArgs> OnTick; //On Tick event handler

    private static int tick; //ticks counter
    private static GameObject timeTickSystemGO; //TimeTickSystem GameObject

    private const float TICK_TIMER_MAX = .2f; //5 ticks per second

    public static float GetSecondsPerTick() {
        return TICK_TIMER_MAX;
    }

    public static float GetTicksPerSecond() {
        return 1f / TICK_TIMER_MAX;
    }

    //Create the tick system gameobject
    public static void Create() {
        if(timeTickSystemGO == null) {
            timeTickSystemGO = new GameObject("TimeTickSystem");
            timeTickSystemGO.AddComponent<TimeTickSystemObject>();
        }
    }

    //Get tick
    public static int GetTick() {
        return tick;
    }

    //EventArgs 
    public class OnTickEventArgs : EventArgs {
        public int tick;
    }

    //Class to create the TimetickSystem GameObject in runtime
    private class TimeTickSystemObject : MonoBehaviour {
        private float tickTimer; //time elapsed
        private void Awake() {
            tick = 0;
        }

        private void Update() {
            tickTimer += Time.deltaTime;
            if(tickTimer >= TICK_TIMER_MAX) {
                tickTimer -= TICK_TIMER_MAX;
                tick++;
                if(OnTick != null) { //if there are subscribers
                    OnTick(this, new OnTickEventArgs { tick = tick}); //Launch tick event to subscribers
                }
            }
        }
    }
}
