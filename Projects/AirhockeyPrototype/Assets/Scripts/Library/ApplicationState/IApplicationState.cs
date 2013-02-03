//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012-2013 Edelweiss Interactive (http://edelweissinteractive.com)
//

using System;
using System.Collections.Generic;

namespace Edelweiss.ApplicationState {
	
	public interface IApplicationState : IComparable <IApplicationState> {
		int GetUniqueInstanceID ();
	}
}
