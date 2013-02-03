//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2011-2013 Edelweiss Interactive (http://edelweissinteractive.com)
//

namespace Edelweiss.Transition {

	/// <summary>
	/// a_Value needs to be in [0.0f, 1.0f]. 
	/// </summary>
	public delegate float TransitionDelegate (float a_From, float a_To, float a_Value);
}