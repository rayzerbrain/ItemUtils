using System;
using System.Collections.Generic;

using HarmonyLib;

using Exiled.API.Features;
using Exiled.Loader;

using ItemUtils.API.Modifiers;
using System.Threading;

namespace ItemUtils
{
    public class PluginMain : Plugin<Config>
    {
        private static PluginMain Singleton;
        public static PluginMain Instance => Singleton;
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(4, 0, 0);
        public override string Author => "rayzer";
        public override string Name => "ItemUtils";
        private List<ItemModifier> loadedModifiers;
        private Harmony hrmny;
        public override void OnEnabled()
        {
            Singleton = this;
            hrmny = new Harmony(Name);
            hrmny.PatchAll();
            //EventHandler = new EventHandlers(this);
            LoadModifiers();
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            UnloadModifiers();
            hrmny.UnpatchAll();
            hrmny = null;
            //EventHandler = null;
            Singleton = null;
            base.OnDisabled();
        }
        //add all the user defined modifiers with their type
        public void LoadModifiers()
        {
            loadedModifiers = new List<ItemModifier>();
            ItemModifier mod = null;
            string rawMod;

            foreach (KeyValuePair<ItemType, string> map in Config.ModifiedItems)
            {
                if (!Config.ItemModifiers.TryGetValue(map.Value, out object match))
                {
                    Log.Warn($"Missing modifier definition of name {map.Value} for item {map.Key}, skipping...");
                    continue;
                }

                foreach (Type t in Assembly.GetTypes())
                {
                    if (t.IsSubclassOf(typeof(ItemModifier)) || t.Equals(typeof(ItemModifier)))
                    {
                        rawMod = Loader.Serializer.Serialize(match);
                        mod = (ItemModifier)Loader.Deserializer.Deserialize(rawMod, t);
                        if (Loader.Serializer.Serialize(mod).Contains(rawMod))
                        {
                            Log.Debug($"Loading modifier of type {t} for item {map.Key}", Config.DebugMode);
                            mod.Type = map.Key;
                            mod.RegisterEvents();
                            break;
                        }
                    }
                }
                if (mod == null)
                {
                    Log.Error($"Your config is not set up properly! Config:\n{map.Value}");
                }
            }
        }
        public void UnloadModifiers()
        {
            foreach (ItemModifier modifier in loadedModifiers)
            {
                modifier.UnregisterEvents();
            }
            loadedModifiers = null;
        }
    }
}
