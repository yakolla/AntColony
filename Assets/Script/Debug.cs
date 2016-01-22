#if UNITY_EDITOR
#define ENABLE_LOG
#endif

using UnityEngine;

/// 
/// It overrides UnityEngine.Debug to mute debug messages completely on a platform-specific basis.
/// 
/// Putting this inside of 'Plugins' foloder is ok.
/// 
/// Important:
///     Other preprocessor directives than 'UNITY_EDITOR' does not correctly work.
/// 
/// Note:
///     [Conditional] attribute indicates to compilers that a method call or attribute should be 
///     ignored unless a specified conditional compilation symbol is defined.
/// 
/// See Also: 
///     http://msdn.microsoft.com/en-us/library/system.diagnostics.conditionalattribute.aspx
/// 
/// 2012.11. @kimsama
/// 
public static class Debug
{
	public static bool isDebugBuild
	{
		get { return UnityEngine.Debug.isDebugBuild; }
	}
	
	[System.Diagnostics.Conditional("ENABLE_LOG")]
	public static void Log(object message)
	{
		UnityEngine.Debug.Log(message);
	}
	
	[System.Diagnostics.Conditional("ENABLE_LOG")]
	public static void Log(object message, UnityEngine.Object context)
	{
		UnityEngine.Debug.Log(message, context);
	}
	
	[System.Diagnostics.Conditional("ENABLE_LOG")]
	public static void LogError(object message)
	{
		UnityEngine.Debug.LogError(message);
	}
	
	[System.Diagnostics.Conditional("ENABLE_LOG")]
	public static void LogError(object message, UnityEngine.Object context)
	{
		UnityEngine.Debug.LogError(message, context);
	}
	
	[System.Diagnostics.Conditional("ENABLE_LOG")]
	public static void LogWarning(object message)
	{
		UnityEngine.Debug.LogWarning(message.ToString());
	}
	
	[System.Diagnostics.Conditional("ENABLE_LOG")]
	public static void LogWarning(object message, UnityEngine.Object context)
	{
		UnityEngine.Debug.LogWarning(message.ToString(), context);
	}
	
	[System.Diagnostics.Conditional("ENABLE_LOG")]
	public static void Assert(bool condition)
	{
		if (!condition) throw new System.Exception();
	}

	[System.Diagnostics.Conditional("ENABLE_LOG")]
	public static void DrawLine(Vector3 start, Vector3 end, Color color)
	{
		UnityEngine.Debug.DrawLine(start, end, color);
	}

	[System.Diagnostics.Conditional("ENABLE_LOG")]
	public static void DrawLine (Vector3 start, Vector3 end, Color color, float duration)
	{
		UnityEngine.Debug.DrawLine(start, end, color, duration);
	}

	[System.Diagnostics.Conditional("ENABLE_LOG")]
	public static void DrawRay(Vector3 start, Vector3 end, Color color)
	{
		UnityEngine.Debug.DrawRay(start, end, color);
	}



	public static void DebugBreak()
	{
		UnityEngine.Debug.DebugBreak();
	}
}
