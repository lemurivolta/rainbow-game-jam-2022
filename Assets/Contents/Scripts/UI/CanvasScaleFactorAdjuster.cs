using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Assertions;

public class CanvasScaleFactorAdjuster : MonoBehaviour
{
    public Camera MainCamera;
    private CanvasScaler canvasScaler;
    private PixelPerfectCamera pixelPerfectCamera;

    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        Assert.IsNotNull(canvasScaler);
        pixelPerfectCamera = MainCamera.GetComponent<PixelPerfectCamera>();
        Assert.IsNotNull(pixelPerfectCamera);
        AdjustScalingFactor();
    }

    void LateUpdate()
    {
        AdjustScalingFactor();
    }

    void AdjustScalingFactor()
    {
        canvasScaler.scaleFactor = pixelPerfectCamera.pixelRatio;
    }

}