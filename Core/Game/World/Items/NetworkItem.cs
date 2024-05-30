
namespace Core.Game.World.Items
{
    public struct ItemChangedDetails
    {
        public int ChangeId { get; }
        public NetworkItem Item { get; }

        public ItemChangedDetails(int changeId, NetworkItem item)
        {
            ChangeId = changeId;
            Item = item;
        }
    }

    public class NetworkItem
    {
        public int Type1 { get; set; }
        public int ObjectId { get; set; }
        public int ItemId { get; set; }
        public int Type2 { get; set; }
        public int Count { get; set; }
        public int CustomType1 { get; set; }
        public bool IsEquipped { get; set; }
        public int Bodypart { get; set; }
        public int EnchantLevel { get; set; }
        public int CustomType2 { get; set; }
        public int AugmentationId { get; set; }
        public int Mana { get; set; }

    }
}
