//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2011-2013 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;

namespace Edelweiss.Pattern {

	public abstract class Manager <G> : MonoBehaviour where G : MonoBehaviour {
		
			// Singleton
		private static G s_Instance;
		public static G Instance {
			get {
				if (s_Instance == null) {
					G l_Instance = (G) FindObjectOfType (typeof (G));
					if (l_Instance == null) {
						Debug.LogError ("No " + typeof (G) + " found in the scene!");
					} else {
						SetInstance (l_Instance);
					}
				}
				return (s_Instance);
			}
		}
		
		public static bool IsInstanceSet {
			get {
				return (s_Instance != null);
			}
		}

		private static void SetInstance (G a_Instance) {
			s_Instance = a_Instance;
			
				// HACK: Cast needed to create this pattern.
			Manager <G> l_Instance = s_Instance as Manager <G>;
			if (!l_Instance.IsDestroyedOnLoad) {
				DontDestroyOnLoad (s_Instance.gameObject);
			}
			l_Instance.InitializeManager ();
		}
		
		public abstract bool IsDestroyedOnLoad {
			get;
		}
		
		private void Awake () {
			if (s_Instance == null) {
				SetInstance (this as G);
			} else if (s_Instance != this) {
				Debug.LogError ("More than one " + GetType ().Name + " component found in the scene!");
			}
		}
		
		protected abstract void InitializeManager ();
	}
}