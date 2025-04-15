using System;
using System.Collections.Generic;

namespace apartment_cost_calculator
{

    public class Apartment
    {
        public decimal Footage { get; set; } 
        public int RoomsCount { get; set; } 
        public List<string> ApartmentOptions { get; set; } 
        public DateTime ConstructionDate { get; set; } 
        public string Material { get; set; } 
        public int Floor { get; set; } 
        public int PricePerMeter { get; set; } 
        public DateTime SaleDate { get; set; } 
    }

    public class Address
    {
        public string Country { get; set; } 
        public string Town { get; set; } 
        public string District { get; set; } 
        public string Street { get; set; } 
        public string House { get; set; } 
        public string Frame { get; set; } 
        public string Apartment { get; set; } 
    }

    public class Developer
    {
        public string DeveloperName { get; set; } 
        public string LegalAddress { get; set; } 
        public string INN { get; set; } 
    }

    public class FormData
    {
        public Apartment ApartmentData { get; set; } 
        public Address AddressData { get; set; } 
        public Developer DeveloperData { get; set; } 
        public string SelectedRadioButton { get; set; } 
    }
}
