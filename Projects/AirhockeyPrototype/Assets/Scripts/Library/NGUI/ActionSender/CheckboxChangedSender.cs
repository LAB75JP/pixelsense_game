//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;

public abstract class CheckboxChangedSender : MonoBehaviour {

	private void OnActivate (bool a_NewState) {
		CheckboxChangedAction (a_NewState);
	}
	
	protected abstract void CheckboxChangedAction (bool a_NewState);
}
