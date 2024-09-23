using System.Collections.Generic;

public class ArraySeparator<T>
{
    public List<List<T>> Separate(List<T> objectsToSeparate, int partsCount = 3)
    {
        List<List<T>> separatedParts = new List<List<T>>();
        if (partsCount < 2)
        {
            separatedParts.Add(objectsToSeparate);
            return separatedParts;
        }
        
        if (objectsToSeparate.Count < 2)
        {
            separatedParts.Add(objectsToSeparate);
            return separatedParts;
        }

        if (objectsToSeparate.Count < partsCount)
        {
            separatedParts.Add(objectsToSeparate);
            return separatedParts;
        }
        
        int chunkSize = objectsToSeparate.Count / partsCount;
        int finalChunk = 0;

        if (objectsToSeparate.Count % partsCount == 0)
        {
            finalChunk = chunkSize ;
        }
        else
        {
            finalChunk = objectsToSeparate.Count - (chunkSize * (partsCount - 1));
        }
        int startIndexRangeValue = 0;
        int endIndexRangeValue = 0;
        
        for (int i = 0; i < partsCount; i++)
        {
            if (i == 0)
            {
                startIndexRangeValue = 0;
                endIndexRangeValue += chunkSize;
            }
            else if (i == partsCount - 1)
            {
                startIndexRangeValue = endIndexRangeValue;
                endIndexRangeValue += finalChunk;
            }
            else
            {
                startIndexRangeValue = endIndexRangeValue;

                endIndexRangeValue += chunkSize;
            }
            separatedParts.Add(CreatePart(objectsToSeparate, startIndexRangeValue, endIndexRangeValue));
        }
        
        return separatedParts;
    }
    private List<T> CreatePart(List<T> objectsToSeparate, int startIndexRangeValue, int endIndexRangeValue)
    {
        List<T> part = new List<T>(endIndexRangeValue);
        for (int i = startIndexRangeValue; i < endIndexRangeValue; i++)
        {
            part.Add(objectsToSeparate[startIndexRangeValue]);
        }
        return part;
    }
}