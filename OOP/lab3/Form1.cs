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
using System.Net;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using System.Xml;
using System.ComponentModel.DataAnnotations;

namespace apartment_cost_calculator
{
    public partial class Form1 : Form
    {
        private InputSearchHandler inputHandler = new InputSearchHandler();
        private bool _isComboBox1Changed = false;
        private bool _isComboBox2Changed = false;
        private List<FormData> searchResults;
        private string _lastUserAction = "Действий не было";

        public Form1()
        {
            InitializeComponent();

            defaultRadioButton.Checked = true;
            orderComboBox.SelectedIndex = 2;

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

                // Валидация данных
                var validationContext = new ValidationContext(formData);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(formData, validationContext, validationResults, true))
                {
                    var errorMessages = validationResults.Select(r => r.ErrorMessage);
                    MessageBox.Show(string.Join(Environment.NewLine, errorMessages), "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<FormData> dataList;

                // Проверяем, существует ли файл
                if (File.Exists(filePath))
                {
                    // Читаем существующие данные
                    string existingJson = File.ReadAllText(filePath);
                    dataList = JsonConvert.DeserializeObject<List<FormData>>(existingJson) ?? new List<FormData>();
                }
                else
                {
                    // Если файла нет, создаем новый список
                    dataList = new List<FormData>();
                }

                // Добавляем новый объект в список
                dataList.Add(formData);

                // Сериализуем обновленный список в JSON
                string json = JsonConvert.SerializeObject(dataList, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, json);

                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _lastUserAction = "Сохранены данные о квартире";
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
                var dataList = JsonConvert.DeserializeObject<List<FormData>>(json);

                if (dataList == null || dataList.Count == 0)
                {
                    MessageBox.Show("Ошибка при загрузке данных: файл пуст или имеет неверный формат.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Валидация данных
                foreach (var formData in dataList)
                {
                    var validationContext = new ValidationContext(formData);
                    var validationResults = new List<ValidationResult>();
                    if (!Validator.TryValidateObject(formData, validationContext, validationResults, true))
                    {
                        var errorMessages = validationResults.Select(r => r.ErrorMessage);
                        MessageBox.Show(string.Join(Environment.NewLine, errorMessages), "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Загружаем последний объект из списка
                var formDataToLoad = dataList.Last();

                // Обновляем UI
                footageUpDown.Value = formDataToLoad.ApartmentData.Footage;
                roomsNumComboBox.SelectedItem = formDataToLoad.ApartmentData.RoomsCount.ToString();
                dateOfConstrTimePicker.Value = formDataToLoad.ApartmentData.ConstructionDate;
                materialComboBox.SelectedItem = formDataToLoad.ApartmentData.Material;
                floorUpDown.Value = formDataToLoad.ApartmentData.Floor;
                countryComboBox.SelectedItem = formDataToLoad.AddressData.Country;
                townComboBox.SelectedItem = formDataToLoad.AddressData.Town;
                districtTextBox.Text = formDataToLoad.AddressData.District;
                streetTextBox.Text = formDataToLoad.AddressData.Street;
                houseUpDown.Value = int.TryParse(formDataToLoad.AddressData.House, out int house) ? house : 0;
                frameTextBox.Text = formDataToLoad.AddressData.Frame;
                apartmentUpDown.Value = int.TryParse(formDataToLoad.AddressData.Apartment, out int apt) ? apt : 0;
                devNameTextBox.Text = formDataToLoad.DeveloperData.DeveloperName;
                legalAddressTextBox.Text = formDataToLoad.DeveloperData.LegalAddress;
                INNtextBox.Text = formDataToLoad.DeveloperData.INN;
                priceTrackBar.Value = formDataToLoad.ApartmentData.PricePerMeter;
                priceForMeterTextBox.Text = formDataToLoad.ApartmentData.PricePerMeter.ToString();
                dateOfSaleTimePicker.Value = formDataToLoad.ApartmentData.SaleDate;

                apartmentOptionsListBox.ClearSelected();
                for (int i = 0; i < apartmentOptionsListBox.Items.Count; i++)
                {
                    if (formDataToLoad.ApartmentData.ApartmentOptions.Contains(apartmentOptionsListBox.Items[i].ToString()))
                    {
                        apartmentOptionsListBox.SetItemChecked(i, true);
                    }
                }

                if (formDataToLoad.SelectedRadioButton == "OOO") OOOradioButton.Checked = true;
                else if (formDataToLoad.SelectedRadioButton == "IE") IEradioButton.Checked = true;
                else if (formDataToLoad.SelectedRadioButton == "OAO") OAOradioButton.Checked = true;

                MessageBox.Show("Данные успешно загружены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _lastUserAction = "Загружены данные о квартире";
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
                _lastUserAction = "Посчитана цена квартиры";
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
                _lastUserAction = "Изменена цена ща квадрат с помощью слайдера";
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
                    saveFileDialog.OverwritePrompt = false;
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
        private void aboutButton_Click(object sender, EventArgs e)
        {
            string developerInfo = "Разработчик: Хуторцов Кирилл Владимирович\nВерсия программы: 52.0.0";

            MessageBox.Show(developerInfo, "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _lastUserAction = "Кликнуто по 'О программе'";
        }
        private void searchButt_Click(object sender, EventArgs e)
        {
            try
            {
                // Открываем диалог выбора файла
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "JSON files (*.json)|*.json";
                    openFileDialog.Title = "Выберите JSON-файл для поиска";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;

                        // Загружаем данные из JSON-файла
                        string json = File.ReadAllText(filePath);
                        var dataList = JsonConvert.DeserializeObject<List<FormData>>(json);

                        if (dataList == null || dataList.Count == 0)
                        {
                            MessageBox.Show("Файл пуст или имеет неверный формат.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Получаем текст для поиска
                        string searchText = searchTextBox.Text;


                        if (regexRadioButton.Checked)
                        {
                            // Поиск по регулярному выражению
                            Regex regex = new Regex(searchText, RegexOptions.IgnoreCase);
                            searchResults = dataList
                                .Where(item =>
                                    regex.IsMatch(item.ApartmentData.Material) ||
                                    regex.IsMatch(item.AddressData.Country) ||
                                    regex.IsMatch(item.AddressData.Town) ||
                                    regex.IsMatch(item.DeveloperData.DeveloperName))
                                .ToList();
                        }
                        else
                        {
                            // Поиск по точному совпадению
                            searchResults = dataList
                                .Where(item =>
                                    item.ApartmentData.Material.Equals(searchText, StringComparison.OrdinalIgnoreCase) ||
                                    item.AddressData.Country.Equals(searchText, StringComparison.OrdinalIgnoreCase) ||
                                    item.AddressData.Town.Equals(searchText, StringComparison.OrdinalIgnoreCase) ||
                                    item.DeveloperData.DeveloperName.Equals(searchText, StringComparison.OrdinalIgnoreCase))
                                .ToList();
                        }

                        // Сортировка результатов
                        if (orderComboBox.SelectedItem != null)
                        {
                            string selectedOrder = orderComboBox.SelectedItem.ToString();

                            switch (selectedOrder)
                            {
                                case "Возрастанию":
                                    searchResults = searchResults
                                        .OrderBy(item => item.ApartmentData.PricePerMeter)
                                        .ToList();
                                    break;

                                case "Убыванию":
                                    searchResults = searchResults
                                        .OrderByDescending(item => item.ApartmentData.PricePerMeter)
                                        .ToList();
                                    break;

                                // "Без" сортировки (по умолчанию)
                                default:
                                    break;
                            }
                        }

                        // Выводим результаты поиска
                        if (searchResults.Count > 0)
                        {
                            StringBuilder resultMessage = new StringBuilder("Результаты поиска:\n\n");
                            foreach (var result in searchResults)
                            {
                                resultMessage.AppendLine($"Материал: {result.ApartmentData.Material}");
                                resultMessage.AppendLine($"Страна: {result.AddressData.Country}");
                                resultMessage.AppendLine($"Город: {result.AddressData.Town}");
                                resultMessage.AppendLine($"Застройщик: {result.DeveloperData.DeveloperName}");
                                resultMessage.AppendLine($"Цена за м²: {result.ApartmentData.PricePerMeter}");
                                resultMessage.AppendLine("-----------------------------");
                            }

                            MessageBox.Show(resultMessage.ToString(), "Результаты поиска", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _lastUserAction = "Выполнен поиск";
                        }
                        else
                        {
                            MessageBox.Show("Ничего не найдено.", "Результаты поиска", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            searchTextBox.TextChanged -= inputHandler.HandleTextChanged;

            inputHandler.RestorePreviousState();

            searchTextBox.Text = inputHandler.CurrentState;

            searchTextBox.TextChanged += inputHandler.HandleTextChanged;
            _lastUserAction = "Отменено изменение";
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            searchTextBox.TextChanged -= inputHandler.HandleTextChanged;

            inputHandler.RestoreNextState();

            searchTextBox.Text = inputHandler.CurrentState;

            searchTextBox.TextChanged += inputHandler.HandleTextChanged;
            _lastUserAction = "Отменено изменение";
        }

        private void cleanButton_Click(object sender, EventArgs e)
        {
            searchTextBox.TextChanged -= inputHandler.HandleTextChanged;

            inputHandler.ClearState();

            searchTextBox.Text = inputHandler.CurrentState;

            searchTextBox.TextChanged += inputHandler.HandleTextChanged;
            _lastUserAction = "Очищена строка поиска";
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            searchTextBox.TextChanged -= inputHandler.HandleTextChanged;

            inputHandler.RemoveLastCharacter();

            searchTextBox.Text = inputHandler.CurrentState;

            searchTextBox.TextChanged += inputHandler.HandleTextChanged;
            _lastUserAction = "Удален последний символ строки поиска";
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isComboBox2Changed) return;

            _isComboBox1Changed = true;

            if (orderComboBox.SelectedIndex >= 0 && orderComboBox.SelectedIndex < orderComboBox1.Items.Count)
            {
                if (orderComboBox.SelectedIndex != orderComboBox1.SelectedIndex)
                {
                    orderComboBox1.SelectedIndex = orderComboBox.SelectedIndex;
                }
            }

            _isComboBox1Changed = false;
            _lastUserAction = "Выбран фильтр";
        }
        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isComboBox1Changed) return;

            _isComboBox2Changed = true;

            if (orderComboBox1.SelectedIndex >= 0 && orderComboBox1.SelectedIndex < orderComboBox.Items.Count)
            {
                if (orderComboBox1.SelectedIndex != orderComboBox.SelectedIndex)
                {
                    orderComboBox.SelectedIndex = orderComboBox1.SelectedIndex;
                }
            }

            _isComboBox2Changed = false;
            _lastUserAction = "Выбран фильтр";
        }

        private void toggleVisibityButton_Click(object sender, EventArgs e)
        {
            // Переключаем видимость кнопок
            prevButton.Visible = !prevButton.Visible;
            nextButton.Visible = !nextButton.Visible;
            cleanButton.Visible = !cleanButton.Visible;
            deleteButton.Visible = !deleteButton.Visible;
            searchButton1.Visible = !searchButton1.Visible;

            // Переключаем видимость ComboBox
            orderComboBox1.Visible = !orderComboBox1.Visible;

            // Обновляем текст кнопки в зависимости от состояния
            toggleVisibityButton.Text = prevButton.Visible ? "Скрыть элементы" : "Показать элементы";
            _lastUserAction = $"{toggleVisibityButton.Text}";
        }
        private void saveButton1_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, есть ли результаты поиска
                if (searchResults == null || searchResults.Count == 0)
                {
                    MessageBox.Show("Нет данных для сохранения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Открываем диалог сохранения файла
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "JSON files (*.json)|*.json";
                    saveFileDialog.Title = "Сохранить результаты поиска";
                    saveFileDialog.DefaultExt = "json";
                    saveFileDialog.AddExtension = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;

                        // Сериализуем результаты поиска в JSON
                        string json = JsonConvert.SerializeObject(searchResults, Newtonsoft.Json.Formatting.Indented);

                        // Сохраняем JSON в файл
                        File.WriteAllText(filePath, json);

                        MessageBox.Show("Результаты поиска успешно сохранены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _lastUserAction = "Сохранены результаты поиска";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении результатов поиска: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void analyzeButton_Click(object sender, EventArgs e)
        {
            // Пример списка файлов для анализа
            List<string> filePaths = new List<string>
            {
                "C:\\BSTU\\SEM4\\OOP\\data.json",
                "C:\\BSTU\\SEM4\\OOP\\requests.json",
                "C:\\BSTU\\SEM4\\OOP\\xkk.json"
            };

            AnalyzeJsonFiles(filePaths);
        }
        private void AnalyzeJsonFiles(List<string> filePaths)
        {
            try
            {
                int totalObjects = 0;
                StringBuilder analysisResult = new StringBuilder();

                foreach (var filePath in filePaths)
                {
                    if (!File.Exists(filePath))
                    {
                        analysisResult.AppendLine($"Файл не найден: {filePath}");
                        continue;
                    }

                    string json = File.ReadAllText(filePath);
                    var dataList = JsonConvert.DeserializeObject<List<FormData>>(json);

                    if (dataList == null)
                    {
                        analysisResult.AppendLine($"Ошибка при чтении файла: {filePath}");
                        continue;
                    }

                    int objectCount = dataList.Count;
                    totalObjects += objectCount;

                    analysisResult.AppendLine($"Файл: {filePath}");
                    analysisResult.AppendLine($"Количество объектов: {objectCount}");
                    analysisResult.AppendLine();
                }

                analysisResult.AppendLine($"Общее количество объектов: {totalObjects}");
                analysisResult.AppendLine($"Последнее действие: {_lastUserAction}");

                // Обновляем infoLabel
                infoLabel.Text = analysisResult.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при анализе JSON-файлов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void resultLabel_Click(object sender, EventArgs e)
        {

        }

        private void resultTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}