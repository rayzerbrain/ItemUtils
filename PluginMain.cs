using System;
using System.Collections.Generic;
using System.Text;

using HarmonyLib;

using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Loader;

using ItemUtils.API.Modifiers;

namespace ItemUtils
{
    public class PluginMain : Plugin<Config>
    {
        private static PluginMain Singleton;
        public static PluginMain Instance => Singleton;
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(5, 0, 0);
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
                    //if t.Equals(typeof(ItemModifier))
                    if (t.IsSubclassOf(typeof(ItemModifier)) || t.Equals(typeof(ItemModifier)))
                    {
                        rawMod = Loader.Serializer.Serialize(match);
                        //mod = (ItemModifier)Loader.Deserializer.Deserialize(rawMod, t);
                        //if (Loader.Serializer.Serialize(mod).Equals(rawMod))
                        Log.Debug($"Testing config \n{rawMod}\n against type {t}", Config.DebugMode);
                        if (TryDeserialize(rawMod, t, out mod))
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
                    Log.Error($"Your config is not set up properly! THe following config will not be loaded:\n{map.Value}");
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
        // This method checks the deserialized modifier to see if any properties are missing from the original configuration
        public bool TryDeserialize(string rawConfig, Type t, out ItemModifier mod)
        {
            mod  = (ItemModifier)Loader.Deserializer.Deserialize(rawConfig, t);
            string allProps = Loader.Serializer.Serialize(mod);
            List<string> configProps = GetRawProperties(rawConfig);
            
            // If any property in the serialized config is not found in the full list of properties, deserialization marked as unsuccessfull
            foreach (string prop in configProps)
            {
                //Log.Debug($"Testing \n{prop}\nAgainst\n{allProps}");
                if (!allProps.Contains(prop))
                {
                    Log.Debug($"{allProps}\n was not type {t} because of property \n{prop}.");
                    mod = null;
                    return false;
                }
            }
            return true;
        }
        public List<string> GetRawProperties(string rawConfig)
        {
            string[] lines = rawConfig.Split('\n');
            StringBuilder prop = new StringBuilder();
            List<string> props = new List<string>();

            prop.AppendLine(lines[0]);
            for (int i=1; i<lines.Length-1; i++)
            {
                char c = lines[i][0];
                if (c != ' ' && c != '-')
                {
                    prop.Remove(prop.Length - 1, 1);
                    props.Add(prop.ToString().Trim());
                    prop.Clear();
                }
                prop.AppendLine(lines[i]);
            }
            props.Add(prop.ToString());
            return props;
        }
    }
}
