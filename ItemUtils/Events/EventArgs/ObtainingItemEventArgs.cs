using InventorySystem.Items;

using Exiled.API.Features;
using Exiled.API.Features.Items;


namespace ItemUtils.Events.EventArgs
{
    public class ObtainingItemEventArgs : System.EventArgs
    {
        public Player Player { get; }
        public Item Item { get; }
        
        public ObtainingItemEventArgs(ReferenceHub hub, ItemBase item)
        {
            Player = Player.Get(hub);
            Item = Item.Get(item);
        }
    }
}
