//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012-2013 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;

namespace Edelweiss.NGUI {
	
	public class ButtonIsPressedStateTracker : MonoBehaviour {
		
		private UIButtonMessage m_OnButtonPress;
		private UIButtonMessage m_OnButtonRelease;
		
		private void Start () {
			m_OnButtonPress = gameObject.AddComponent <UIButtonMessage> ();
			m_OnButtonPress.target = gameObject;
			m_OnButtonPress.trigger = UIButtonMessage.Trigger.OnPress;
			m_OnButtonPress.functionName = "OnButtonPress";
			m_OnButtonPress.includeChildren = false;
			
			m_OnButtonRelease = gameObject.AddComponent <UIButtonMessage> ();
			m_OnButtonRelease.target = gameObject;
			m_OnButtonRelease.trigger = UIButtonMessage.Trigger.OnRelease;
			m_OnButtonRelease.functionName = "OnButtonRelease";
			m_OnButtonRelease.includeChildren = false;
			
			enabled = false;
		}
		
		private void OnButtonPress () {
			enabled = true;
		}
		
		private void OnButtonRelease () {
			enabled = false;
		}
		
		public bool IsButtonPressed {
			get {
				return (enabled);
			}
		}
	}
}