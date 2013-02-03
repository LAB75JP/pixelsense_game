//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012-2013 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Edelweiss.Pattern;
using Edelweiss.Utilities;

namespace Edelweiss.ApplicationState {
	
	public class ApplicationStateManager : Manager <ApplicationStateManager> {
		
		private bool m_IsStateChanging = false;
		public bool IsStateChanging {
			get {
				return (m_IsStateChanging);
			}
		}
		
		private IApplicationState m_CurrentApplicationState;
		public IApplicationState CurrentApplicationState {
			get {
				return (m_CurrentApplicationState);
			}
			set {
				if (value == null) {
					throw (new System.InvalidOperationException ("Unable to assign null as the current application state."));
				}
				if (m_IsStateChanging) {
					throw (new System.InvalidOperationException ("Unable to assign a new application state while it is changing."));
				}
				if (CurrentApplicationState != value) {
					if (m_CurrentApplicationState != null) {
						m_CurrentApplicationStateDelegates.leaveDelegate ();
					}
					m_CurrentApplicationState = value;
					m_CurrentApplicationStateDelegates = m_ApplicationStatesDelegates [m_CurrentApplicationState];
					m_CurrentApplicationStateDelegates.enterDelegate ();
				}
			}
		}
		
		private ApplicationStateDelegates m_CurrentApplicationStateDelegates;
		
		private IApplicationState m_TargetApplicationState;
		public IApplicationState TargetApplicationState {
			get {
				return (m_TargetApplicationState);
			}
		}
		
		private ApplicationStateDelegates m_TargetApplicationStateDelegates;
		
		private Dictionary <IApplicationState, ApplicationStateDelegates> m_ApplicationStatesDelegates = new Dictionary <IApplicationState, ApplicationStateDelegates> ();
		
		private Value01Timer m_Value01Timer = new Value01Timer ();
		public float PassedTransitionTime {
			get {
				return (m_Value01Timer.PassedTime);
			}
		}
		
		public float TransitionDuration {
			get {
				return (m_Value01Timer.Duration);
			}
		}
		
		public float TransitionValue01 {
			get {
				return (m_Value01Timer.Value01);
			}
		}
		
		public override bool IsDestroyedOnLoad {
			get {
				return (true);
			}
		}

		protected override void InitializeManager () {
		}
		
		public void RegisterApplicationState
			(IApplicationState a_ApplicationState,
			 ApplicationStateDelegate a_PreEnterDelegate,
			 ApplicationStateDelegate a_EnterDelegate,
			 ApplicationStateDelegate a_PreLeaveDelegate,
			 ApplicationStateDelegate a_LeaveDelegate,
			 ApplicationStateDelegate a_UpdateDelegate,
			 ApplicationStateDelegate a_LateUpdateDelegate)
		{
			ApplicationStateDelegates l_Delegates =
				new ApplicationStateDelegates (
					a_PreEnterDelegate,
					a_EnterDelegate,
					a_PreLeaveDelegate,
					a_LeaveDelegate,
					a_UpdateDelegate,
					a_LateUpdateDelegate);
			m_ApplicationStatesDelegates.Add (a_ApplicationState, l_Delegates);
		}
		
		public void UnregisterApplicationState (IApplicationState a_ApplicationState) {
			m_ApplicationStatesDelegates.Remove (a_ApplicationState);
		}
		
		public void TransitionToMode (IApplicationState a_ApplicationState, float a_TransitionDuration) {
			if (a_ApplicationState == null) {
				throw (new System.ArgumentNullException ("Unable to transition to an application state that is null."));
			}
			if (m_IsStateChanging) {
				throw (new System.InvalidOperationException ("Unable to start an application state transition while it is changing."));
			}
			if (m_CurrentApplicationState == null) {
				throw (new System.InvalidOperationException ("No application state set, that's why a transition is not valid."));
			}
			if (a_TransitionDuration <= 0.0f) {
				throw (new System.ArgumentNullException ("Transition durations have to be strictly greater than zero."));
			}
			if (m_CurrentApplicationState == a_ApplicationState) {
				throw (new System.ArgumentException ("New state is invalid because it is identical to the current one."));
			}
			
			StartCoroutine (TransitionToModeCoroutine (a_ApplicationState, a_TransitionDuration));
		}
		
		private IEnumerator TransitionToModeCoroutine (IApplicationState a_ApplicationState, float a_TransitionDuration) {
			m_IsStateChanging = true;
			m_TargetApplicationState = a_ApplicationState;
			m_TargetApplicationStateDelegates = m_ApplicationStatesDelegates [m_TargetApplicationState];
			m_Value01Timer.Initialize (a_TransitionDuration);
			
			if (m_CurrentApplicationState != null) {
				m_CurrentApplicationStateDelegates.preLeaveDelegate ();
			}
			m_TargetApplicationStateDelegates.preEnterDelegate ();
			
			while (PassedTransitionTime < TransitionDuration) {
				yield return (null);
				m_Value01Timer.Progress (Time.deltaTime);
			}
			
			if (m_CurrentApplicationState != null) {
				m_CurrentApplicationStateDelegates.leaveDelegate ();
			}
			m_CurrentApplicationState = a_ApplicationState;
			m_CurrentApplicationStateDelegates = m_ApplicationStatesDelegates [m_CurrentApplicationState];
			m_CurrentApplicationStateDelegates.enterDelegate ();
			
			m_IsStateChanging = false;
			m_TargetApplicationState = null;
			m_TargetApplicationStateDelegates = null;
			m_Value01Timer.Reset ();
		}
		
		private void Update () {
			if (CurrentApplicationState != null) {
				m_CurrentApplicationStateDelegates.updateDelegate ();
			}
		}
		
		private void LateUpdate () {
			if (CurrentApplicationState != null) {
				m_CurrentApplicationStateDelegates.lateUpdateDelegate ();
			}
		}
		
		private class ApplicationStateDelegates {
			
			public ApplicationStateDelegates
				(ApplicationStateDelegate a_PreEnterDelegate,
				 ApplicationStateDelegate a_EnterDelegate,
				 ApplicationStateDelegate a_PreLeaveDelegate,
				 ApplicationStateDelegate a_LeaveDelegate,
				 ApplicationStateDelegate a_UpdateDelegate,
				 ApplicationStateDelegate a_LateUpdateDelegate)
			{
				preEnterDelegate = a_PreEnterDelegate;
				enterDelegate = a_EnterDelegate;
				preLeaveDelegate = a_PreLeaveDelegate;
				leaveDelegate = a_LeaveDelegate;
				updateDelegate = a_UpdateDelegate;
				lateUpdateDelegate = a_LateUpdateDelegate;
			}
			
			public ApplicationStateDelegate preEnterDelegate;
			public ApplicationStateDelegate enterDelegate;
			public ApplicationStateDelegate preLeaveDelegate;
			public ApplicationStateDelegate leaveDelegate;
			public ApplicationStateDelegate updateDelegate;
			public ApplicationStateDelegate lateUpdateDelegate;
		}
	}
}