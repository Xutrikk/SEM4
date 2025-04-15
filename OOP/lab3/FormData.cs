using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace apartment_cost_calculator
{
    public class Apartment
    {
        [Range(0.1, 1000, ErrorMessage = "Метраж должен быть от 0.1 до 1000.")]
        public decimal Footage { get; set; }

        [Range(0, 10, ErrorMessage = "Количество комнат должно быть от 0 до 10.")]
        public int RoomsCount { get; set; }

        public List<string> ApartmentOptions { get; set; }

        [Required(ErrorMessage = "Дата постройки обязательна.")]
        public DateTime ConstructionDate { get; set; }

        [Required(ErrorMessage = "Материал обязателен.")]
        public string Material { get; set; }

        [Range(0, 100, ErrorMessage = "Этаж должен быть от 0 до 100.")]
        public int Floor { get; set; }

        [Range(0, 1000000, ErrorMessage = "Цена за квадратный метр должна быть от 0 до 1 000 000.")]
        public int PricePerMeter { get; set; }

        [Required(ErrorMessage = "Дата продажи обязательна.")]
        public DateTime SaleDate { get; set; }
    }

    public class Address
    {
        [Required(ErrorMessage = "Страна обязательна.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Город обязателен.")]
        public string Town { get; set; }

        public string District { get; set; }

        [Required(ErrorMessage = "Улица обязательна.")]
        public string Street { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Номер дома должен быть числом.")]
        public string House { get; set; }

        public string Frame { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Номер квартиры должен быть числом.")]
        public string Apartment { get; set; }
    }

    public class Developer
    {
        [Required(ErrorMessage = "Название застройщика обязательно.")]
        public string DeveloperName { get; set; }

        [Required(ErrorMessage = "Юридический адрес обязателен.")]
        public string LegalAddress { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "ИНН должен состоять из 10 цифр.")]
        public string INN { get; set; }
    }


    public class FormData
    {
        [ValidateObject]
        public Apartment ApartmentData { get; set; }

        [ValidateObject]
        public Address AddressData { get; set; }

        [ValidateObject]
        public Developer DeveloperData { get; set; }

        public string SelectedRadioButton { get; set; }
    }

}
