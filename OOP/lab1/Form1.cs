using System;
using System.Windows.Forms;

namespace ConstructionCalculator
{
    public partial class MainForm : Form
    {
      
        public delegate double ConversionHandler(double value);
        public delegate double CalculationHandler(double area);

      
        public event Action<string> CalculationStarted;
        public event Action<string> CalculationCompleted;
        public event Action<string> ErrorOccurred;

        public MainForm()
        {
            InitializeComponent();
            SubscribeEvents();
            InitializeComponents();
        }

        private void SubscribeEvents()
        {
            btnCalculate.Click += BtnCalculate_Click;
            CalculationStarted += OnCalculationStarted;
            CalculationCompleted += OnCalculationCompleted;
            ErrorOccurred += OnErrorOccurred;
        }

        private void InitializeComponents()
        {
            cmbMaterial.Items.AddRange(new object[] { "Ламинат", "Плитка", "Обои" });
            cmbUnits.Items.AddRange(new object[] { "Метры", "Футы" });
            cmbMaterial.SelectedIndex = 0;
            cmbUnits.SelectedIndex = 0;
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                CalculationStarted?.Invoke("Начало расчета...");

                var input = GetInputValues();
                ValidateInput(input);

                var converter = GetConverter();
                var calculator = GetCalculator();

                var results = CalculateResults(input, converter, calculator);

                DisplayResults(results);
                CalculationCompleted?.Invoke("");
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
            }
        }

        private InputData GetInputValues()
        {
            return new InputData(
                Length: double.Parse(txtLength.Text),
                Width: double.Parse(txtWidth.Text),
                Height: double.Parse(txtHeight.Text),
                Material: cmbMaterial.SelectedItem.ToString(),
                Units: cmbUnits.SelectedIndex
            );
        }

        private void ValidateInput(InputData data)
        {
            if (data.Length <= 0 || data.Width <= 0 || data.Height <= 0)
                throw new ArgumentException("Все размеры должны быть положительными числами");
        }

        private ConversionHandler GetConverter()
        {
            return cmbUnits.SelectedIndex == 0
                ? new ConversionHandler(ConvertToSquareMeters)
                : new ConversionHandler(ConvertToSquareFeet);
        }

        private CalculationHandler GetCalculator()
        {
            return cmbMaterial.SelectedIndex switch
            {
                0 => CalculateLaminate,
                1 => CalculateTiles,
                2 => CalculateWallpaper,
                _ => throw new ArgumentException("Неверный тип материала")
            };
        }

        private CalculationResults CalculateResults(InputData data,
                                                   ConversionHandler converter,
                                                   CalculationHandler calculator)
        {
            double floorArea = data.Length * data.Width;
            double wallsArea = 2 * (data.Length + data.Width) * data.Height;

            return new CalculationResults(
                FloorArea: converter(floorArea),
                WallsArea: converter(wallsArea),
                MaterialNeeded: calculator(data.Material == "Обои" ? wallsArea : floorArea)
            );
        }

        private void DisplayResults(CalculationResults results)
        {
            string floorArea = $"Площадь пола: {results.FloorArea:F2} {GetUnitSymbol()}";
            string wallsArea = $"Площадь стен: {results.WallsArea:F2} {GetUnitSymbol()}";
            string materialNeeded = $"Требуется материала: {Math.Ceiling(results.MaterialNeeded)} {GetMaterialUnit()}";
            string separator = new string('─', 30);

            txtResults.AppendText(
                string.Join(Environment.NewLine,
                    Environment.NewLine, // Добавляем пустую строку
                    floorArea,
                    wallsArea,
                    materialNeeded,
                    separator,
                    "Расчет завершен!")
            );
        }

        // Конверсионные методы
        private double ConvertToSquareFeet(double meters) => meters * 10.7639;
        private double ConvertToSquareMeters(double feet) => feet;

        // Методы расчета материалов
        private double CalculateLaminate(double area) => area * 1.1 / 2.5;
        private double CalculateTiles(double area) => area * 1.15 / 1.8;
        private double CalculateWallpaper(double area) => area * 1.2 / 5.0;

        // Вспомогательные методы
        private string GetUnitSymbol() => cmbUnits.SelectedIndex == 0 ? "м²" : "фут²";
        private string GetMaterialUnit() => cmbMaterial.SelectedIndex == 2 ? "рулонов" : "упаковок";

        // Обработчики событий
        private void OnCalculationStarted(string message)
        {
            txtResults.Text = message;
            btnCalculate.Enabled = false;
        }

        private void OnCalculationCompleted(string message)
        {
            txtResults.AppendText($"\n{message}");
            btnCalculate.Enabled = true;
        }

        private void OnErrorOccurred(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            btnCalculate.Enabled = true;
        }

        // Классы данных
        private record InputData(
            double Length,
            double Width,
            double Height,
            string Material,
            int Units
        );

        private record CalculationResults(
            double FloorArea,
            double WallsArea,
            double MaterialNeeded
        );
    }
}