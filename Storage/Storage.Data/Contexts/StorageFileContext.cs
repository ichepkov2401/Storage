using Storage.Data.Entity;

namespace Storage.Data.Contexts
{
    internal class StorageFileContext : FileContext
    {
        [TableName("pallets")]
        public List<Pallet> Pallets { get; private set; }

        [TableName("boxes")]
        [FK(typeof(Pallet),
            nameof(Box.PalletId),
            nameof(Pallet.Id),
            nameof(Box.Pallet),
            nameof(Pallet.Boxes))]
        public List<Box> Boxes { get; private set; }

        internal StorageFileContext(string connection) : base("data.json") { }

    }
}
