using System;
using System.Collections.Generic;

using Exiled.API.Features;
using Exiled.Loader;


namespace ItemUtils.API
{
    //T is the base class
    public class SubtypeDeserializer<TBase> where TBase : class
    {
        // Finds highest class in the hierarchy(TBase being highest) that retains all properties from the config
        // rawConfig represents a single object, with types containing the possible subtypes
        public TBase FindValidSubtype(string rawConfig, List<Type> types)
        {
            TBase baseObj = null;

            types.RemoveAll((t) => !t.IsSubclassOf(typeof(TBase)));
            types.Add(typeof(TBase));

            Log.Debug($"Checking deserialization for config: \n{rawConfig}", PluginMain.Instance.Config.DebugMode);

            foreach (Type t in types)
            {
                
                if (TryDeserialize(rawConfig, t, out TBase newObj))
                {
                    if (baseObj == null || baseObj.GetType().IsSubclassOf(t))
                    {
                        baseObj = newObj;
                        Log.Debug($"\tObject type becoming new type {t}", PluginMain.Instance.Config.DebugMode);
                    }
                }
            }
            
            Log.Debug($"Highest valid type was {baseObj.GetType()}", PluginMain.Instance.Config.DebugMode);

            Log.Assert(baseObj != null, $"Your config is not set up properly! Config:\n{rawConfig}");

            return baseObj;
        }

        private bool TryDeserialize(string rawConfig, Type t, out TBase obj)
        {
            obj = (TBase)Loader.Deserializer.Deserialize(rawConfig, t);
            string allProps = "\n" + Loader.Serializer.Serialize(obj);

            // If any property in the serialized config is not found in the full list of properties, deserialization marked as unsuccessfull
            foreach (string line in rawConfig.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string prop = "\n" + line.Substring(0, line.IndexOf(':') + 1);
                if (!allProps.Contains(prop))
                {
                    Log.Debug($"Object was not {t} because of{prop}", PluginMain.Instance.Config.DebugMode);
                    obj = null;
                    return false;
                }
            }
            return true;
        }
    }
}