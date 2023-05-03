using UnityEngine;

/// <summary>
/// Class for holding data/values needed across scripts and scenes.
/// Values remain even when changing scenes.
/// </summary>
public static class TestDataStatic
{
    /// <summary>
    /// The file path of the selected test
    /// </summary>
    public static string testFilePath { get; set; }

    /// <summary>
    /// The file path of the folder where the tests are saved
    /// </summary>
    public static string testFolderPath { get; } = Application.dataPath + "/TestFiles/";

    /// <summary>
    /// The file path of the folder where the test resutls are saved
    /// </summary>
    public static string testResultFolder { get; } = Application.dataPath + "/TestData/";

    /// <summary>
    /// The file path of the test that is currently open in the scene
    /// </summary>
    public static string currentTestFilePath { get; set; }

    /// <summary>
    /// The time it takes to detect the players vision on an object
    /// </summary>
    public static float visionDetectionTime { get; set; }

    /// <summary>
    /// Whether the test is running
    /// </summary>
    public static bool testIsRunning { get; set; }

    /// <summary>
    /// The players distance to the test area
    /// </summary>
    public static float playerDistance { get; set; }

}
