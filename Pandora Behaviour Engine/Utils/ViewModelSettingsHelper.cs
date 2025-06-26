using System;
using System.Diagnostics;
using System.Reactive.Linq;

namespace Pandora.Utils;

public static class ViewModelSettingsHelper
{
	public static IDisposable BindSetting<T>(
		IObservable<T> source,
		Action<T> assignToSetting,
		TimeSpan? throttle = null)
	{
		var observable = throttle.HasValue ? source.Throttle(throttle.Value) : source;

		return observable.Subscribe(value =>
		{
			assignToSetting(value);
			Properties.GUISettings.Default.Save();
			Debug.WriteLine($"[Save] {assignToSetting.Method.Name}: {value}");
		});
	}
}
