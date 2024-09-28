using System;
using Debug = UnityEngine.Debug;
using Utils.Logger;

namespace UnityUtils
{
	public class UnityLogger : ILogger
	{
		public virtual void Log(Severity severity, string content)
		{
			switch (severity)
			{
				case Severity.Log:
					Debug.Log(content);
					break;
				case Severity.Warning:
					Debug.LogWarning(content);
					break;
				case Severity.Error:
					Debug.LogError(content);
					break;
				case Severity.Exception:
					Log(new Exception(content));
					break;
			}
		}

		public virtual void Log(Exception e)
		{
			Debug.LogException(e);
		}
	}
}
