using UnityEngine;
using System.Collections;
using Edelweiss.Pattern;

public class NGUIGameLayoutManager : Manager <NGUIGameLayoutManager> {
	
	public enum ScreenRotationEnum {
		AntiClockwise0,
		AntiClockwise90,
		AntiClockwise180,
		AntiClockwise270
	}
	
	public UIRoot nguiRoot;
	public Transform NGUIAnchor;
	public ScreenRotationEnum startScreenRotation;
	
	public const float c_FixedScreenHeight = 768.0f;
	public const float c_InvertedFixedScreenHeight = 1.0f / c_FixedScreenHeight;
	
	private ScreenRotationEnum m_CurrentScreenRotation;
	public ScreenRotationEnum CurrentScreenRotation {
		get {
			return (m_CurrentScreenRotation);
		}
		set {
			m_CurrentScreenRotation = value;
			if (NGUIAnchor != null) {
				Vector3 l_Rotation = NGUIAnchor.localRotation.eulerAngles;
				if (CurrentScreenRotation == ScreenRotationEnum.AntiClockwise0) {
					l_Rotation.z = 0.0f;
				} else if (CurrentScreenRotation == ScreenRotationEnum.AntiClockwise90) {
					l_Rotation.z = 90.0f;
				} else if (CurrentScreenRotation == ScreenRotationEnum.AntiClockwise180) {
					l_Rotation.z = 180.0f;
				} else if (CurrentScreenRotation == ScreenRotationEnum.AntiClockwise270) {
					l_Rotation.z = 270.0f;
				}
				NGUIAnchor.localRotation = Quaternion.Euler (l_Rotation);
			}
		}
	}
	
	public float ScreenWidth {
		get {
			float l_Result;
			if
				(CurrentScreenRotation == ScreenRotationEnum.AntiClockwise0 ||
				 CurrentScreenRotation == ScreenRotationEnum.AntiClockwise180)
			{
				l_Result = c_FixedScreenHeight * (float) Screen.width / (float) Screen.height;
			} else {
				l_Result = c_FixedScreenHeight;
			}
			return (l_Result);
		}
	}
	
	public float ScreenHeight {
		get {
			float l_Result;
			if
				(CurrentScreenRotation == ScreenRotationEnum.AntiClockwise0 ||
				 CurrentScreenRotation == ScreenRotationEnum.AntiClockwise180)
			{
				l_Result = c_FixedScreenHeight;
			} else {
				l_Result = c_FixedScreenHeight * (float) Screen.width / (float) Screen.height;
			}
			return (l_Result);
		}
	}
	
	public override bool IsDestroyedOnLoad {
		get {
			return (true);
		}
	}
	
	protected override void InitializeManager () {
		CurrentScreenRotation = startScreenRotation;
		
		if (nguiRoot.automatic) {
			Debug.LogError ("The current implementation assumes that the UIRoot is not set to automatic. It is changed automatically.");
			nguiRoot.automatic = false;
		}
		if (nguiRoot.manualHeight != c_FixedScreenHeight) {
			Debug.LogError ("The manual height of the gui is supposed to be " + c_FixedScreenHeight + ". It is changed automatically.");
			nguiRoot.manualHeight = Mathf.RoundToInt (c_FixedScreenHeight);
		}
	}
}
