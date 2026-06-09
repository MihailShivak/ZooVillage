using System.Collections;
using ZooVillage.Models.Products;

namespace ZooVillage.Models.Collections
{
    public class StoreItemCollection : IEnumerable<StoreItem>
    {
        private readonly List<StoreItem> _items = new();

        public int Count => _items.Count;

        public StoreItem this[int index] => _items[index];

        public void Add(StoreItem item) => _items.Add(item);

        public bool Remove(StoreItem item) => _items.Remove(item);

        public StoreItem? FindByName(string name) =>
            _items.FirstOrDefault(i => i.ItemName == name);

        public IEnumerable<StoreItem> GetAffordable(int budget) =>
            _items.Where(i => i.Price <= budget);

        public StoreItem? MostExpensive() =>
            _items.Count == 0 ? null : _items.Aggregate((a, b) => a > b ? a : b);

        public StoreItem? Cheapest() =>
            _items.Count == 0 ? null : _items.Aggregate((a, b) => a < b ? a : b);

        public IEnumerator<StoreItem> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
