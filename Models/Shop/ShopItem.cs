usinusing System;

namespace ZooVillage.Models.Shop
{
    /// <summary>
    /// Товар в магазине (с перегрузкой операторов)
    /// </summary>
    public class ShopItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public int AvailableInStock { get; set; } // Для вкладки "Продать"

        // ==========================================
        // ПЕРЕГРУЗКА ОПЕРАТОРОВ
        // ==========================================

        /// <summary>
        /// Оператор сложения: добавление количества товара
        /// </summary>
        public static ShopItem operator +(ShopItem item, int quantity)
        {
            var newItem = new ShopItem
            {
                Name = item.Name,
                Icon = item.Icon,
                Price = item.Price,
                Unit = item.Unit,
                Quantity = item.Quantity + quantity,
                AvailableInStock = item.AvailableInStock
            };
            return newItem;
        }

        /// <summary>
        /// Оператор вычитания: уменьшение количества товара
        /// </summary>
        public static ShopItem operator -(ShopItem item, int quantity)
        {
            var newItem = new ShopItem
            {
                Name = item.Name,
                Icon = item.Icon,
                Price = item.Price,
                Unit = item.Unit,
                Quantity = Math.Max(0, item.Quantity - quantity),
                AvailableInStock = item.AvailableInStock
            };
            return newItem;
        }

        /// <summary>
        /// Оператор умножения: расчет общей стоимости
        /// ShopItem * int = общая цена
        /// </summary>
        public static decimal operator *(ShopItem item, int quantity)
        {
            return item.Price * quantity;
        }

        /// <summary>
        /// Оператор умножения (обратный порядок)
        /// </summary>
        public static decimal operator *(int quantity, ShopItem item)
        {
            return item * quantity;
        }

        /// <summary>
        /// Оператор сравнения == (по имени и цене)
        /// </summary>
        public static bool operator ==(ShopItem left, ShopItem right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Name == right.Name && left.Price == right.Price;
        }

        /// <summary>
        /// Оператор сравнения !=
        /// </summary>
        public static bool operator !=(ShopItem left, ShopItem right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Оператор больше > (сравнение цен)
        /// </summary>
        public static bool operator >(ShopItem left, ShopItem right)
        {
            return left.Price > right.Price;
        }

        /// <summary>
        /// Оператор меньше < (сравнение цен)
        /// </summary>
        public static bool operator <(ShopItem left, ShopItem right)
        {
            return left.Price < right.Price;
        }

        /// <summary>
        /// Неявное преобразование в decimal (для расчета цены)
        /// </summary>
        public static implicit operator decimal(ShopItem item)
        {
            return item.Price * item.Quantity;
        }

        // ==========================================
        // МЕТОДЫ
        // ==========================================

        public decimal GetTotalPrice()
        {
            // Используем перегруженный оператор *
            return this * Quantity;
        }

        public override bool Equals(object obj)
        {
            if (obj is ShopItem other)
                return this == other;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Price);
        }

        public override string ToString()
        {
            return $"{Icon} {Name} - {Price} руб/{Unit} (в наличии: {AvailableInStock})";
        }
    }
}