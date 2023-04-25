using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestDataStatic
{
    // The file path of the selected test
    public static string testFilePath { get; set; }

    // The file path of the test that is currently open in the scene
    public static string currentTestFilePath { get; set; }

    public static float visionDetectionTime { get; set; }

    public static bool testIsRunning { get; set; }

    public static float playerDistance { get; set; }
}
