using DG.Tweening;
using Lean.Touch;
using UnityEngine;

public class MapCameraManager : Singleton<MapCameraManager>
{
    public LeanDragCamera leanDragCamera;
    public LeanPinchCamera leanPinchCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void MoveToSelected(Transform tf)
    {
        var localPos = transform.InverseTransformPoint(tf.transform.position);
        localPos += new Vector3(0, -1f, 0);
        var newPos = new Vector3(localPos.x, localPos.y, 0);
        var globalPos = transform.TransformPoint(newPos);
	    
        transform.DOMove(globalPos, GS.INS.cameraMoveToSelectedDuration).SetEase(Ease.InOutSine).OnUpdate(ConstraintCamera);
    }

    private void ConstraintCamera()
    {
        leanDragCamera.ConstrainCamera();
    }

    public void ZoomInToSelected(Transform waypointsMoverTransform)
    {
        MoveToSelected(waypointsMoverTransform);
        DOTween.To(() => leanPinchCamera.Zoom, x => leanPinchCamera.Zoom = x, 7f, GS.INS.cameraMoveToSelectedDuration).SetEase(Ease.InOutSine);
    }
}
