using UnityEditor;
using UnityEngine;

namespace IngameDebugConsole
{
	[CustomEditor( typeof( DebugLogManager ) )]
	public class DebugLogManagerEditor : Editor
	{
		private SerializedProperty singleton;

	}
}
