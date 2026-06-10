using System.Collections.Generic;

namespace ZooVillage.Models.Products
{
    /// <summary>
    /// Инвентарь для хранения продуктов животных (яйца, молоко, шерсть)
    /// </summary>
    public class Inventory
    {
        private readonly Dictionary<ProductType, Product> _products;

        public Inventory()
        {
            _products = new Dictionary<ProductType, Product>
            {
                { ProductType.Egg, new Product { Type = ProductType.Egg, Quantity = 0, Unit = "шт" } },
                { ProductType.Milk, new Product { Type = ProductType.Milk, Quantity = 0, Unit = "л" } },
                { ProductType.Wool, new Product { Type = ProductType.Wool, Quantity = 0, Unit = "кг" } }
            };
        }

        /// <summary>
        /// Получить продукт из инвентаря
        /// </summary>
        public Product GetProduct(ProductType type)
        {
            return _products.ContainsKey(type) ? _products[type] : null;
        }

        /// <summary>
        /// Добавить продукт в инвентарь
        /// </summary>
        public void AddProduct(Product product)
        {
            if (product == null)
                return;

            if (_products.ContainsKey(product.Type))
            {
                _products[product.Type].Quantity += product.Quantity;
            }
            else
            {
                _products[product.Type] = product;
            }
        }

        /// <summary>
        /// Удалить продукт из инвентаря
        /// </summary>
        public bool RemoveProduct(Product product)
        {
            if (product == null || !_products.ContainsKey(product.Type))
                return false;

            var existing = _products[product.Type];
            if (existing.Quantity >= product.Quantity)
            {
                existing.Quantity -= product.Quantity;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Получить общее количество продуктов
        /// </summary>
        public int GetTotalQuantity()
        {
            int total = 0;
            foreach (var product in _products.Values)
            {
                total += product.Quantity;
            }
            return total;
        }

        /// <summary>
        /// Очистить инвентарь
        /// </summary>
        public void Clear()
        {
            foreach (var product in _products.Values)
            {
                product.Quantity = 0;
            }
        }
    }
}
