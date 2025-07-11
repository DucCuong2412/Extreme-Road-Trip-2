using System.Collections.Generic;
using UnityEngine;

namespace Prime31
{
	public class FacebookEventListener : MonoBehaviour
	{
		private void OnEnable()
		{
			FacebookManager.sessionOpenedEvent += sessionOpenedEvent;
			FacebookManager.loginFailedEvent += loginFailedEvent;
			FacebookManager.dialogCompletedWithUrlEvent += dialogCompletedWithUrlEvent;
			FacebookManager.dialogFailedEvent += dialogFailedEvent;
			FacebookManager.graphRequestCompletedEvent += graphRequestCompletedEvent;
			FacebookManager.graphRequestFailedEvent += facebookCustomRequestFailed;
			FacebookManager.facebookComposerCompletedEvent += facebookComposerCompletedEvent;
			FacebookManager.reauthorizationFailedEvent += reauthorizationFailedEvent;
			FacebookManager.reauthorizationSucceededEvent += reauthorizationSucceededEvent;
			FacebookManager.shareDialogFailedEvent += shareDialogFailedEvent;
			FacebookManager.shareDialogSucceededEvent += shareDialogSucceededEvent;
		}

		private void OnDisable()
		{
			FacebookManager.sessionOpenedEvent -= sessionOpenedEvent;
			FacebookManager.loginFailedEvent -= loginFailedEvent;
			FacebookManager.dialogCompletedWithUrlEvent -= dialogCompletedWithUrlEvent;
			FacebookManager.dialogFailedEvent -= dialogFailedEvent;
			FacebookManager.graphRequestCompletedEvent -= graphRequestCompletedEvent;
			FacebookManager.graphRequestFailedEvent -= facebookCustomRequestFailed;
			FacebookManager.facebookComposerCompletedEvent -= facebookComposerCompletedEvent;
			FacebookManager.reauthorizationFailedEvent -= reauthorizationFailedEvent;
			FacebookManager.reauthorizationSucceededEvent -= reauthorizationSucceededEvent;
			FacebookManager.shareDialogFailedEvent -= shareDialogFailedEvent;
			FacebookManager.shareDialogSucceededEvent -= shareDialogSucceededEvent;
		}

		private void sessionOpenedEvent()
		{
			UnityEngine.Debug.Log("Successfully logged in to Facebook");
		}

		private void loginFailedEvent(P31Error error)
		{
			UnityEngine.Debug.Log("Facebook login failed: " + error);
		}

		private void dialogCompletedWithUrlEvent(string url)
		{
			UnityEngine.Debug.Log("dialogCompletedWithUrlEvent: " + url);
		}

		private void dialogFailedEvent(P31Error error)
		{
			UnityEngine.Debug.Log("dialogFailedEvent: " + error);
		}

		private void facebokDialogCompleted()
		{
			UnityEngine.Debug.Log("facebokDialogCompleted");
		}

		private void graphRequestCompletedEvent(object obj)
		{
			UnityEngine.Debug.Log("graphRequestCompletedEvent");
			Utils.logObject(obj);
		}

		private void facebookCustomRequestFailed(P31Error error)
		{
			UnityEngine.Debug.Log("facebookCustomRequestFailed failed: " + error);
		}

		private void facebookComposerCompletedEvent(bool didSucceed)
		{
			UnityEngine.Debug.Log("facebookComposerCompletedEvent did succeed: " + didSucceed);
		}

		private void reauthorizationSucceededEvent()
		{
			UnityEngine.Debug.Log("reauthorizationSucceededEvent");
		}

		private void reauthorizationFailedEvent(P31Error error)
		{
			UnityEngine.Debug.Log("reauthorizationFailedEvent: " + error);
		}

		private void shareDialogFailedEvent(P31Error error)
		{
			UnityEngine.Debug.Log("shareDialogFailedEvent: " + error);
		}

		private void shareDialogSucceededEvent(Dictionary<string, object> dict)
		{
			UnityEngine.Debug.Log("shareDialogSucceededEvent");
			Utils.logObject(dict);
		}
	}
}
