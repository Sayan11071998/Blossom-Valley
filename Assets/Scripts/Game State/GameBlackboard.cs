using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameBlackboard 
{

    public Dictionary<string, object> entries;
    public GameBlackboard()
    {
        entries = new Dictionary<string, object>();
    }

    public void Debug()
    {
        //For each entry print out its corresponding value
        foreach(var entry in entries)
        {
            string key = entry.Key.ToString(); 
            UnityEngine.Debug.Log($"key: {key} value: {entry.Value}");

        }
    }

    //Get a list type value or initialise one
    public List<T> GetOrInitList<T>(string key)
    {
        List<T> value = new List<T>();
        if (entries.TryGetValue(key, out object v))
        {
            value = (List<T>) v;
            
        }
        SetValue(key, value); 
        return value;

    }

    //Try to get the value according to the type
    public bool TryGetValue<T>(string key, out T value)
    {
        if (entries.TryGetValue(key, out object v))
        {
            value = (T) v; 
            if (value is not null)
            {
                return true;
            } 
        }
        value = default(T);
        
        return false; 
    }

    //Change the value in the blackboard entry, otherwise create a new entry
    public void SetValue<T>(string key, T value)
    {
        //UnityEngine.Debug.Log($"Setting {key} to {value}");
        entries[key] = value;
        //UnityEngine.Debug.Log($"The value is {entries[key]}");
        Debug();
    }

    //Increase value of an entry
    public void IncreaseValue(string key, int valueToIncrease){
        //Get the value
        if(TryGetValue(key, out int previousValue))
        {
            SetValue(key, previousValue+ valueToIncrease);
        } else
        {
            //Create a new entry
            SetValue(key, valueToIncrease);
        }
    }

    public bool ContainsKey(string key) => entries.ContainsKey(key);

    public void RemoveKey(string key) => entries.Remove(key);

}
