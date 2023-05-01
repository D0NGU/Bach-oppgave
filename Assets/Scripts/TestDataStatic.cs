using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestDataStatic
{
    // The file path of the selected test
    public static string testFilePath { get; set; }

    // The file path of the folder where the tests are saved
    public static string testFolderPath { get; } = Application.dataPath + "/TestFiles/";

    // The file path of the folder where the test resutls are saved
    public static string testResultFolder { get; } = Application.dataPath + "/TestData/";

    // The file path of the test that is currently open in the scene
    public static string currentTestFilePath { get; set; }

    public static float visionDetectionTime { get; set; }

    public static bool testIsRunning { get; set; }

    public static float playerDistance { get; set; }

}
