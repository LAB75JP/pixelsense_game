using UnityEngine;
using System.Collections;
using Edelweiss.Pattern;

public class CameraManager : Manager <CameraManager> {

	public Camera mainCamera;
	
	public override bool IsDestroyedOnLoad {
		get {
			return (true);
		}
	}
	
	protected override void InitializeManager () {
		Quaternion l_OffsetRotation = Quaternion.identity;
		
		if (NGUIGameLayoutManager.Instance.CurrentScreenRotation == NGUIGameLayoutManager.ScreenRotationEnum.AntiClockwise0) {
			l_OffsetRotation = Quaternion.identity;
		} else if (NGUIGameLayoutManager.Instance.CurrentScreenRotation == NGUIGameLayoutManager.ScreenRotationEnum.AntiClockwise90) {
			l_OffsetRotation = Quaternion.Euler (0.0f, 0.0f, -90.0f);
		} else if (NGUIGameLayoutManager.Instance.CurrentScreenRotation == NGUIGameLayoutManager.ScreenRotationEnum.AntiClockwise180) {
			l_OffsetRotation = Quaternion.Euler (0.0f, 0.0f, -180.0f);
		} else if (NGUIGameLayoutManager.Instance.CurrentScreenRotation == NGUIGameLayoutManager.ScreenRotationEnum.AntiClockwise270) {
			l_OffsetRotation = Quaternion.Euler (0.0f, 0.0f, -270.0f);
		}
		
		mainCamera.transform.localRotation = l_OffsetRotation;
	}
}
