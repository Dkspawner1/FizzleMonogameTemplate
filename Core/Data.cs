using System;

namespace FizzleGame.Core;

public static class Data
{
    public record WindowSettings
    {
        public string Title { get; set; } = "Fizzle's Game!";
        public int Width { get; set; } = 1600;
        public int Height { get; set; } = 900;
        public bool Exit { get; set; } = false;
    }


    public record GameSettings { };
    public static WindowSettings window = new();


}
[Flags]
public enum SCENES : byte
{
    NONE = 0,
    MENU = 1 << 0,      // 0000 0001
    GAME = 1 << 1,      // 0000 0010
    SETTINGS = 1 << 2,  // 0000 0100
    TRANSITION = 1 << 3 // 0000 1000

    // Usage:
    // SCENES activeScenes = SCENES.MENU | SCENES.TRANSITION;
    // Checking if a flag is set
    // Menu is active
    // if (activeScenes.HasFlag(SCENES.MENU))
    // Adding a flag
    // activeScenes |= SCENES.GAME;
    // Removing a flag
    // activeScenes &= ~SCENES.MENU;
}
