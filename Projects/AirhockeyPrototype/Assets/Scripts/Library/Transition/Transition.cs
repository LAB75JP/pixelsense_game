//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2011-2012 Edelweiss Interactive (http://edelweissinteractive.com)
//

using UnityEngine;

namespace Edelweiss.Utils {

	public class Transition {
		public static TransitionDelegate GetTransitionDelegate (TransitionEnum a_TransitionEnum) {
			switch (a_TransitionEnum) {
				case TransitionEnum.LinearInOut:
					return (LinearInOut);
				case TransitionEnum.SmoothInOut:
					return (SmoothInOut);
				case TransitionEnum.LinearInSmoothOut:
					return (LinearInSmoothOut);
				case TransitionEnum.SmoothInLinearOut:
					return (SmoothInLinearOut);
				default:
					return (LinearInOut);
			}
		}
		
		private static float LinearInOut (float a_From, float a_To, float a_Value) {
			return (Mathf.Lerp (a_From, a_To, a_Value));
		}
		
		private static float SmoothInOut (float a_From, float a_To, float a_Value) {
				// f(x) = -2x^3 + 3x^2
				// f(0) = 0
				// f'(0) = 0
				// f(1) = 1
				// f'(1) = 0
			float l_Value = a_Value * a_Value * (3.0f - (2.0f * a_Value));
			return (Mathf.Lerp (a_From, a_To, l_Value));
		}
		
		private static float LinearInSmoothOut (float a_From, float a_To, float a_Value) {
				// f(x) = -x^3 + x^2 + x
				// f(0) = 0
				// f'(0) = 1
				// f(1) = 1
				// f'(1) = 0
			float l_Value = a_Value * (1.0f + (a_Value * (1.0f - a_Value)));
			return (Mathf.Lerp (a_From, a_To, l_Value));
		}
		
		private static float SmoothInLinearOut (float a_From, float a_To, float a_Value) {
				// f(x) = -x^3 + 2x^2
				// f(0) = 0
				// f'(0) = 0
				// f(1) = 1
				// f'(1) = 1
			float l_Value = a_Value * a_Value * (2.0f - a_Value);
			return (Mathf.Lerp (a_From, a_To, l_Value));
		}
	}
}