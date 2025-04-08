#if UNITY_EDITOR || UNITY_STANDALONE
// Unity's Text component doesn't render <b> tag correctly on mobile devices
#define USE_BOLD_COMMAND_SIGNATURES
#endif

using UnityEngine;
using System.Text;

#if UNITY_EDITOR && UNITY_2021_1_OR_NEWER
using SystemInfo = UnityEngine.Device.SystemInfo;
using System.Globalization; // To support Device Simulator on Unity 2021.1+
#endif

// Manages the console commands, parses console input and handles execution of commands
// Supported method parameter types: int, float, bool, string, Vector2, Vector3, Vector4

// Helper class to store important information about a command
namespace IngameDebugConsole
{
	public static class DebugLogConsole
	{
        // CompareInfo used for case-insensitive command name comparison
        internal static readonly CompareInfo caseInsensitiveComparer = new CultureInfo( "en-US" ).CompareInfo;

        [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.SubsystemRegistration )] // Configurable Enter Play Mode: https://docs.unity3d.com/Manual/DomainReloading.html
		private static void ResetStatics()
		{
#if IDG_ENABLE_HELPER_COMMANDS || IDG_ENABLE_SYSINFO_COMMAND
			AddCommand( "sysinfo", "Prints system information", LogSystemInfo );
#endif
		}

		// Logs system information
		public static void LogSystemInfo()
		{
			StringBuilder stringBuilder = new StringBuilder( 1024 );
			stringBuilder.Append( "Rig: " ).AppendSysInfoIfPresent( SystemInfo.deviceModel ).AppendSysInfoIfPresent( SystemInfo.processorType )
				.AppendSysInfoIfPresent( SystemInfo.systemMemorySize, "MB RAM" ).Append( SystemInfo.processorCount ).Append( " cores\n" );
			stringBuilder.Append( "OS: " ).Append( SystemInfo.operatingSystem ).Append( "\n" );
			stringBuilder.Append( "GPU: " ).Append( SystemInfo.graphicsDeviceName ).Append( " " ).Append( SystemInfo.graphicsMemorySize )
				.Append( "MB " ).Append( SystemInfo.graphicsDeviceVersion )
				.Append( SystemInfo.graphicsMultiThreaded ? " multi-threaded\n" : "\n" );
			stringBuilder.Append( "Data Path: " ).Append( Application.dataPath ).Append( "\n" );
			stringBuilder.Append( "Persistent Data Path: " ).Append( Application.persistentDataPath ).Append( "\n" );
			stringBuilder.Append( "StreamingAssets Path: " ).Append( Application.streamingAssetsPath ).Append( "\n" );
			stringBuilder.Append( "Temporary Cache Path: " ).Append( Application.temporaryCachePath ).Append( "\n" );
			stringBuilder.Append( "Device ID: " ).Append( SystemInfo.deviceUniqueIdentifier ).Append( "\n" );
			stringBuilder.Append( "Max Texture Size: " ).Append( SystemInfo.maxTextureSize ).Append( "\n" );
			stringBuilder.Append( "Max Cubemap Size: " ).Append( SystemInfo.maxCubemapSize ).Append( "\n" );
			stringBuilder.Append( "Accelerometer: " ).Append( SystemInfo.supportsAccelerometer ? "supported\n" : "not supported\n" );
			stringBuilder.Append( "Gyro: " ).Append( SystemInfo.supportsGyroscope ? "supported\n" : "not supported\n" );
			stringBuilder.Append( "Location Service: " ).Append( SystemInfo.supportsLocationService ? "supported\n" : "not supported\n" );
			stringBuilder.Append( "Compute Shaders: " ).Append( SystemInfo.supportsComputeShaders ? "supported\n" : "not supported\n" );
			stringBuilder.Append( "Shadows: " ).Append( SystemInfo.supportsShadows ? "supported\n" : "not supported\n" );
			stringBuilder.Append( "Instancing: " ).Append( SystemInfo.supportsInstancing ? "supported\n" : "not supported\n" );
			stringBuilder.Append( "Motion Vectors: " ).Append( SystemInfo.supportsMotionVectors ? "supported\n" : "not supported\n" );
			stringBuilder.Append( "3D Textures: " ).Append( SystemInfo.supports3DTextures ? "supported\n" : "not supported\n" );
			stringBuilder.Append( "3D Render Textures: " ).Append( SystemInfo.supports3DRenderTextures ? "supported\n" : "not supported\n" );
			stringBuilder.Append( "2D Array Textures: " ).Append( SystemInfo.supports2DArrayTextures ? "supported\n" : "not supported\n" );
			stringBuilder.Append( "Cubemap Array Textures: " ).Append( SystemInfo.supportsCubemapArrayTextures ? "supported" : "not supported" );

			Debug.Log( stringBuilder.ToString() );

			// After typing sysinfo, the log that lists system information should automatically be expanded for better UX
			if( DebugLogManager.Instance )
				DebugLogManager.Instance.AdjustLatestPendingLog( true, true );
		}

		private static StringBuilder AppendSysInfoIfPresent( this StringBuilder sb, string info, string postfix = null )
		{
			if( info != SystemInfo.unsupportedIdentifier )
			{
				sb.Append( info );

				if( postfix != null )
					sb.Append( postfix );

				sb.Append( " " );
			}

			return sb;
		}

		private static StringBuilder AppendSysInfoIfPresent( this StringBuilder sb, int info, string postfix = null )
		{
			if( info > 0 )
			{
				sb.Append( info );

				if( postfix != null )
					sb.Append( postfix );

				sb.Append( " " );
			}

			return sb;
		}
	}
}
