using Prism.Mvvm;
using System.ComponentModel;

namespace Dotnet9WPFControls.Demo.ViewModels
{
    internal class EnumRadioGroupWindowViewModel : BindableBase
    {
        private Gender _selectedGender = Gender.Male;

        public Gender SelectedGender
        {
            get => _selectedGender;
            set
            {
                _selectedGender = value;
                SetProperty(ref _selectedGender, value);
            }
        }

        private Country _selectedCountry = Country.China;

        public Country SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                _selectedCountry = value;
                SetProperty(ref _selectedCountry, value);
            }
        }
    }

    public enum Gender
    {
        [Description("猛男")] Male,
        [Description("靓女")] Female,
        [Description("可猛可甜")] Unknown
    }

    public enum Country
    {
        [Description("中国")] China,
        [Description("漂亮国")] UnitedStates,
        [Description("小日本")] Japan
    }
}