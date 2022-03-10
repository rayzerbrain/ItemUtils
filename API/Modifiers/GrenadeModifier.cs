using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;

using PlayerHandler = Exiled.Events.Handlers.Player;


namespace ItemUtils.API.Modifiers
{
    public class GrenadeModifier : WeaponModifier
    {
        public float EffectDurationMulti { get; set; } = 1;
        public float FuseTimeMulti { get; set; } = 1;

        public override void RegisterEvents()
        {
            PlayerHandler.ThrowingItem += OnThrowingItem;
            base.RegisterEvents();
        }
        public override void UnregisterEvents()
        {
            PlayerHandler.ThrowingItem -= OnThrowingItem;
            base.UnregisterEvents();
        }
        public void OnThrowingItem(ThrowingItemEventArgs ev)
        {
            Log.Debug($"Player {ev.Player} Throwing Item: {ev.Item}", PluginMain.Instance.Config.DebugMode);

            if (!CanModify(ev.Item, ev.Player))
                return;

            if (ev.Item is ExplosiveGrenade gren)
            {
                gren.BurnDuration *= EffectDurationMulti;
                gren.ConcussDuration *= EffectDurationMulti;
                gren.DeafenDuration *= EffectDurationMulti;
                gren.FuseTime *= FuseTimeMulti;
            }
            else if (ev.Item is FlashGrenade flash)
            {
                flash.FuseTime *= FuseTimeMulti;
            }
        }
        
    }
}
