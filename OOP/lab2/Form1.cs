using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace apartment_cost_calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.BackgroundImage = Image.FromFile("C:\\BSTU\\SEM4\\OOP\\bill.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void SaveDataToJson(string filePath)
        {
            try
            {
                string selectedRadioButton = null;
                if (OOOradioButton.Checked) selectedRadioButton = "OOO";
                else if (IEradioButton.Checked) selectedRadioButton = "IE";
                else if (OAOradioButton.Checked) selectedRadioButton = "OAO";

                var formData = new FormData
                {
                    ApartmentData = new Apartment
                    {
                        Footage = footageUpDown.Value,
                        RoomsCount = int.TryParse(roomsNumComboBox.SelectedItem?.ToString(), out int rooms) ? rooms : 0,
                        ApartmentOptions = apartmentOptionsListBox.CheckedItems.Cast<string>().ToList(),
                        ConstructionDate = dateOfConstrTimePicker.Value,
                        Material = materialComboBox.SelectedItem?.ToString() ?? "",
                        Floor = (int)floorUpDown.Value,
                        PricePerMeter = priceTrackBar.Value,
                        SaleDate = dateOfSaleTimePicker.Value
                    },
                    AddressData = new Address
                    {
                        Country = countryComboBox.SelectedItem?.ToString() ?? "",
                        Town = townComboBox.SelectedItem?.ToString() ?? "",
                        District = districtTextBox.Text,
                        Street = streetTextBox.Text,
                        House = houseUpDown.Value.ToString(),
                        Frame = frameTextBox.Text,
                        Apartment = apartmentUpDown.Value.ToString()
                    },
                    DeveloperData = new Developer
                    {
                        DeveloperName = devNameTextBox.Text,
                        LegalAddress = legalAddressTextBox.Text,
                        INN = INNtextBox.Text
                    },
                    SelectedRadioButton = selectedRadioButton
                };

                string json = JsonConvert.SerializeObject(formData, Formatting.Indented);
                File.WriteAllText(filePath, json);

                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDataFromJson(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Файл не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string json = File.ReadAllText(filePath);
                var formData = JsonConvert.DeserializeObject<FormData>(json);

                if (formData == null)
                {
                    MessageBox.Show("Ошибка при загрузке данных: файл пуст или имеет неверный формат.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                footageUpDown.Value = formData.ApartmentData.Footage;
                roomsNumComboBox.SelectedItem = formData.ApartmentData.RoomsCount.ToString();
                dateOfConstrTimePicker.Value = formData.ApartmentData.ConstructionDate;
                materialComboBox.SelectedItem = formData.ApartmentData.Material;
                floorUpDown.Value = formData.ApartmentData.Floor;
                countryComboBox.SelectedItem = formData.AddressData.Country;
                townComboBox.SelectedItem = formData.AddressData.Town;
                districtTextBox.Text = formData.AddressData.District;
                streetTextBox.Text = formData.AddressData.Street;
                houseUpDown.Value = int.TryParse(formData.AddressData.House, out int house) ? house : 0;
                frameTextBox.Text = formData.AddressData.Frame;
                apartmentUpDown.Value = int.TryParse(formData.AddressData.Apartment, out int apt) ? apt : 0;
                devNameTextBox.Text = formData.DeveloperData.DeveloperName;
                legalAddressTextBox.Text = formData.DeveloperData.LegalAddress;
                INNtextBox.Text = formData.DeveloperData.INN;
                priceTrackBar.Value = formData.ApartmentData.PricePerMeter;
                priceForMeterTextBox.Text = formData.ApartmentData.PricePerMeter.ToString();
                dateOfSaleTimePicker.Value = formData.ApartmentData.SaleDate;

                apartmentOptionsListBox.ClearSelected();
                for (int i = 0; i < apartmentOptionsListBox.Items.Count; i++)
                {
                    if (formData.ApartmentData.ApartmentOptions.Contains(apartmentOptionsListBox.Items[i].ToString()))
                    {
                        apartmentOptionsListBox.SetItemChecked(i, true);
                    }
                }

                if (formData.SelectedRadioButton == "OOO") OOOradioButton.Checked = true;
                else if (formData.SelectedRadioButton == "IE") IEradioButton.Checked = true;
                else if (formData.SelectedRadioButton == "OAO") OAOradioButton.Checked = true;

                MessageBox.Show("Данные успешно загружены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void calculateButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем значения из элементов управления
                decimal footage = footageUpDown.Value; // Метраж квартиры
                decimal pricePerMeter = Math.Abs(decimal.Parse(priceForMeterTextBox.Text)); // Цена за квадратный метр

                // Базовая стоимость квартиры
                decimal baseCost = footage * pricePerMeter;

                // Учет количества комнат
                int roomsCount = int.Parse(roomsNumComboBox.SelectedItem.ToString());
                decimal roomsFactor = 1.0m + (roomsCount - 1) * 0.1m; // Увеличиваем стоимость на 10% за каждую дополнительную комнату

                // Учет материала постройки
                string material = materialComboBox.SelectedItem.ToString();
                decimal materialFactor = 1.0m;
                switch (material)
                {
                    case "Бетон":
                        materialFactor = 1.0m;
                        break;
                    case "Дерево":
                        materialFactor = 1.2m;
                        break;
                    case "Кирпич":
                        materialFactor = 0.8m;
                        break;
                }

                // Учет этажа
                int floor = (int)floorUpDown.Value;
                decimal floorFactor = 1.0m;
                if (floor == 1 || floor == 5) // Пример: первый и последний этажи дешевле
                {
                    floorFactor = 0.9m;
                }
                else if (floor == 2 || floor == 3 || floor == 4)
                {
                    floorFactor = 1.1m;
                }

                // Учет дополнительных опций квартиры
                decimal optionsFactor = 1.0m;
                foreach (var option in apartmentOptionsListBox.CheckedItems)
                {
                    switch (option.ToString())
                    {
                        case "Кухня":
                            optionsFactor += 0.05m;
                            break;
                        case "Ванна":
                            optionsFactor += 0.03m;
                            break;
                        case "Туалет":
                            optionsFactor += 0.02m;
                            break;
                        case "Подвал":
                            optionsFactor += 0.1m;
                            break;
                        case "Балкон":
                            optionsFactor += 0.07m;
                            break;
                    }
                }

                DateTime constructionDate = dateOfConstrTimePicker.Value;     // Дата постройки из DateTimePicker
                DateTime currentDate = DateTime.Now;                          // Текущая дата
                int ageOfBuilding = currentDate.Year - constructionDate.Year; // Возраст дома в годах

                // Коэффициент для учета возраста дома
                decimal ageFactor = 1.0m;
                if (ageOfBuilding <= 5)
                {
                    ageFactor = 1.2m; 
                }
                else if (ageOfBuilding <= 10)
                {
                    ageFactor = 1.1m; 
                }
                else if (ageOfBuilding <= 20)
                {
                    ageFactor = 1.0m; 
                }
                else
                {
                    ageFactor = 0.9m; 
                }

                decimal locationFactor = 1.2m;
                if (townComboBox.SelectedItem.ToString() != "Минск") locationFactor = 0.8m;


                // Итоговая стоимость
                decimal totalCost = baseCost * roomsFactor * materialFactor * floorFactor * optionsFactor * ageFactor * locationFactor;

                // Выводим результат
                resultTextBox.Text = $"Результат расчета: {totalCost.ToString("C2", new System.Globalization.CultureInfo("en-US"))}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при расчете стоимости: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void prohibitTextChanges(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void priceTrackBar_Scroll(object sender, EventArgs e)
        {
            try
            {
                // Получаем текущее значение ползунка
                int trackBarValue = priceTrackBar.Value;

                // Устанавливаем это значение в текстовое поле priceForMeterTextBox
                priceForMeterTextBox.Text = trackBarValue.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении слайдера: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "JSON files (*.json)|*.json";
                    saveFileDialog.Title = "Сохранить данные";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        SaveDataToJson(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "JSON files (*.json)|*.json";
                    openFileDialog.Title = "Загрузить данные";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        LoadDataFromJson(openFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void banNon_floats(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '.')
                {
                    e.Handled = true; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вводе символа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void banNon_int(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вводе символа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}