using UnityEngine;
using System.Collections;

using Share.EventSystem;
using Share.KinectUtils;

namespace Sandbox.GameUtils {
	public class Singleton<T> where T : class, new() {
		
		private static T instance;
		public static T Instance {
			get {
				if (instance == null) {
					instance = new T();
				}
				
				return instance;
			}
		}
	}
	
	public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component {
		
		protected static T instance;
		public static T Instance {
			get {
				if (instance == null) {
					instance = FindObjectOfType<T>();
					if (instance == null) {
						Debug.LogError("Nenhum Game Object do tipo " + typeof(T).Name + " foi encontrado.");
					}
				}
				
				return instance;
			}
		}
	}
	
	public class SingletonGameEvent<T> : IGameEvent where T : class, new() {
		
		private static T instance;
		public static T Instance {
			get {
				if (instance == null) {
					instance = new T();
					InstancesManager.AddInstance(instance);
				}
				
				return instance;
			}
		}
	}
	
	public abstract class MonoBehaviourSingletonGameEvent<T> : MonoBehaviour, IGameEvent where T : Component {
		
		protected static T instance;
		
		protected void CreateInstance(T i) {
			instance = i;
			InstancesManager.AddInstance(instance);
		}
		
		// Ao implementar o metodo, chamar 'CreateInstance()' e passar o 'this' como parametro 
		protected abstract void Awake();
		
		public static T Instance {
			get {
				if (instance == null) {
					instance = FindObjectOfType<T>();
					InstancesManager.AddInstance(instance);
					if (instance == null) {
						Debug.LogError("Nenhum Game Object do tipo " + typeof(T).Name + " foi encontrado.");
					}
				}
				
				return instance;
			}
		}
	}
}