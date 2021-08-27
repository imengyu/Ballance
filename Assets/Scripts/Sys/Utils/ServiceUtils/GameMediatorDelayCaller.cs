using System.Collections.Generic;
using Ballance2.Sys.Services;
using UnityEngine;

namespace Ballance2.Sys.Utils.ServiceUtils
{

    public class GameMediatorDelayCaller : MonoBehaviour
    {
        public GameMediator GameMediator;
        
        private void FixedUpdate() {
            if(data.Count > 0) {
                for(int i = data.Count - 1; i >= 0; i--) {
                    var d = data[i];
                    d.tick --;
                    if(d.tick <= 0) {
                        switch(d.type) {
                            case GameMediatorDelayCallType.NormalEvent:
                                GameMediator.DispatchGlobalEvent(d.name, d.filter, d.args);
                                break;
                            case GameMediatorDelayCallType.SingleEvent:
                                GameMediator.NotifySingleEvent(d.name, d.args);
                                break;
                        }
                        data.RemoveAt(i);
                    }
                }
            }
        }

        private enum GameMediatorDelayCallType {
            SingleEvent,
            NormalEvent
        }
        private List<DelayCallData> data = new List<DelayCallData>();
        private class DelayCallData {
            public GameMediatorDelayCallType type;
            public string name;
            public int tick; 
            public string filter; 
            public object[] args;
        }

        public void AddDelayCallSingle(string name, float delayeSecond, object[] args) {
            var d = new DelayCallData();
            d.type = GameMediatorDelayCallType.SingleEvent;
            d.name = name;
            d.tick = (int)(60 * delayeSecond);
            d.args = args;
            data.Add(d);
        }
        public void AddDelayCallNormal(string name, string filter, float delayeSecond, object[] args) {
            var d = new DelayCallData();
            d.type = GameMediatorDelayCallType.NormalEvent;
            d.name = name;
            d.filter = filter;
            d.tick = (int)(60 * delayeSecond);
            d.args = args;
            data.Add(d);
        }
    }
}