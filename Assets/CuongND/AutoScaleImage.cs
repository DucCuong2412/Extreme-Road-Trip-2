using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScaleImage : MonoBehaviour
{
    [Header("Scale Settings")]
    public bool maintainAspectRatio = true;
    public bool scaleToFitScreen = true;
    public float scaleFactor = 1f; // Hệ số scale bổ sung

    private Camera mainCamera;
    private MeshRenderer spriteRenderer;
    private Vector3 originalScale;
    private float originalScreenWidth;
    private float originalScreenHeight;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<MeshRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found!");
            return;
        }

        // Lưu scale gốc và kích thước màn hình ban đầu
        originalScale = transform.localScale;
        originalScreenWidth = Screen.width;
        originalScreenHeight = Screen.height;

        // Scale ngay lập tức
        AutoScaleToScreen();
    }

    void Update()
    {
        AutoScaleToScreen();
    }

    void AutoScaleToScreen()
    {

        if (mainCamera == null || spriteRenderer == null)
        {
            Debug.Log($"main camera: {mainCamera}");
            Debug.Log($"spriteRenderer: {spriteRenderer}");

            return;
        }
        // Kiểm tra xem màn hình có thay đổi không
        if (Screen.width != originalScreenWidth || Screen.height != originalScreenHeight)
        {
            originalScreenWidth = Screen.width;
            originalScreenHeight = Screen.height;
        }

        if (scaleToFitScreen)
        {
            // Lấy kích thước màn hình theo world units
            float screenHeight = mainCamera.orthographicSize * 2f;
            float screenWidth = screenHeight * mainCamera.aspect;

            // Lấy kích thước sprite gốc
            Bounds spriteBounds = spriteRenderer.bounds;
            float spriteWidth = spriteBounds.size.x / transform.localScale.x;
            float spriteHeight = spriteBounds.size.y / transform.localScale.y;

            float scaleX = screenWidth / spriteWidth;
            float scaleY = screenHeight / spriteHeight;

            if (maintainAspectRatio)
            {
                // Giữ tỉ lệ khung hình, scale theo chiều nhỏ hơn
                float uniformScale = Mathf.Min(scaleX, scaleY);
                transform.localScale = originalScale * uniformScale * scaleFactor;
            }
            else
            {
                // Scale theo cả 2 chiều
                transform.localScale = new Vector3(
                    originalScale.x * scaleX * scaleFactor,
                    originalScale.y * scaleY * scaleFactor,
                    originalScale.z
                );
            }
        }
    }

    // Hàm để gọi từ bên ngoài nếu cần
    public void ForceUpdateScale()
    {
        AutoScaleToScreen();
    }

    // Hàm để set scale factor mới
    public void SetScaleFactor(float newScaleFactor)
    {
        scaleFactor = newScaleFactor;
        AutoScaleToScreen();
    }
}
