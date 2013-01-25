//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;

public class FrameCounter : MonoBehaviour {

	public float updateInterval = 1.0f;
	private int m_PassedFrames;
	private float m_PassedTime;
	private float m_LastFramesPerSecond;
	public float LastFramesPerSecond {
		get {
			return (m_LastFramesPerSecond);
		}
	}
	
	private void Update () {
		m_PassedTime = m_PassedTime + Time.deltaTime;
		m_PassedFrames = m_PassedFrames + 1;
		
		if (m_PassedTime > updateInterval) {
			m_LastFramesPerSecond = (float) m_PassedFrames / m_PassedTime;
			m_PassedTime = 0.0f;
			m_PassedFrames = 0;
		}
	}
}
