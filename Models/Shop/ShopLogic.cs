using System;
using System.Collections.Generic;
using System.Linq;
using ZooVillage.Models.Products;

namespace ZooVillage.Models.Shop
{
    /// <summary>
    /// Логика магазина с перегрузкой операторов
    /// </summary>
    public class ShopLogic
    {
        private readonly Dictionary<string, decimal> _buyPrices;
        private readonly Dictionary<string, decimal> _sellPrices;

        public ShopLogic()
        {
            // Цены на покупку животных
            _buyPrices = new Dictionary<string, decimal>
            {
                { "Курица", 100 },
                { "Овца", 150 },
                { "Корова", 300 },
                { "Баран", 180 },
                { "Зерно", 30 }
            };

            // Цены на продажу продуктов
            _sellPrices = new Dictionary<string, decimal>
            {
                { "Яйцо", 20 },
                { "Молоко", 50 },
                { "Шерсть", 40 },
                { "Зерно", 25 } // Продаем дешевле, чем покупаем
            };
        }

        // ==========================================
        // ПЕРЕГРУЗКА ОПЕРАТОРОВ (статические методы)
        // ==========================================

        /// <summary>
        /// Оператор + : объединение цен
        /// </summary>
        public static decimal operator +(ShopLogic shop, string itemName)
        {
            if (shop._buyPrices.ContainsKey(itemName))
                return shop._buyPrices[itemName];
            if (shop._sellPrices.ContainsKey(itemName))
                return shop._sellPrices[itemName];
            return 0;
        }

        /// <summary>
        /// Оператор * : расчет стоимости с учетом количества
        /// </summary>
        public static decimal operator *(ShopLogic shop, ShopItem item)
        {
            return item.Price * item.Quantity;
        }

        // ==========================================
        // МЕТОДЫ МАГАЗИНА
        // ==========================================

        /// <summary>
        /// Получить список товаров для покупки
        /// </summary>
        public List<ShopItem> GetBuyItems()
        {
            return new List<ShopItem>
            {
                new ShopItem { Name = "Курица", Icon = "🐔", Price = _buyPrices["Курица"], Unit = "шт", Quantity = 0 },
                new ShopItem { Name = "Овца", Icon = "🐑", Price = _buyPrices["Овца"], Unit = "шт", Quantity = 0 },
                new ShopItem { Name = "Корова", Icon = "🐮", Price = _buyPrices["Корова"], Unit = "шт", Quantity = 0 },
                new ShopItem { Name = "Баран", Icon = "🐏", Price = _buyPrices["Баран"], Unit = "шт", Quantity = 0 }
            };
        }

        /// <summary>
        /// Получить список товаров для продажи
        /// </summary>
        public List<ShopItem> GetSellItems(Inventory inventory)
        {
            var items = new List<ShopItem>();

            if (inventory != null)
            {
                var egg = inventory.GetProduct(ProductType.Egg);
                var milk = inventory.GetProduct(ProductType.Milk);
                var wool = inventory.GetProduct(ProductType.Wool);

                items.Add(new ShopItem
                {
                    Name = "Яйцо",
                    Icon = "🥚",
                    Price = _sellPrices["Яйцо"],
                    Unit = "шт",
                    Quantity = 0,
                    AvailableInStock = (int)(egg?.Quantity ?? 0)
                });

                items.Add(new ShopItem
                {
                    Name = "Молоко",
                    Icon = "🥛",
                    Price = _sellPrices["Молоко"],
                    Unit = "л",
                    Quantity = 0,
                    AvailableInStock = (int)(milk?.Quantity ?? 0)
                });

                items.Add(new ShopItem
                {
                    Name = "Шерсть",
                    Icon = "🧶",
                    Price = _sellPrices["Шерсть"],
                    Unit = "кг",
                    Quantity = 0,
                    AvailableInStock = (int)(wool?.Quantity ?? 0)
                });
            }

            return items;
        }

        /// <summary>
        /// Рассчитать стоимость покупки (использует перегрузку *)
        /// </summary>
        public decimal CalculateBuyPrice(ShopItem item)
        {
            // Используем перегруженный оператор *
            return this * item;
        }

        /// <summary>
        /// Рассчитать стоимость продажи
        /// </summary>
        public decimal CalculateSellPrice(ShopItem item)
        {
            return this * item;
        }

        /// <summary>
        /// Купить товар (вернуть True если успешно)
        /// </summary>
        public bool BuyItem(ShopItem item, ref double budget)
        {
            decimal totalPrice = CalculateBuyPrice(item);

            if (budget >= (double)totalPrice)
            {
                budget -= (double)totalPrice;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Продать товар
        /// </summary>
        public bool SellItem(ShopItem item, Inventory inventory, ref double budget)
        {
            if (inventory == null)
                return false;

            var product = GetProductFromItem(item, inventory);
            if (product == null || product.Quantity < item.Quantity)
                return false;

            // Используем перегрузку оператора - для удаления из амбара
            var soldProduct = new Product(product.Type, item.Quantity, product.Unit);
            inventory.RemoveProduct(soldProduct);

            decimal totalPrice = CalculateSellPrice(item);
            budget += (double)totalPrice;

            return true;
        }

        private Product GetProductFromItem(ShopItem item, Inventory inventory)
        {
            ProductType? type = item.Name switch
            {
                "Яйцо" => ProductType.Egg,
                "Молоко" => ProductType.Milk,
                "Шерсть" => ProductType.Wool,
                _ => (ProductType?)null
            };

            return type.HasValue ? inventory.GetProduct(type.Value) : null;
        }

        /// <summary>
        /// Получить прайс-лист
        /// </summary>
        public string GetPriceList()
        {
            string list = "🛒 ПРАЙС-ЛИСТ:\n\n";
            list += "📥 ПОКУПКА:\n";
            foreach (var item in _buyPrices)
            {
                list += $"  {item.Key}: {item.Value} руб.\n";
            }

            list += "\n📤 ПРОДАЖА:\n";
            foreach (var item in _sellPrices)
            {
                list += $"  {item.Key}: {item.Value} руб.\n";
            }

            return list;
        }
    }
}