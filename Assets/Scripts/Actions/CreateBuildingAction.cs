using UnityEngine;
using System.Collections;

public class CreateBuildingAction : ActionBehavior {

	public override System.Action GetClickAction ()
	{
		return delegate() {
			Debug.Log("Create Command Base Attempt");
		};
	}
}
