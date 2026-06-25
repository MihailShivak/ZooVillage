# 📋 АРХИТЕКТУРНЫЙ ОТЧЕТ ПРОЕКТА ZOOVILLAGE

## 1. МНОГОУРОВНЕВАЯ ИЕРАРХИЯ ИНТЕРФЕЙСОВ

### Иерархия интерфейсов (7 уровней)

```
УРОВЕНЬ 1 - Корневой интерфейс
└── IAnimal (базовый интерфейс для всех животных)
    ├── Свойства: Id, Name, AgeInDays, Gender, Stage, HealthInfo, Children, BasePrice
    └── Методы: CalculateCurrentPrice(), AgeOneDay(), TryMature()

УРОВЕНЬ 2 - Группировка по типам
├── IBird : IAnimal (маркер для птиц)
└── IMammal : IAnimal (маркер для млекопитающих)

УРОВЕНЬ 3 - Интерфейсы характеристик
├── IGrowable : IAnimal (способность расти/взрослеть)
│   └── Метод: TryMature()
├── IBreeder : IAnimal (способность размножаться)
│   ├── Свойство: CanBreed
│   └── Свойство: Species
└── IJuvenile : IAnimal, IGrowable (молодое животное)
    ├── Свойство: Parent (ссылка на родителя)
    └── Свойство: DaysSinceBirth (дни жизни)

УРОВЕНЬ 4 - Разделение по полу
├── IFemaleBreeder : IBreeder (самки)
│   └── Метод: GiveBirth(IBreeder father) → IJuvenile
└── IMaleBreeder : IBreeder (самцы)
    └── Свойство: CanImpregnate

УРОВЕНЬ 5 - Интерфейсы производства
├── IEggLayer (производство яиц)
│   └── Свойство: EggsPerWeek
├── IMilkProducer (производство молока)
│   └── Свойство: DailyMilkYield
└── IWoolProducer (производство шерсти)
    └── Свойство: WoolPerYear

УРОВЕНЬ 6 - Специфические интерфейсы животных
├── ICow : IMammal, IMilkProducer, IFemaleBreeder
├── IBull : IMammal, IMaleBreeder
├── IRam : IMammal, IWoolProducer, IMaleBreeder
├── IChicken : IBird, IEggLayer, IFemaleBreeder
├── IRooster : IBird, IMaleBreeder
├── ICalf : IMammal, IJuvenile
└── IChick : IBird, IJuvenile

УРОВЕНЬ 7 - Базовые абстрактные классы
├── AnimalBase : IAnimal, IGrowable (базовая реализация)
│   ├── Bird : AnimalBase, IBird (для птиц)
│   └── Mammal : AnimalBase, IMammal (для млекопитающих)
```

### Анализ множественного наследования интерфейсов

**Cow** (максимальная сложность):
```
Cow : Mammal, ICow
    → реализует 3 роли: млекопитающее + производитель молока + самка
    → может: производить молоко + размножаться (давать потомство)
```

**Chicken**:
```
Chicken : Bird, IChicken
    → реализует 3 роли: птица + производитель яиц + самка
    → может: откладывать яйца + размножаться
```

**Ram**:
```
Ram : Mammal, IRam
    → реализует 3 роли: млекопитающее + производитель шерсти + самец
    → может: производить шерсть + участвовать в размножении
```

---

## 2. АГРЕГАЦИЯ И КОМПОЗИЦИЯ

### Композиция (сильные связи - владение)

#### AnimalBase ⟶ (содержит)
```
┌─────────────────────────────────┐
│ AnimalBase                      │
├─────────────────────────────────┤
│ HealthRecord (1:1)              │ - здоровье животного
│ List<IAnimal> Children (1:*)    │ - потомки животного
│ Guid Id                         │ - уникальный идентификатор
│ string Name, Gender, Stage      │ - основные характеристики
└─────────────────────────────────┘
```

#### FarmViewModel ⟶ (содержит)
```
┌─────────────────────────────────────────┐
│ FarmViewModel : INotifyPropertyChanged  │
├─────────────────────────────────────────┤
│ ObservableCollection<IAnimal> Farm      │ - все животные на ферме
│ ObservableCollection<AnimalVisual>      │ - визуальные модели животных
│ Dictionary<string, int> Inventory       │ - ресурсы (яйца, молоко и т.д.)
│ BreedingService BreedingService (1:1)   │ - система размножения
│ AudioManager AudioManager (1:1)         │ - управление музыкой
│ DispatcherTimer[] Timers                │ - таймеры симуляции
└─────────────────────────────────────────┘
```

#### Inventory ⟶ (содержит)
```
┌──────────────────────────────────────┐
│ Inventory                            │
├──────────────────────────────────────┤
│ Dict<ProductType, Product> Products  │ - товары (яйца, молоко, шерсть)
│  ├── ProductType.Egg → Product      │
│  ├── ProductType.Milk → Product     │
│  └── ProductType.Wool → Product     │
└──────────────────────────────────────┘
```

### Агрегация (слабые связи - использование)

#### AnimalVisual → использует IAnimal
```
AnimalVisual
    ├── private readonly IAnimal _animal  ← не владеет, только ссылка
    ├── DispatcherTimer _moveTimer        ← самостоятельное управление
    └── DispatcherTimer _breedingTimer    ← самостоятельное управление
```

#### BreedingService → использует IEnumerable<IAnimal>
```
BreedingService
    ├── private Dictionary<Guid, int> _lastBirthDay  ← внутреннее состояние
    └── public List<IJuvenile> TryBreed(IEnumerable<IAnimal> farm)
        ↑ параметр, не владеет фермой
```

#### StoreWindow → использует FarmViewModel и Inventory
```
StoreWindow
    ├── DataContext = FarmViewModel  ← отображает данные ViewModel
    ├── StoreItemCollection _buyItems    ← самостоятельная коллекция
    └── StoreItemCollection _sellItems   ← самостоятельная коллекция
```

---

## 3. ПЕРЕГРУЗКА ОПЕРАТОРОВ (18 всего)

### 3.1 ShopItem - Работа с ценами и количеством (10 операторов)

```csharp
// Арифметические операторы (3)
ShopItem operator +(ShopItem item, int quantity)    // добавить кол-во
ShopItem operator -(ShopItem item, int quantity)    // уменьшить кол-во
decimal operator *(ShopItem item, int quantity)     // item * qty = цена

// Умножение в обратном порядке
decimal operator *(int quantity, ShopItem item)     // qty * item = цена

// Операторы сравнения (6)
bool operator ==(ShopItem left, ShopItem right)     // по имени и цене
bool operator !=(ShopItem left, ShopItem right)
bool operator >(ShopItem left, ShopItem right)      // по цене
bool operator <(ShopItem left, ShopItem right)
bool operator >=(ShopItem left, ShopItem right)
bool operator <=(ShopItem left, ShopItem right)

// Неявное преобразование типов
implicit operator decimal(ShopItem item)            // в общую стоимость
```

**Практическое использование:**
```
item + 5           ← добавить 5 штук
item - 2           ← убрать 2 штуки
item * 3           ← стоимость 3 штук = price * 3
3 * item           ← то же самое
item > anotherItem ← сравнить цены
decimal cost = item ← неявно преобразовать в decimal
```

### 3.2 StoreItem - Управление количеством в UI (8 операторов)

```csharp
// Инкремент/декремент (2)
StoreItem operator ++(StoreItem item)   // ++item - увеличить на 1
StoreItem operator --(StoreItem item)   // --item - уменьшить на 1

// Операторы сравнения (6)
bool operator ==(StoreItem? a, StoreItem? b)       // по цене
bool operator !=(StoreItem? a, StoreItem? b)
bool operator <(StoreItem a, StoreItem b)
bool operator >(StoreItem a, StoreItem b)
bool operator <=(StoreItem a, StoreItem b)
bool operator >=(StoreItem a, StoreItem b)
```

**Практическое использование:**
```
item++             ← увеличить количество на 1 (в UI кнопка +)
item--             ← уменьшить количество на 1 (в UI кнопка -)
item1 > item2      ← сравнить цены (для сортировки)
```

### 3.3 ShoppingCart - Работа с корзинами (2 оператора)

```csharp
// Объединение корзин
ShoppingCart operator +(ShoppingCart left, ShoppingCart right)
    // Объединяет товары из двух корзин
    // Суммирует количество одинаковых товаров

// Применение скидки
decimal operator *(ShoppingCart cart, decimal discount)
    // cart * 0.9 = стоимость с 10% скидкой
    // Возвращает decimal итоговую цену
```

**Практическое использование:**
```
cart1 + cart2      ← объединить две корзины
cart * 0.8         ← применить 20% скидку (остаток 80%)
```

### 3.4 ShopLogic - Получение цен (3 оператора)

```csharp
// Получение цены покупки
decimal operator +(ShopLogic shop, string itemName)
    // shop + "Корова" = 300 (цена покупки)

// Получение цены продажи
decimal operator -(ShopLogic shop, string itemName)
    // shop - "Молоко" = 50 (цена продажи)

// Расчет стоимости товара
decimal operator *(ShopLogic shop, ShopItem item)
    // shop * item = item.Price * item.Quantity
```

**Практическое использование:**
```
decimal buyPrice = shop + "Куриц" ← получить цену покупки
decimal sellPrice = shop - "Яйцо"  ← получить цену продажи
decimal total = shop * shopItem    ← рассчитать общую стоимость
```

---

## 4. СВОДНАЯ ТАБЛИЦА ПЕРЕГРУЗКИ ОПЕРАТОРОВ

| Класс | Оператор | Тип | Параметры | Возвращает | Статус |
|-------|----------|-----|-----------|-----------|--------|
| **ShopItem** | + | Арифметика | (item, int) | ShopItem | ✓ |
| | - | Арифметика | (item, int) | ShopItem | ✓ |
| | * | Умножение | (item, int) | decimal | ✓ |
| | * | Умножение | (int, item) | decimal | ✓ |
| | == | Сравнение | (item, item) | bool | ✓ |
| | != | Сравнение | (item, item) | bool | ✓ |
| | > | Сравнение | (item, item) | bool | ✓ |
| | < | Сравнение | (item, item) | bool | ✓ |
| | >= | Сравнение | (item, item) | bool | ✓ |
| | <= | Сравнение | (item, item) | bool | ✓ |
| **StoreItem** | ++ | Инкремент | (item) | StoreItem | ✓ |
| | -- | Декремент | (item) | StoreItem | ✓ |
| | == | Сравнение | (item?, item?) | bool | ✓ |
| | != | Сравнение | (item?, item?) | bool | ✓ |
| | > | Сравнение | (item, item) | bool | ✓ |
| | < | Сравнение | (item, item) | bool | ✓ |
| **ShoppingCart** | + | Объединение | (cart, cart) | ShoppingCart | ✓ |
| | * | Скидка | (cart, decimal) | decimal | ✓ |
| **ShopLogic** | + | Цена | (shop, string) | decimal | ✓ |
| | - | Цена | (shop, string) | decimal | ✓ |
| | * | Расчет | (shop, item) | decimal | ✓ |

---

## 5. ПАТТЕРНЫ ПРОЕКТИРОВАНИЯ

### Observer Pattern (MVC)
- **FarmViewModel : INotifyPropertyChanged** - уведомляет UI об изменениях
- **AnimalVisual : INotifyPropertyChanged** - уведомляет UI о позиции животного

### Strategy Pattern
- **BreedingService** - стратегия размножения животных
- **AudioManager** - стратегия управления аудио

### Factory Pattern
- **AnimalToImageConverter** - создание пути к изображению для животного
- **ShopLogic.GetBuyItems()** - создание коллекции товаров

### Value Converter Pattern
- **AnimalToImageConverter : IValueConverter** - преобразование животного в изображение
- **BoolToVisibilityConverter : IValueConverter** - преобразование bool в видимость

---

## 6. СТАТИСТИКА АРХИТЕКТУРЫ

| Показатель | Значение |
|-----------|----------|
| **Всего интерфейсов** | 18 |
| **Уровней иерархии интерфейсов** | 7 |
| **Классов животных** | 7 (Cow, Bull, Ram, Chicken, Rooster, Calf, Chick) |
| **Перегруженных операторов** | 18 |
| **Service классов** | 2 (BreedingService, AudioManager) |
| **ViewModels** | 1 (FarmViewModel) |
| **Views** | 9 (GameView, MainView, StoreWindow, SettingsWindow, StorageWindow, NotificationWindow и т.д.) |
| **Converters** | 2 (AnimalToImageConverter, BoolToVisibilityConverter) |
| **Максимальная глубина наследования** | 7 уровней (через интерфейсы) |
| **Максимальное множественное наследование** | 3 интерфейса (у Cow, Chicken, Ram) |

---

## 7. АРХИТЕКТУРНЫЕ ВЫВОДЫ

### Сильные стороны ✅
1. **Гибкая иерархия интерфейсов** - можно комбинировать различные характеристики
2. **Множественное наследование интерфейсов** - животные реализуют несколько ролей
3. **Удобная перегрузка операторов** - интуитивный синтаксис для операций
4. **Четкое разделение ответственности** - каждый класс имеет одну зону ответственности
5. **Слабая связанность через интерфейсы** - легко добавлять новые типы животных

### Потенциальные улучшения ⚠️
1. **Глубокая иерархия** - может затруднить добавление нового вида животного (7 уровней)
2. **Неиспользуемые классы** - ShopLogic, ShoppingCart, ShopItem не используются функционально
3. **Сложность тестирования** - множественное наследование может усложнить unit-тесты
4. **Статические методы в иерархии** - перегруженные операторы могут быть непредсказуемы

---

## 8. РЕКОМЕНДАЦИИ

1. ✅ **Оставить текущую иерархию** - она хорошо спроектирована и работает
2. ⚠️ **Рассмотреть удаление неиспользуемых Shop классов** - они не используются функционально
3. ✅ **Документировать интерфейсы** - 7 уровней требуют хорошей документации
4. ✅ **Добавить unit-тесты** - особенно для перегруженных операторов
5. ✅ **Создать диаграмму наследования** - поможет новым разработчикам понять архитектуру

