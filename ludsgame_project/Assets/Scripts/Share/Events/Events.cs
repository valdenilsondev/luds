using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

namespace Share.EventsSystem {
	public interface IGameEvent { }
	public class PlayRecordingEvent : IGameEvent { }
	public class PauseRecordingEvent : IGameEvent { }
	public class StartRecordingEvent : IGameEvent { }
	public class StopRecordingEvent : IGameEvent { }
	public class PauseEvent : IGameEvent {}
	public class UnPauseEvent : IGameEvent {}

	public class AddScoreEvent : IGameEvent {}
	public class UpdateScoreText : IGameEvent {}
	public class GameOverEvent : IGameEvent {}
    public class RestartGameEvent : IGameEvent { }
	public class GameStart : IGameEvent {}
	public class PushGestureGame : IGameEvent {}
	public class PushGesturePresentation : IGameEvent {}

	public class Events {
		
		public delegate void GameEventsDelegate();
		
		private static GameEventsDelegate tempEvent;
		private static Type tempGameEventType;
		
		private static Dictionary<System.Type, GameEventsDelegate> delegates = new Dictionary<System.Type, GameEventsDelegate>();
		
		public static Events instance;
		
		public static void RaiseEvent<T>() where T : IGameEvent {
			tempGameEventType = typeof(T);
			
			if (delegates.TryGetValue(tempGameEventType, out tempEvent)) {
				tempEvent.Invoke();
			}
		}
		
		public static void AddListener<T>(GameEventsDelegate method) where T : IGameEvent {
			tempGameEventType = typeof(T);
			
			if (delegates.TryGetValue(tempGameEventType, out tempEvent)) {
				tempEvent += method;
				delegates[tempGameEventType] = tempEvent;
			} else {
				delegates.Add(tempGameEventType, method);
			}
		}
		
		public static void RemoveListener<T>(GameEventsDelegate method) where T : IGameEvent {
			GameEventsDelegate tempEvent;
			tempGameEventType = typeof(T);
			
			if (delegates.TryGetValue(tempGameEventType, out tempEvent)) {
				tempEvent -= method;
				if(tempEvent == null) {
					delegates.Remove(tempGameEventType);
				} else {
					delegates[tempGameEventType] = tempEvent;
				}
			}
		}
		
		public static void RemoveAllListeners<T>() where T : IGameEvent {
			GameEventsDelegate tempEvent;
			tempGameEventType = typeof(T);
			
			if (delegates.TryGetValue(tempGameEventType, out tempEvent)) {
				tempEvent = null;
				delegates[tempGameEventType] = tempEvent;
			}
		}
	}

}