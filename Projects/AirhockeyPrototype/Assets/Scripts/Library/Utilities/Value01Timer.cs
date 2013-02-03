//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2013 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;

namespace Edelweiss.Utilities {

	public class Value01Timer {
		
		private float m_InverseDuration;
		private float m_Duration;
		public float Duration {
			get {
				return (m_Duration);
			}
		}
		
		private float m_PassedTime;
		public float PassedTime {
			get {
				return (m_PassedTime);
			}
		}
		
		private float m_Value01;
		public float Value01 {
			get {
				return (m_Value01);
			}
		}
		
		public bool Ready {
			get {
				return (m_Duration != 0.0f);
			}
		}
		
		public bool Done {
			get {
				return (!Ready || (m_Value01 == 1.0f));
			}
		}
		
		public void Initialize (float a_Duration) {
			if (a_Duration <= 0.0f) {
				throw (new System.ArgumentException ("Initializations are only valid with a strictly greater than zero duration."));
			}
			
			m_Duration = a_Duration;
			m_InverseDuration = 1.0f / m_Duration;
			m_PassedTime = 0.0f;
			m_Value01 = 0.0f;
		}
		
		public void Reset () {
			m_Duration = 0.0f;
			m_InverseDuration = 0.0f;
			m_PassedTime = 0.0f;
			m_Value01 = 0.0f;
		}
		
		public void Progress (float a_DeltaTime) {
			if (!Ready) {
				throw (new System.InvalidOperationException ("Progress can only be called if the timer is ready."));
			}
			
			m_PassedTime = m_PassedTime + a_DeltaTime;
			m_PassedTime = Mathf.Clamp (m_PassedTime, 0.0f, m_Duration);
			m_Value01 = m_PassedTime * m_InverseDuration;
		}
	}
}