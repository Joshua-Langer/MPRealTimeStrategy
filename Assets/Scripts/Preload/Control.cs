using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Control
{
    public static SFXManager sfxManager;
    public static MusicManager musicManager;
    //GM/NM goes here

    static Control()
    {
        GameObject g = SafeFind("_app");
        sfxManager = (SFXManager)SafeComponent(g, "SFXManager");
        musicManager = (MusicManager)SafeComponent(g, "MusicManager");
    }

    private static GameObject SafeFind(string s)
    {
        GameObject g = GameObject.Find(s);
        if (g == null) Woe("GameObject " + s + " not on _preload");
        return g;
    }

    private static Component SafeComponent(GameObject g, string s)
    {
        Component c = g.GetComponent(s);
        if (c == null) Woe("Component" + s + " not on _preload");
        return c;
    }

    private static void Woe(string error)
    {
        Debug.Log("Can't Proceed... " + error);
        Debug.Log("It's very likely you just forgot to launch");
        Debug.Log("from scene zero, the _preload scene");
    }
}
