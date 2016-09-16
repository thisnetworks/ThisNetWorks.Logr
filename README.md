# ThisNetWorks.Logr

A lightweight Cross platform Logger for Xamarin.

Provides a Logr to write console messages to Android or iOS.

Logr does not need to be initialized, it works straight out of the box.

```
Logr.Debug("Debug Message");
Logr.Debug("Debug Message with string formatting {0}", args);
Logr.Error(e, "Error Message {1}", args);
```

If Xamarin.Insights is installed (and initialized) Logr will send Logr.Error message to Insights. Xamarin Insights should be initialized before writing any logs.

On Android Logr will also send the last 500 lines of logcat (configurable) to Insights. 
By default it will only do this in Debug mode, but can be turned on for Release if you're trying to pin down an exception. You could also turn it on in specific places in your code and turn it off again afterwards.

To configure Logr, for example, to disable Insights reporting, access the LogrWriter Settings class.

```
LogrWriterInstance.Instance.Settings.Insights.LogNumberOfLines = 10;
LogrWriterInstance.Instance.Settings.Insights.ShouldTryReportToInsights = false;
LogrWriterInstance.Instance.Settings.Insights.OnlySendLogFileInDebug = true;
```

To swap out the LogrWriter to your own implementation, inherit from ILogrWriter, and replace the instance.

```
var logr = new CustomLogrWriter();
LogrWriterInstance.Instance = logr;
```
Version 1.0.1

Added object.ToLogr Extension Method. When used on an object this will direct the .ToString() output of the object to the Logr.Debug console output. Also added functionality to this method to catch the calling method, and line number, and report this to the console. I can't catch the variable name however so have been using nameof(object) to send an extra message with the variable name to the Logr.

```
var stringToLog = "I'm a string":
stringToLog.ToLogr();
stringToLog.ToLogr(nameof(stringToLog));

```


## Nuget

https://www.nuget.org/packages/Xamarin.Logr

Add ThisNetWorks.Logr to your PCL and platform projects. Currently supported platforms are:

- Xamarin.Android
- Xamarin.iOS
