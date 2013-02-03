//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012-2013 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;
using Edelweiss.Pattern;

namespace Edelweiss.ApplicationState {

	public abstract class ApplicationState <G> : Manager <G>, IApplicationState where G : MonoBehaviour {
		
		public override bool IsDestroyedOnLoad {
			get {
				return (true);
			}
		}
		
		protected override void InitializeManager () {
			ApplicationStateManager.Instance.RegisterApplicationState
				(this,
				 PreEnter,
				 Enter,
				 PreLeave,
				 Leave,
				 PerformUpdate,
				 PerformLateUpdate);
			InitializeApplicationState ();
		}
		
		private void OnDestroy () {
			if (ApplicationStateManager.IsInstanceSet) {
				ApplicationStateManager.Instance.UnregisterApplicationState (this);
			}
			OnDestroyApplicationState ();
		}
		
		protected virtual void InitializeApplicationState () {
		}
		
		protected virtual void OnDestroyApplicationState () {
		}
		
		public int GetUniqueInstanceID () {
			return (gameObject.GetInstanceID ());
		}
		
		protected abstract void PreEnter ();
		protected abstract void Enter ();
		protected abstract void PreLeave ();
		protected abstract void Leave ();
		
		protected abstract void PerformUpdate ();
		protected abstract void PerformLateUpdate ();

		public int CompareTo (IApplicationState a_Other) {
			int l_Result = GetUniqueInstanceID ().CompareTo (a_Other.GetUniqueInstanceID ());
			return (l_Result);
		}
	}
}
