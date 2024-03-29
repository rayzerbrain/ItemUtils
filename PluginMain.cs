﻿using System;
using System.Collections.Generic;

using HarmonyLib;

using Exiled.API.Features;
using Exiled.Loader;

using ItemUtils.API.Modifiers;
using ItemUtils.API;


namespace ItemUtils
{
    public class PluginMain : Plugin<Config>
    {
        private static PluginMain Singleton;
        public static PluginMain Instance => Singleton;
        public override Version Version => new Version(1, 2, 3);
        public override Version RequiredExiledVersion => new Version(5, 0, 0);
        public override string Author => "rayzer";
        public override string Name => "ItemUtils";

        private List<ItemModifier> loadedModifiers;
        private Harmony harmony;

        public override void OnEnabled()
        {
            Singleton = this;
            harmony = new Harmony(Name);
            harmony.PatchAll();
            LoadModifiers();
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            UnloadModifiers();
            harmony.UnpatchAll();
            harmony = null;
            Singleton = null;
            base.OnDisabled();
        }
        //add all the user defined modifiers with their type
        public void LoadModifiers()
        {
            loadedModifiers = new List<ItemModifier>();
            SubtypeDeserializer<ItemModifier> sd = new SubtypeDeserializer<ItemModifier>();
            List<Type> types = new List<Type>(Assembly.GetTypes());

            foreach (KeyValuePair<string, object> map in Config.ItemModifiers)
            {
                ItemModifier mod = sd.FindValidSubtype(Loader.Serializer.Serialize(map.Value), types);

                mod.RegisterEvents();
                loadedModifiers.Add(mod);
            }
        }
        public void UnloadModifiers()
        {
            foreach (ItemModifier mod in loadedModifiers)
            {
                mod.UnregisterEvents();
            }
            loadedModifiers = null;
        }
        
    }
}
