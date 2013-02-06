using UnityEngine;
using System.Collections;
using Edelweiss.Pattern;

public class AirhockeyManager : Manager <AirhockeyManager> {
	
	public float guiTransitionDuration = 0.25f;
	public float cameraFlightDuration = 3.0f;
	
	public int guiLayer = 8;
	public int GuiLayerMask {
		get {
			return (1 << guiLayer);
		}
	}
	
	public int puckAreaLayer = 9;
	public int PuckAreaLayerMask {
		get {
			return (1 << puckAreaLayer);
		}
	}
	
	public int puckLayer = 10;
	public int PuckLayerMask {
		get {
			return (1 << puckLayer);
		}
	}
	
	public SplineComponent cameraPath;
	public Transform introductionCameraTransform;
	public Transform playCameraTransform;
	
	public override bool IsDestroyedOnLoad {
		get {
			return (true);
		}
	}
	
	protected override void InitializeManager () {
	}
}
