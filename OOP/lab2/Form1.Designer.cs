namespace apartment_cost_calculator
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.footageUpDown = new System.Windows.Forms.NumericUpDown();
            this.roomsNumComboBox = new System.Windows.Forms.ComboBox();
            this.apartmentOptionsListBox = new System.Windows.Forms.CheckedListBox();
            this.dateOfConstrTimePicker = new System.Windows.Forms.DateTimePicker();
            this.materialComboBox = new System.Windows.Forms.ComboBox();
            this.floorUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.houseUpDown = new System.Windows.Forms.NumericUpDown();
            this.apartmentUpDown = new System.Windows.Forms.NumericUpDown();
            this.countryComboBox = new System.Windows.Forms.ComboBox();
            this.townComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.devNameTextBox = new System.Windows.Forms.TextBox();
            this.OOOradioButton = new System.Windows.Forms.RadioButton();
            this.OAOradioButton = new System.Windows.Forms.RadioButton();
            this.IEradioButton = new System.Windows.Forms.RadioButton();
            this.legalAddressTextBox = new System.Windows.Forms.TextBox();
            this.INNtextBox = new System.Windows.Forms.TextBox();
            this.priceTrackBar = new System.Windows.Forms.TrackBar();
            this.dateOfSaleTimePicker = new System.Windows.Forms.DateTimePicker();
            this.priceForMeterTextBox = new System.Windows.Forms.TextBox();
            this.calculateButton = new System.Windows.Forms.Button();
            this.resultLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.resultTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.districtTextBox = new System.Windows.Forms.TextBox();
            this.streetTextBox = new System.Windows.Forms.TextBox();
            this.frameTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.footageUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.floorUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.houseUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.apartmentUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.priceTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Квартира";
            // 
            // footageUpDown
            // 
            this.footageUpDown.Location = new System.Drawing.Point(17, 173);
            this.footageUpDown.Name = "footageUpDown";
            this.footageUpDown.Size = new System.Drawing.Size(157, 22);
            this.footageUpDown.TabIndex = 1;
            this.footageUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.banNon_floats);
            // 
            // roomsNumComboBox
            // 
            this.roomsNumComboBox.FormattingEnabled = true;
            this.roomsNumComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.roomsNumComboBox.Location = new System.Drawing.Point(17, 202);
            this.roomsNumComboBox.Name = "roomsNumComboBox";
            this.roomsNumComboBox.Size = new System.Drawing.Size(157, 24);
            this.roomsNumComboBox.TabIndex = 2;
            this.roomsNumComboBox.Text = "Кол-во комнат";
            this.roomsNumComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // apartmentOptionsListBox
            // 
            this.apartmentOptionsListBox.FormattingEnabled = true;
            this.apartmentOptionsListBox.Items.AddRange(new object[] {
            "Кухня",
            "Ванна",
            "Туалет",
            "Подвал",
            "Балкон "});
            this.apartmentOptionsListBox.Location = new System.Drawing.Point(17, 233);
            this.apartmentOptionsListBox.Name = "apartmentOptionsListBox";
            this.apartmentOptionsListBox.Size = new System.Drawing.Size(157, 89);
            this.apartmentOptionsListBox.TabIndex = 3;
            this.apartmentOptionsListBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // dateOfConstrTimePicker
            // 
            this.dateOfConstrTimePicker.Location = new System.Drawing.Point(17, 329);
            this.dateOfConstrTimePicker.Name = "dateOfConstrTimePicker";
            this.dateOfConstrTimePicker.Size = new System.Drawing.Size(157, 22);
            this.dateOfConstrTimePicker.TabIndex = 4;
            this.dateOfConstrTimePicker.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // materialComboBox
            // 
            this.materialComboBox.FormattingEnabled = true;
            this.materialComboBox.Items.AddRange(new object[] {
            "Бетон",
            "Дерево",
            "Кирпич"});
            this.materialComboBox.Location = new System.Drawing.Point(17, 358);
            this.materialComboBox.Name = "materialComboBox";
            this.materialComboBox.Size = new System.Drawing.Size(157, 24);
            this.materialComboBox.TabIndex = 5;
            this.materialComboBox.Text = "Материал";
            this.materialComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // floorUpDown
            // 
            this.floorUpDown.Location = new System.Drawing.Point(17, 422);
            this.floorUpDown.Name = "floorUpDown";
            this.floorUpDown.Size = new System.Drawing.Size(157, 22);
            this.floorUpDown.TabIndex = 6;
            this.floorUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.banNon_int);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(261, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 29);
            this.label2.TabIndex = 7;
            this.label2.Text = "Адрес";
            // 
            // houseUpDown
            // 
            this.houseUpDown.Location = new System.Drawing.Point(229, 303);
            this.houseUpDown.Name = "houseUpDown";
            this.houseUpDown.Size = new System.Drawing.Size(161, 22);
            this.houseUpDown.TabIndex = 12;
            this.houseUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.banNon_int);
            // 
            // apartmentUpDown
            // 
            this.apartmentUpDown.Location = new System.Drawing.Point(227, 425);
            this.apartmentUpDown.Name = "apartmentUpDown";
            this.apartmentUpDown.Size = new System.Drawing.Size(161, 22);
            this.apartmentUpDown.TabIndex = 14;
            this.apartmentUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.banNon_int);
            // 
            // countryComboBox
            // 
            this.countryComboBox.FormattingEnabled = true;
            this.countryComboBox.Items.AddRange(new object[] {
            "Беларусь"});
            this.countryComboBox.Location = new System.Drawing.Point(228, 146);
            this.countryComboBox.Name = "countryComboBox";
            this.countryComboBox.Size = new System.Drawing.Size(162, 24);
            this.countryComboBox.TabIndex = 15;
            this.countryComboBox.Text = "Страна";
            this.countryComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // townComboBox
            // 
            this.townComboBox.FormattingEnabled = true;
            this.townComboBox.Items.AddRange(new object[] {
            "Минск",
            "Гомель",
            "Гродно"});
            this.townComboBox.Location = new System.Drawing.Point(227, 176);
            this.townComboBox.Name = "townComboBox";
            this.townComboBox.Size = new System.Drawing.Size(162, 24);
            this.townComboBox.TabIndex = 19;
            this.townComboBox.Text = "Город";
            this.townComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(452, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 29);
            this.label3.TabIndex = 20;
            this.label3.Text = "Застройщик";
            // 
            // devNameTextBox
            // 
            this.devNameTextBox.Location = new System.Drawing.Point(445, 146);
            this.devNameTextBox.Name = "devNameTextBox";
            this.devNameTextBox.Size = new System.Drawing.Size(175, 22);
            this.devNameTextBox.TabIndex = 21;
            this.devNameTextBox.Text = "Название";
            // 
            // OOOradioButton
            // 
            this.OOOradioButton.AutoSize = true;
            this.OOOradioButton.Location = new System.Drawing.Point(445, 178);
            this.OOOradioButton.Name = "OOOradioButton";
            this.OOOradioButton.Size = new System.Drawing.Size(58, 20);
            this.OOOradioButton.TabIndex = 22;
            this.OOOradioButton.TabStop = true;
            this.OOOradioButton.Text = "ООО";
            this.OOOradioButton.UseVisualStyleBackColor = true;
            // 
            // OAOradioButton
            // 
            this.OAOradioButton.AutoSize = true;
            this.OAOradioButton.Location = new System.Drawing.Point(563, 179);
            this.OAOradioButton.Name = "OAOradioButton";
            this.OAOradioButton.Size = new System.Drawing.Size(57, 20);
            this.OAOradioButton.TabIndex = 23;
            this.OAOradioButton.TabStop = true;
            this.OAOradioButton.Text = "ОАО";
            this.OAOradioButton.UseVisualStyleBackColor = true;
            // 
            // IEradioButton
            // 
            this.IEradioButton.AutoSize = true;
            this.IEradioButton.Location = new System.Drawing.Point(509, 178);
            this.IEradioButton.Name = "IEradioButton";
            this.IEradioButton.Size = new System.Drawing.Size(48, 20);
            this.IEradioButton.TabIndex = 24;
            this.IEradioButton.TabStop = true;
            this.IEradioButton.Text = "ИП";
            this.IEradioButton.UseVisualStyleBackColor = true;
            // 
            // legalAddressTextBox
            // 
            this.legalAddressTextBox.Location = new System.Drawing.Point(445, 224);
            this.legalAddressTextBox.Name = "legalAddressTextBox";
            this.legalAddressTextBox.Size = new System.Drawing.Size(175, 22);
            this.legalAddressTextBox.TabIndex = 25;
            this.legalAddressTextBox.Text = "Юрид. адрес";
            // 
            // INNtextBox
            // 
            this.INNtextBox.Location = new System.Drawing.Point(445, 272);
            this.INNtextBox.Name = "INNtextBox";
            this.INNtextBox.Size = new System.Drawing.Size(175, 22);
            this.INNtextBox.TabIndex = 26;
            this.INNtextBox.Text = "ИНН";
            // 
            // priceTrackBar
            // 
            this.priceTrackBar.Location = new System.Drawing.Point(445, 331);
            this.priceTrackBar.Maximum = 150;
            this.priceTrackBar.Name = "priceTrackBar";
            this.priceTrackBar.Size = new System.Drawing.Size(157, 56);
            this.priceTrackBar.TabIndex = 27;
            this.priceTrackBar.Scroll += new System.EventHandler(this.priceTrackBar_Scroll);
            this.priceTrackBar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // dateOfSaleTimePicker
            // 
            this.dateOfSaleTimePicker.Location = new System.Drawing.Point(445, 422);
            this.dateOfSaleTimePicker.Name = "dateOfSaleTimePicker";
            this.dateOfSaleTimePicker.Size = new System.Drawing.Size(157, 22);
            this.dateOfSaleTimePicker.TabIndex = 28;
            this.dateOfSaleTimePicker.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // priceForMeterTextBox
            // 
            this.priceForMeterTextBox.Location = new System.Drawing.Point(445, 385);
            this.priceForMeterTextBox.Name = "priceForMeterTextBox";
            this.priceForMeterTextBox.Size = new System.Drawing.Size(157, 22);
            this.priceForMeterTextBox.TabIndex = 29;
            this.priceForMeterTextBox.Text = "Цена за кв.м $";
            this.priceForMeterTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.banNon_floats);
            // 
            // calculateButton
            // 
            this.calculateButton.Location = new System.Drawing.Point(445, 463);
            this.calculateButton.Name = "calculateButton";
            this.calculateButton.Size = new System.Drawing.Size(157, 45);
            this.calculateButton.TabIndex = 30;
            this.calculateButton.Text = "Рассчитать";
            this.calculateButton.UseVisualStyleBackColor = true;
            this.calculateButton.Click += new System.EventHandler(this.calculateButton_Click);
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.resultLabel.Location = new System.Drawing.Point(201, 28);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(249, 29);
            this.resultLabel.TabIndex = 31;
            this.resultLabel.Text = "Итоговый результат";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(17, 463);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(157, 45);
            this.saveButton.TabIndex = 34;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(227, 463);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(157, 45);
            this.loadButton.TabIndex = 35;
            this.loadButton.Text = "Загрузить";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // resultTextBox
            // 
            this.resultTextBox.Location = new System.Drawing.Point(161, 77);
            this.resultTextBox.Name = "resultTextBox";
            this.resultTextBox.Size = new System.Drawing.Size(323, 22);
            this.resultTextBox.TabIndex = 36;
            this.resultTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(13, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 20);
            this.label6.TabIndex = 37;
            this.label6.Text = "Метраж";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(13, 399);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 20);
            this.label7.TabIndex = 38;
            this.label7.Text = "Этаж";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(227, 281);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 20);
            this.label8.TabIndex = 39;
            this.label8.Text = "Дом";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(223, 402);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 20);
            this.label9.TabIndex = 40;
            this.label9.Text = "Номер кв.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(441, 308);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(178, 20);
            this.label10.TabIndex = 41;
            this.label10.Text = "Слайдер стоимости";
            // 
            // districtTextBox
            // 
            this.districtTextBox.Location = new System.Drawing.Point(228, 206);
            this.districtTextBox.Name = "districtTextBox";
            this.districtTextBox.Size = new System.Drawing.Size(160, 22);
            this.districtTextBox.TabIndex = 42;
            this.districtTextBox.Text = "Район";
            // 
            // streetTextBox
            // 
            this.streetTextBox.Location = new System.Drawing.Point(227, 236);
            this.streetTextBox.Name = "streetTextBox";
            this.streetTextBox.Size = new System.Drawing.Size(160, 22);
            this.streetTextBox.TabIndex = 43;
            this.streetTextBox.Text = "Улица";
            // 
            // frameTextBox
            // 
            this.frameTextBox.Location = new System.Drawing.Point(227, 363);
            this.frameTextBox.Name = "frameTextBox";
            this.frameTextBox.Size = new System.Drawing.Size(160, 22);
            this.frameTextBox.TabIndex = 44;
            this.frameTextBox.Text = "Корпус";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 520);
            this.Controls.Add(this.frameTextBox);
            this.Controls.Add(this.streetTextBox);
            this.Controls.Add(this.districtTextBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.resultTextBox);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.resultLabel);
            this.Controls.Add(this.calculateButton);
            this.Controls.Add(this.priceForMeterTextBox);
            this.Controls.Add(this.dateOfSaleTimePicker);
            this.Controls.Add(this.priceTrackBar);
            this.Controls.Add(this.INNtextBox);
            this.Controls.Add(this.legalAddressTextBox);
            this.Controls.Add(this.IEradioButton);
            this.Controls.Add(this.OAOradioButton);
            this.Controls.Add(this.OOOradioButton);
            this.Controls.Add(this.devNameTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.townComboBox);
            this.Controls.Add(this.countryComboBox);
            this.Controls.Add(this.apartmentUpDown);
            this.Controls.Add(this.houseUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.floorUpDown);
            this.Controls.Add(this.materialComboBox);
            this.Controls.Add(this.dateOfConstrTimePicker);
            this.Controls.Add(this.apartmentOptionsListBox);
            this.Controls.Add(this.roomsNumComboBox);
            this.Controls.Add(this.footageUpDown);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.footageUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.floorUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.houseUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.apartmentUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.priceTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown footageUpDown;
        private System.Windows.Forms.ComboBox roomsNumComboBox;
        private System.Windows.Forms.CheckedListBox apartmentOptionsListBox;
        private System.Windows.Forms.DateTimePicker dateOfConstrTimePicker;
        private System.Windows.Forms.ComboBox materialComboBox;
        private System.Windows.Forms.NumericUpDown floorUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown houseUpDown;
        private System.Windows.Forms.NumericUpDown apartmentUpDown;
        private System.Windows.Forms.ComboBox countryComboBox;
        private System.Windows.Forms.ComboBox townComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox devNameTextBox;
        private System.Windows.Forms.RadioButton OOOradioButton;
        private System.Windows.Forms.RadioButton OAOradioButton;
        private System.Windows.Forms.RadioButton IEradioButton;
        private System.Windows.Forms.TextBox legalAddressTextBox;
        private System.Windows.Forms.TextBox INNtextBox;
        private System.Windows.Forms.TrackBar priceTrackBar;
        private System.Windows.Forms.DateTimePicker dateOfSaleTimePicker;
        private System.Windows.Forms.TextBox priceForMeterTextBox;
        private System.Windows.Forms.Button calculateButton;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.TextBox resultTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox districtTextBox;
        private System.Windows.Forms.TextBox streetTextBox;
        private System.Windows.Forms.TextBox frameTextBox;
    }
}

