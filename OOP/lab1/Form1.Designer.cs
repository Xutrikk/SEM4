namespace ConstructionCalculator
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox txtLength;
        private TextBox txtWidth;
        private TextBox txtHeight;
        private ComboBox cmbMaterial;
        private ComboBox cmbUnits;
        private Button btnCalculate;
        private TextBox txtResults;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtLength = new TextBox();
            this.txtWidth = new TextBox();
            this.txtHeight = new TextBox();
            this.cmbMaterial = new ComboBox();
            this.cmbUnits = new ComboBox();
            this.btnCalculate = new Button();
            this.txtResults = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();

            // Настройка формы
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(400, 450);
            this.Text = "Строительный калькулятор";

            // Настройка элементов
            // txtLength
            this.txtLength.Location = new System.Drawing.Point(120, 20);
            this.txtLength.Size = new System.Drawing.Size(150, 20);

            // txtWidth
            this.txtWidth.Location = new System.Drawing.Point(120, 60);
            this.txtWidth.Size = new System.Drawing.Size(150, 20);

            // txtHeight
            this.txtHeight.Location = new System.Drawing.Point(120, 100);
            this.txtHeight.Size = new System.Drawing.Size(150, 20);

            // cmbMaterial
            this.cmbMaterial.Location = new System.Drawing.Point(120, 140);
            this.cmbMaterial.Size = new System.Drawing.Size(150, 21);

            // cmbUnits
            this.cmbUnits.Location = new System.Drawing.Point(120, 180);
            this.cmbUnits.Size = new System.Drawing.Size(150, 21);

            // btnCalculate
            this.btnCalculate.Location = new System.Drawing.Point(120, 220);
            this.btnCalculate.Size = new System.Drawing.Size(150, 30);
            this.btnCalculate.Text = "Рассчитать";

            // txtResults
            this.txtResults.Multiline = true;
            this.txtResults.Location = new System.Drawing.Point(20, 260);
            this.txtResults.Size = new System.Drawing.Size(350, 150);
            this.txtResults.ScrollBars = ScrollBars.Vertical;
            this.txtResults.ReadOnly = true;

            // Labels
            this.label1.Text = "Длина:";
            this.label1.Location = new System.Drawing.Point(20, 20);

            this.label2.Text = "Ширина:";
            this.label2.Location = new System.Drawing.Point(20, 60);

            this.label3.Text = "Высота:";
            this.label3.Location = new System.Drawing.Point(20, 100);

            this.label4.Text = "Материал:";
            this.label4.Location = new System.Drawing.Point(20, 140);

            this.label5.Text = "Единицы:";
            this.label5.Location = new System.Drawing.Point(20, 180);

            // Добавление элементов на форму
            this.Controls.AddRange(new Control[] {
                txtLength, txtWidth, txtHeight,
                cmbMaterial, cmbUnits,
                btnCalculate, txtResults,
                label1, label2, label3, label4, label5
            });

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}