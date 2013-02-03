//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012-2013 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;

namespace Edelweiss.NGUI {
	
	public abstract class ButtonClickSender : MonoBehaviour {
	
		private UIButtonMessage m_OnButtonClick;
		
		private void Start () {
			m_OnButtonClick = gameObject.AddComponent <UIButtonMessage> ();
			m_OnButtonClick.target = gameObject;
			m_OnButtonClick.trigger = UIButtonMessage.Trigger.OnClick;
			m_OnButtonClick.functionName = "ButtonClickAction";
			m_OnButtonClick.includeChildren = false;
		}
		
		protected abstract void ButtonClickAction ();
	}
}