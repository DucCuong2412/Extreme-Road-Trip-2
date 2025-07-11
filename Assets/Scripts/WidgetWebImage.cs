public class WidgetWebImage : IWidgetLoader
{
	private string _filename;

	private MetroStretch _streched;

	private MetroPng _loadedImage;

	private WidgetWebImage(string filename, string url, MetroStretch strech)
	{
		_filename = filename;
		_streched = strech;
		if (!string.IsNullOrEmpty(url))
		{
			PictureManager.StorePicture(filename, url);
		}
	}

	public bool IsLoaded()
	{
		bool flag = false;
		if (_loadedImage == null)
		{
			flag = PictureManager.IsPictureLoaded(_filename);
			if (flag)
			{
				_loadedImage = PictureManager.CreateMetroPicture(_filename);
				_loadedImage.SetStretch(_streched);
				flag = _loadedImage.IsLoaded();
			}
		}
		else
		{
			flag = _loadedImage.IsLoaded();
		}
		return flag;
	}

	public MetroWidget GetLoadedWidget()
	{
		return _loadedImage;
	}

	public static WidgetLoaderContainer Create(string filename, string url, MetroWidget loading, MetroStretch strech)
	{
		WidgetWebImage streamedWidget = new WidgetWebImage(filename, url, strech);
		return WidgetLoaderContainer.Create(streamedWidget, loading);
	}
}
