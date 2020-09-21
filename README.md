# BioBridge

Socket data callback (Program.cs line 71,78)
--------
```csharp
public static void OnCameraData(CameraData data)
{
    cameraStatus = data.Status;
    // handle data callback here
}

public static void OnFingerprintData(FingerprintData data)
{
    fingerprintStatus = data.Status;
    // handle data callback here 
}
```