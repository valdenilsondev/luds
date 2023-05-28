using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System;

namespace Share.EventSystem {
	public class InstancesManager {
		private static List<object> instances = new List<object>();
		
		public static void AddInstance(object instance) {
			if(!instances.Contains(instance)) {
				instances.Add(instance);
			}
		}
		
		public static void RemoveInstance(object instance) {
			if(instances.Contains(instance)) {
				instances.Remove(instance);
			}
		}
		
		public static T GetInstance<T>(System.Type type) where T : class{
			for (int i = 0; i < instances.Count; i++) {
				if (instances[i].GetType() == type) {
					return (T)instances[i];
				}
			}
			
			return null;
		}
	}
}