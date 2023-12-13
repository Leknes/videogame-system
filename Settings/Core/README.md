# Video Game Settings

This library is meant to be a both versatile and easy to use framework for implementing settings, like language, framerate limit or aspect ratio, into a video game.

## `SettingPreview<T>`
Lets get started by creating the preview of the setting that can modified externally. It implements the `ISettingPreview` interface as well as the `ISettingPreview<T>` interface. A validation method has to be provided for the preview and in order to manage the setting an instance of the `SettingValue<T>` class may be created.

## `SettingValue<T>`
This class requires an instance of the `SettingValue<T>` class to be instantiated. Then, this class act as a representation of the actual value of the setting. It implements the `ISettingValue` interface and the `ISettingValue<T>` interface. In order to manage applying and discarding of the setting value the class has to be activated which returns an `ISettingMeditator` which grant access to modification of the inner value.

## `EventSettingValue<T>`
This class derives from the `SettingValue<T>` class and implements the `IEventSettingValue<T>` interface. It fires an event containing the new setting value whenever the setting is modified.

## `Setting<T>`
If this differentiation between the preview and the actual value of the setting is an overkill for the application, the `Setting<T>` class can be used to integrate both features into one class. It implements every interface of the `SettingPreview<T>` and the `SettingValue<T>` class.
It also implements the `ISetting` interface.

Here is an example of a usable custom setting.

```cs
public class FramerateLimitSetting : Setting<int>
{
  public const MIN_FRAMERATE = 20;
  public const MAX_FRAMERATE = 180;

  protected override bool Validate(int value)
  {
    return value >= MIN_FRAMERATE && value <= MAX_FRAMERATE;
  }

  protected override int GetDefault() => 60;
}
```

This class can then be modified externally with the `PreviewValue` property. 
Still this class differentiates between this changable preview value and the `Value` property, 
that can only be changed by activating the class which will be explained later.

## `EventSetting<T>`

This class represents the equivalent of the `EventSettingValue<T>` which provides the functionality of the `EventSettingValue<T>` and the `SettingPreview<T>` class. Therefore it also implements the `IEventSettingValue<T>` interface.

## `EventSetting<T>`
However one may want to react on setting changes. For this occasion the `EventSetting<T>` class might become useful. 
Whenever the `Value` property of this class gets modified it fires the `Modified` event which encapsulated the `Action<T>` delegate.
 
## `SettingManager`

But in order to activate the settings a collection of settings has to be injected into a new instance of the `SettingManager` class.
This class is responsable for applying or discarding the preview values corresponding to the `Value` property of the setting that is
declared once the setting has been activated. It also fires the `Modified` event whenever any of the values has been changed.

## `SettingFacade`

For granting type-safe access to a collection of settings there also is the abstract `SettingFacade` class. 
This class implements the `IReadOnlyCollection<ISetting>` interface, the `IReadOnlyCollection<ISettingPreview>` interface and the `IReadOnlyCollection<ISettingValue>` interface and therefore can be directly fed into the `SettingManager` or the `SettingDefiner`.
It also captures every public setting that cannot be changed externally be default using reflection. 

## `SettingRenderer` 
In order to read every major information regarding the settings the `SettingRenderer` should be used.
This class is able to generate a `IReadOnlyDictionary<string, SettingInfo>` class which contains the `Name` property of a setting. 
as well the generic type and the value of the setting value. The usual use case for this class is serialization of settings because
the `SettingRenderer` class breaks settings down to the defining members of a setting.
 
## `SettingReporter`
This class acts as an helper for using the `SettingManager` and the `SettingRenderer` class. 
The only functionality it provides is firing the `Modified` event whenever the injected manager has been modified. This event captures the 
render of the corresponding `SettingRenderer` as an event parameter of the type `IReadOnlyDictionary<string, SettingInfo>`.

## `SettingResult`
Since both the `SettingRenderer` and the `SettingReporter` are based on the `SettingManager` the `SettingResult` class may act as container class 
for these three management classes. An instance can be created by injecting a setting collection where `SettingManager` class is instantiated 
immediatly based on the provided setting collection. Then the `SettingRenderer` and the `SettingReporter` can be fetched lazily using the corresponding properties.

## `SettingDefiner`
This class acts a counterpart of the `SettingRenderer` class and may be used for the deserialization of settings. 
The `SettingDefiner` class can be instantiated by injecting an `ISettingPreview` collection which should be deserialized into it. This may be done before instantiating the `SettingManager` class that activates the settings since otherwise the deserialized values may not correspond to the actual `Value` properties of the settings. 
After instantiation the class provides the `Definition` property which exposes an `IReadOnlyDictionary<string, Type>` interface that may used to deserialize appropriate values
for the settings. These values can then be used to redefine the setting collection using the `Define` method which takes an `IReadOnlyDictionary<string, object>` interface as an argument.













