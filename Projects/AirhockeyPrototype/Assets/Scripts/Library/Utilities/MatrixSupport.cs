//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012-2013 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;

namespace Edelweiss.Utilities {

	public class MatrixSupport {
	
		public static Matrix4x4 Lerp (Matrix4x4 a_From, Matrix4x4 a_To, float a_Value) {
			Matrix4x4 l_Result = new Matrix4x4 ();
			for (int i = 0; i < 16; i = i + 1) {
				l_Result [i] = Mathf.Lerp (a_From [i], a_To [i], a_Value);
			}
			return (l_Result);
		}
	}
}