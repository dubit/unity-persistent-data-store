# unity-persistent-data-store

## What is it?
A library wrapping Unity's persistentDataPath access, to store arbitrary serializable objects by their type, or unique IDs.

Features:
* Persistent storage of serializable objects.

## How to use it.
Call the PersistentDataStore.SerializedData API.

### Saving and loading singleton objects (only one per application)

```c#
if (SerializedData.Exists<MainConfig>())
{
    config = SerializedData.Load<MainConfig>();
}
```

...

```c#
SerializedData.Save(mainConfig);
```


### Saving and loading uniquely-identified objects (as many as necessary)

```c#
if (SerializedData.Exists<SaveFile>(thisPlayerId))
{
    saveFile = SerializedData.Load<SaveFile>(thisPlayerId);
}
else saveFile = new SaveFile();

...

SerializedData.Delete<SaveFile>(thisPlayerId);

...

SerializedData.Save<SaveFile>(thisPlayerId);
```