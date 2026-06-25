# ZooVillage

Учебный проект на C# / WPF, демонстрирующий ключевые принципы объектно-ориентированного программирования на примере симулятора фермы.

**Авторы:** Шивак Михаил, Мишин Олег  
**Платформа:** .NET 9.0, WPF (Windows Presentation Foundation)  
**Архитектура:** MVVM

---

## Цель проекта

Реализовать приложение, демонстрирующее три принципа ООП:

1. **Агрегация и композиция** — управление жизненным циклом объектов
2. **Многоуровневые интерфейсы** — 7-уровневая иерархия интерфейсов животных
3. **Перегрузка операторов** — интуитивный синтаксис для работы с товарами и корзиной

---

## Описание приложения

ZooVillage — симулятор фермерского хозяйства. Игрок управляет животными: покупает их в магазине, наблюдает за ростом, сбором ресурсов (молоко, яйца, шерсть) и размножением. Животные взрослеют со временем и дают потомство.

---

## Принципы ООП в проекте

### 1. Многоуровневые интерфейсы (7 уровней)

Иерархия интерфейсов строится от абстрактного к конкретному:

```
Уровень 1 — IAnimal              (корневой интерфейс)
Уровень 2 — IBird, IMammal       (группировка по типу)
Уровень 3 — IGrowable, IBreeder, IJuvenile   (характеристики)
Уровень 4 — IFemaleBreeder, IMaleBreeder     (разделение по полу)
Уровень 5 — IEggLayer, IMilkProducer, IWoolProducer  (производство)
Уровень 6 — ICow, IBull, IRam, IChicken, IRooster, ICalf, IChick
Уровень 7 — AnimalBase → Bird / Mammal       (реализация)
```

Каждое животное реализует набор интерфейсов, описывающих его роли:

| Животное | Роли |
|----------|------|
| `Cow` | `IMammal` + `IMilkProducer` + `IFemaleBreeder` |
| `Bull` | `IMammal` + `IMaleBreeder` |
| `Chicken` | `IBird` + `IEggLayer` + `IFemaleBreeder` |
| `Rooster` | `IBird` + `IMaleBreeder` |
| `Ram` | `IMammal` + `IWoolProducer` + `IMaleBreeder` |
| `Calf` | `IMammal` + `IJuvenile` → взрослеет в `Cow` за 365 дней |
| `Chick` | `IBird` + `IJuvenile` → взрослеет в `Chicken`/`Rooster` за 60 дней |

Всего интерфейсов: **18**

---

### 2. Агрегация и композиция

#### Композиция (объект владеет дочерними объектами)

```
AnimalBase
  ├── HealthRecord        (1:1) — здоровье животного
  └── List<IAnimal>       (1:*) — список потомков

FarmViewModel
  ├── ObservableCollection<IAnimal>   — все животные на ферме
  ├── Inventory                       — ресурсы (яйца, молоко, шерсть)
  ├── BreedingService                 — логика размножения
  ├── AudioManager                    — фоновая музыка
  └── DispatcherTimer[]               — таймеры симуляции

Inventory
  └── Dictionary<ProductType, Product>
        ├── ProductType.Egg
        ├── ProductType.Milk
        └── ProductType.Wool
```

#### Агрегация (объект использует, но не владеет)

```
AnimalVisual    → IAnimal         (только ссылка, не владеет)
BreedingService → IEnumerable<IAnimal>  (параметр метода)
StoreWindow     → FarmViewModel   (отображает данные, не владеет)
```

---

### 3. Перегрузка операторов (18 операторов, 4 класса)

#### `ShopItem` — работа с товарами (10 операторов)

```csharp
item + 5           // добавить 5 штук
item - 2           // убрать 2 штуки
item * 3           // стоимость 3 штук
3 * item           // то же самое
item > anotherItem // сравнить по цене
decimal d = item   // неявное преобразование в общую стоимость
// также: ==, !=, <, >=, <=
```

#### `StoreItem` — управление количеством в UI (8 операторов)

```csharp
item++   // кнопка «+» в интерфейсе
item--   // кнопка «-» в интерфейсе
// также: ==, !=, >, <, >=, <=
```

#### `ShoppingCart` — работа с корзиной (2 оператора)

```csharp
cart1 + cart2   // объединить две корзины (суммирует одинаковые товары)
cart * 0.8      // применить скидку 20% → возвращает decimal
```

#### `ShopLogic` — получение цен (3 оператора)

```csharp
decimal buy  = shop + "Корова"  // цена покупки
decimal sell = shop - "Яйцо"    // цена продажи
decimal sum  = shop * shopItem  // общая стоимость позиции
```

---

## Архитектура проекта

```
ZooVillage/
├── Models/
│   ├── Interfaces/       # 18 интерфейсов (7 уровней иерархии)
│   ├── Animals/          # Cow, Bull, Ram, Chicken, Rooster, Calf, Chick
│   └── Shop/             # ShopItem, ShopLogic, ShoppingCart, Inventory
├── ViewModels/
│   └── FarmViewModel.cs  # MVVM-центр: ферма, инвентарь, таймеры
├── Views/                # GameView, StoreWindow, StorageWindow, SettingsWindow…
├── Services/
│   ├── BreedingService.cs
│   └── AudioManager.cs
└── Assets/               # Изображения животных, фоновая музыка
```

### Паттерны проектирования

| Паттерн | Где применён |
|---------|-------------|
| Observer (MVVM) | `FarmViewModel`, `AnimalVisual` → `INotifyPropertyChanged` |
| Strategy | `BreedingService`, `AudioManager` |
| Factory | `ShopLogic.GetBuyItems()`, `AnimalToImageConverter` |
| Value Converter | `AnimalToImageConverter`, `BoolToVisibilityConverter` |

---

## Статистика

| Показатель | Значение |
|-----------|----------|
| Интерфейсов | 18 |
| Уровней иерархии | 7 |
| Классов животных | 7 |
| Перегруженных операторов | 18 |
| Views | 9+ |
| Services | 2 |
| Ошибок компиляции | 0 |
| Платформа | .NET 9.0-windows |
