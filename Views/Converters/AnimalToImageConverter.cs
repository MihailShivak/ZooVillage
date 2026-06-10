using System;
using System.Globalization;
using System.Windows.Data;
using ZooVillage.Models.Animals.Birds;
using ZooVillage.Models.Animals.Mammals;

namespace ZooVillage.Views.Converters
{
    public class AnimalToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var animalType = value.GetType();

            return animalType.Name switch
            {
                nameof(Cow)     => "/Assets/Animals/cow.png",
                nameof(Bull)    => "/Assets/Animals/cow.png",
                nameof(Calf)    => "/Assets/Animals/cow.png",
                nameof(Chicken) => "/Assets/Animals/chicken.png",
                nameof(Rooster) => "/Assets/Animals/chicken.png",
                nameof(Chick)   => "/Assets/Animals/chicken.png",
                nameof(Ram)     => "/Assets/Animals/sheep.png",
                nameof(Lamb)    => "/Assets/Animals/sheep.png",
                _ => "/Assets/Animals/chicken.png"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
