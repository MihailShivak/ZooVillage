using System;
using System.Collections.Generic;
using System.Linq;

namespace ZooVillage.Models.Shop
{
    /// <summary>
    /// Корзина покупок/продаж (с перегрузкой операторов)
    /// </summary>
    public class ShoppingCart
    {
        private readonly List<ShopItem> _items;

        public ShoppingCart()
        {
            _items = new List<ShopItem>();
        }

        /// <summary>
        /// Добавить товар в корзину
        /// </summary>
        public void AddItem(ShopItem item)
        {
            if (item.Quantity > 0)
            {
                _items.Add(item);
            }
        }

        /// <summary>
        /// Удалить товар из корзины
        /// </summary>
        public void RemoveItem(ShopItem item)
        {
            _items.RemoveAll(i => i.Name == item.Name);
        }

        /// <summary>
        /// Очистить корзину
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// Получить все товары
        /// </summary>
        public List<ShopItem> GetItems()
        {
            return new List<ShopItem>(_items);
        }

        // ==========================================
        // ПЕРЕГРУЗКА ОПЕРАТОРОВ
        // ==========================================

        /// <summary>
        /// Оператор сложения: объединение двух корзин
        /// ShoppingCart + ShoppingCart = ShoppingCart
        /// </summary>
        public static ShoppingCart operator +(ShoppingCart left, ShoppingCart right)
        {
            var result = new ShoppingCart();

            // Добавляем все товары из левой корзины
            foreach (var item in left._items)
            {
                result.AddItem(item);
            }

            // Добавляем все товары из правой корзины
            foreach (var item in right._items)
            {
                result.AddItem(item);
            }

            return result;
        }

        /// <summary>
        /// Оператор умножения: корзина * скидка
        /// </summary>
        public static decimal operator *(ShoppingCart cart, decimal discount)
        {
            decimal total = cart.GetTotalPrice();
            return total * discount;
        }

        // ==========================================
        // МЕТОДЫ
        // ==========================================

        /// <summary>
        /// Получить общую стоимость корзины
        /// </summary>
        public decimal GetTotalPrice()
        {
            // Используем перегрузку оператора *
            return _items.Sum(item => item * item.Quantity);
        }

        /// <summary>
        /// Получить количество товаров
        /// </summary>
        public int GetItemCount()
        {
            return _items.Sum(item => item.Quantity);
        }

        /// <summary>
        /// Применить скидку (использует оператор *)
        /// </summary>
        public decimal ApplyDiscount(decimal discountPercent)
        {
            // discountPercent от 0 до 1 (0.1 = 10% скидка)
            return this * (1 - discountPercent);
        }

        public override string ToString()
        {
            return $"Корзина: {GetItemCount()} товаров на сумму {GetTotalPrice()} руб.";
        }
    }
}