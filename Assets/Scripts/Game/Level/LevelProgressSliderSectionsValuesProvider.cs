using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelProgressSliderSectionsValuesProvider
{
    private SliderSectionValues[] _sliderSectionsValues;
    public LevelProgressSliderSectionsValuesProvider(SliderSectionValues[] sliderSectionsValues)
    {
        _sliderSectionsValues = sliderSectionsValues;
    }
    // public SliderSectionValues GetCurrentLevelPartConfig(int level)
    // {
    //     if (level %3 == 0)
    //     {
    //         return _sliderSectionsValues[2];
    //     }
    //     else if(level %2 == 0)
    //     {
    //         return _sliderSectionsValues[1];
    //     }
    //     else if (level == 3)
    //     {
    //         return _sliderSectionsValues[0];
    //     }
    // }
    // public LevelPart GetCurrentLevelPartConfig(int level)
    // {
    //     // Level 1 - Level Index Config 0 , Level Index Part 0               4 - 1, 0
    //     // 2 - 0, 1                                                          5 - 1, 1
    //     // 3 - 0, 2                                                          6 - 1, 2
    //     int indexConfig = 0;
    //     int indexPart = 0;
    //     switch (level)
    //     {
    //         case 1:
    //             indexConfig = 0;
    //             indexPart = 0;
    //             break;
    //         case 2:
    //             indexConfig = 0;
    //             indexPart = 1;
    //             break;
    //         case 3:
    //             indexConfig = 0;
    //             indexPart = 2;
    //             break;
    //         case 4:
    //             indexConfig = 1;
    //             indexPart = 0;
    //             break;
    //         case 5:
    //             indexConfig = 1;
    //             indexPart = 1;
    //             break;
    //         case 6:
    //             indexConfig = 1;
    //             indexPart = 2;
    //             break;
    //     }
    //     return _levelConfigs[indexConfig].LevelParts[indexPart];
    // }
}
[System.Serializable]
public class SliderSectionValues
{
    public float SectionStartValue;
    public float SectionEndValue;
}
