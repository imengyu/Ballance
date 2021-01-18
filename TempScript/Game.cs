#pragma warning disable
using UnityEngine;
using System.Collections.Generic;

namespace MaxyGames.Generated {
	public class test : MaxyGames.RuntimeBehaviour {
		private MaxyGames.Runtime.EventCoroutine coroutine1;
		private MaxyGames.Runtime.EventCoroutine coroutine2;

		void Start() {
			coroutine2.Run();
		}

		System.Collections.IEnumerable _ExecuteCoroutineEvent(int uid) {
			switch(uid) {
				case -90932: {
					Ballance2.System.GameManager.GameMediator.RegisterGlobalEvent(Ballance2.System.Bridge.GameEventNames.EVENT_BEFORE_GAME_QUIT);
					yield return coroutine1.Run();
				}
				break;
				case -97236: {
					Ballance2.System.GameManager.GameMediator.RegisterEventHandler(null, Ballance2.System.GameManager.GameMediator.RegisterGlobalEvent(Ballance2.System.Bridge.GameEventNames.EVENT_BEFORE_GAME_QUIT).EventName, "Test", null);
				}
				break;
			}
			yield break;
		}

		public override void OnAwake() {
			coroutine1 = new MaxyGames.Runtime.EventCoroutine(this, _ExecuteCoroutineEvent(-97236));
			coroutine2 = new MaxyGames.Runtime.EventCoroutine(this, _ExecuteCoroutineEvent(-90932));
		}
	}
}
