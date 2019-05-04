using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class Utils
{
    [Serializable]
    public class RangeFloat
    {
        public float start;
        public float end;

        public RangeFloat(float start, float end)
        {
            this.start = start;
            this.end = end;
        }

        public float Clamp(float value)
        {
            return Mathf.Clamp(value, start, end);
        }

        public float InverseLerp(float value)
        {
            return Mathf.InverseLerp(start, end, value);
        }
    }

    public static T[] Shuffle<T>(T[] array)
    {
        for(int i = 0; i < array.Length; i++)
        {
            T tmp = array[i];
            int index = UnityEngine.Random.Range(0, array.Length);
            array[i] = array[index];
            array[index] = tmp;
        }

        return array;
    }

    public static T RandomElement<T>(IList<T> collection)
    {
        if (collection.Count == 0)
        {
            throw new InvalidOperationException("Picking random from empty list");
        }
        else
        {
            return collection[UnityEngine.Random.Range(0, collection.Count)];
        }
    }

    public static T[] RandomElements<T>(IList<T> collection, int numElements)
    {
        if(collection.Count == 0)
        {
            throw new InvalidOperationException("Picking random from empty list");
        }
        else if(numElements < 0)
        {
            throw new ArgumentException("numElements should be >= 0");
        }
        else
        {
            T[] shuffledElements = Shuffle(collection.ToArray());
            return shuffledElements.Take(numElements).ToArray();
        }
    }

    public static void PlayRandomSound(AudioSource source, IList<AudioClip> clips)
    {
        AudioClip clip = RandomElement(clips);
        source.PlayOneShot(clip);
    }
}
