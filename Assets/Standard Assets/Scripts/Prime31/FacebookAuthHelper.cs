using System;

namespace Prime31
{
	public class FacebookAuthHelper
	{
		public Action afterAuthAction;

		public bool requiresPublishPermissions;

		private static FacebookAuthHelper _instance;

		public FacebookAuthHelper(bool requiresPublishPermissions, Action afterAuthAction)
		{
			_instance = this;
			this.requiresPublishPermissions = requiresPublishPermissions;
			this.afterAuthAction = afterAuthAction;
			FacebookManager.sessionOpenedEvent += sessionOpenedEvent;
			FacebookManager.loginFailedEvent += loginFailedEvent;
			if (requiresPublishPermissions)
			{
				FacebookManager.reauthorizationSucceededEvent += reauthorizationSucceededEvent;
				FacebookManager.reauthorizationFailedEvent += reauthorizationFailedEvent;
			}
		}

		~FacebookAuthHelper()
		{
			cleanup();
		}

		public void cleanup()
		{
			if (afterAuthAction != null)
			{
				FacebookManager.sessionOpenedEvent -= sessionOpenedEvent;
				FacebookManager.loginFailedEvent -= loginFailedEvent;
				if (requiresPublishPermissions)
				{
					FacebookManager.reauthorizationSucceededEvent -= reauthorizationSucceededEvent;
					FacebookManager.reauthorizationFailedEvent -= reauthorizationFailedEvent;
				}
			}
			_instance = null;
		}

		public void start()
		{
			FacebookAndroid.login();
		}

		private void sessionOpenedEvent()
		{
			if (requiresPublishPermissions && !FacebookAndroid.getSessionPermissions().Contains("publish_actions"))
			{
				FacebookAndroid.reauthorizeWithPublishPermissions(new string[1]
				{
					"publish_actions"
				}, FacebookSessionDefaultAudience.Everyone);
				return;
			}
			afterAuthAction();
			cleanup();
		}

		private void loginFailedEvent(P31Error error)
		{
			cleanup();
		}

		private void reauthorizationSucceededEvent()
		{
			afterAuthAction();
			cleanup();
		}

		private void reauthorizationFailedEvent(P31Error error)
		{
			cleanup();
		}
	}
}
