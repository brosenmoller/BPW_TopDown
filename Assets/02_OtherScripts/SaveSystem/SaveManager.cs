// Adapted from Tutorial by Dapper Dino : https://www.youtube.com/watch?v=f5GvfZfy3yk&t=487s

using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;

public class SaveManager : Singleton<SaveManager>
{
    protected override void SingletonAwake()
    {
        Load();
    }

    private string SavePath => $"{Application.persistentDataPath}/GameData.save";

    [ContextMenu("Save")]
    public void Save()
    {
        var state = LoadFile();
        CaptureState(state);
        SaveFile(state);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        var state = LoadFile();
        RestoreState(state);
    }

    [ContextMenu("Delete")]
    public void DeleteSave()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log($"No save file found at{SavePath}");
        }

        File.Delete(SavePath);
    }

    private Dictionary<string, object> LoadFile()
    {
        if (!File.Exists(SavePath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(SavePath, FileMode.Open))
        {
            BinaryFormatter formatter = GetBinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    private void SaveFile(object state)
    {
        using (FileStream stream = File.Open(SavePath, FileMode.Create))
        {
            BinaryFormatter formatter = GetBinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    private void CaptureState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id] = saveable.CaptureState();
        }
    }

    private void RestoreState(Dictionary<string, object> state)
    {
        foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
        {
            if (state.TryGetValue(saveable.Id, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }

    private BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new();

        SurrogateSelector selector = new();

        Vector3SerializationSurrogate vector3Surrogate = new();
        QuaterionSerializationSurrogate quaterionSurrogate = new();
        TypeSerializationSurrogate typeSurrogate = new();

        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
        selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaterionSurrogate);
        selector.AddSurrogate(typeof(Type), new StreamingContext(StreamingContextStates.All), typeSurrogate);

        formatter.SurrogateSelector = selector;

        return formatter;
    }
}