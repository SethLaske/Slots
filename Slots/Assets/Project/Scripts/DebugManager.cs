using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance { get; private set; }

    private string inputBuffer = "";

    // Command table
    private Dictionary<string, Action> commands;

    
    void Awake()
    {
        if (instance == null)
        {

            instance = this;
        }
        else if (instance != this)
        {

            Destroy(gameObject);
        }
        
        RegisterCommands();
    }

    private void RegisterCommands()
    {
        commands = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase)
        {
            { "FW", () => ProgressiveManager.instance.OnWildShown() },
            { "G20", () => SlotCurrencyController.instance.AdjustBank(20) },
            { "S20", () => SlotCurrencyController.instance.SetBank(20) },
            { "Reset", () =>
            {
                PlayerPrefs.DeleteAll();
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
                return;
#else
                Application.Quit();
#endif
            } },
        };
    }

    // Called by your GameManager's Update loop
    public void DoUpdate(float delta)
    {
        ReadKeyboardInput();
    }

    private void ReadKeyboardInput()
    {
        foreach (char c in Input.inputString)
        {
            // ENTER / RETURN
            if (c == '\n' || c == '\r')
            {
                ProcessCommand(inputBuffer);
                inputBuffer = "";
            }
            // BACKSPACE
            else if (c == '\b')
            {
                if (inputBuffer.Length > 0)
                    inputBuffer = inputBuffer.Substring(0, inputBuffer.Length - 1);
            }
            // NORMAL CHARACTER
            else
            {
                inputBuffer += char.ToLowerInvariant(c);
            }
        }
    }

    private void ProcessCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
            return;

        if (commands.TryGetValue(command, out Action action))
        {
            Debug.Log($"[DEBUG] Command executed: {command}");
            action.Invoke();
        }
        else
        {
            Debug.Log($"[DEBUG] Unknown command: {command}");
        }
    }
}
