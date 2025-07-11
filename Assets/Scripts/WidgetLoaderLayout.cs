public class WidgetLoaderLayout : IWidgetLoader
{
	public delegate bool IsLoadedDelegate();

	public delegate MetroWidget GetWidget();

	private IsLoadedDelegate _isLoaded;

	private GetWidget _createWidget;

	public WidgetLoaderLayout(GetWidget createWidget, IsLoadedDelegate isLoaded)
	{
		_isLoaded = isLoaded;
		_createWidget = createWidget;
	}

	public bool IsLoaded()
	{
		if (_isLoaded != null)
		{
			return _isLoaded();
		}
		return false;
	}

	public MetroWidget GetLoadedWidget()
	{
		return _createWidget();
	}

	public static WidgetLoaderContainer Create(GetWidget getWidget, IsLoadedDelegate isLoaded, MetroWidget loading = null)
	{
		WidgetLoaderLayout streamedWidget = new WidgetLoaderLayout(getWidget, isLoaded);
		return WidgetLoaderContainer.Create(streamedWidget, loading);
	}
}
