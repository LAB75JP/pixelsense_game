//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PoolManager <M, G> : Manager <M> where M : MonoBehaviour {

	private List <G> m_Pool = new List <G> ();
	
	public void Initialize (int a_Capacity) {
		for (int i = 0; i < a_Capacity; i = i + 1) {
			G l_NewObject = CreateObject ();
			AddToPool (l_NewObject);
		}
	}
	
	public G NewObject () {
		G l_Result;
		if (m_Pool.Count > 0) {
			l_Result = m_Pool [m_Pool.Count - 1];
			m_Pool.RemoveAt (m_Pool.Count - 1);
		} else {
			l_Result = CreateObject ();
		}
		
		InitializeObject (l_Result);
		return (l_Result);
	}
	
	public void AddToPool (G a_Object) {
		ClearObject (a_Object);
		m_Pool.Add (a_Object);
	}
	
	protected abstract G CreateObject ();
	protected abstract void InitializeObject (G a_Object);
	protected abstract void ClearObject (G a_Object);
}
