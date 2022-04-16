using Exiled.Events.Extensions;

using ItemUtils.Events.EventArgs;

namespace ItemUtils.Events
{
    public static class CustomHandler
    {
        public static event Exiled.Events.Events.CustomEventHandler<ObtainingItemEventArgs> ObtainingItem;
        public static void OnObtainingItem(ObtainingItemEventArgs ev) => ObtainingItem.InvokeSafely(ev);
    }
}