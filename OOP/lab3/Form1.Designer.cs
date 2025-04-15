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
            this.serchButt = new System.Windows.Forms.Button();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.defaultRadioButton = new System.Windows.Forms.RadioButton();
            this.regexRadioButton = new System.Windows.Forms.RadioButton();
            this.orderComboBox = new System.Windows.Forms.ComboBox();
            this.saveButton1 = new System.Windows.Forms.Button();
            this.aboutProgramLabel = new System.Windows.Forms.Label();
            this.cleanButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.prevButton = new System.Windows.Forms.Button();
            this.searchButton1 = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.orderComboBox1 = new System.Windows.Forms.ComboBox();
            this.toggleVisibityButton = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
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
            this.label1.Location = new System.Drawing.Point(10, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Квартира";
            // 
            // footageUpDown
            // 
            this.footageUpDown.Location = new System.Drawing.Point(16, 242);
            this.footageUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.footageUpDown.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
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
            this.roomsNumComboBox.Location = new System.Drawing.Point(16, 271);
            this.roomsNumComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            "кухня",
            "ванна",
            "туалет",
            "подвал",
            "балкон "});
            this.apartmentOptionsListBox.Location = new System.Drawing.Point(16, 302);
            this.apartmentOptionsListBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.apartmentOptionsListBox.Name = "apartmentOptionsListBox";
            this.apartmentOptionsListBox.Size = new System.Drawing.Size(157, 72);
            this.apartmentOptionsListBox.TabIndex = 3;
            this.apartmentOptionsListBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // dateOfConstrTimePicker
            // 
            this.dateOfConstrTimePicker.Location = new System.Drawing.Point(16, 398);
            this.dateOfConstrTimePicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.materialComboBox.Location = new System.Drawing.Point(16, 427);
            this.materialComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.materialComboBox.Name = "materialComboBox";
            this.materialComboBox.Size = new System.Drawing.Size(157, 24);
            this.materialComboBox.TabIndex = 5;
            this.materialComboBox.Text = "Материал";
            this.materialComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // floorUpDown
            // 
            this.floorUpDown.Location = new System.Drawing.Point(16, 491);
            this.floorUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.floorUpDown.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.floorUpDown.Name = "floorUpDown";
            this.floorUpDown.Size = new System.Drawing.Size(157, 22);
            this.floorUpDown.TabIndex = 6;
            this.floorUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.banNon_int);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(220, 183);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 29);
            this.label2.TabIndex = 7;
            this.label2.Text = "Адрес";
            // 
            // houseUpDown
            // 
            this.houseUpDown.Location = new System.Drawing.Point(224, 370);
            this.houseUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.houseUpDown.Name = "houseUpDown";
            this.houseUpDown.Size = new System.Drawing.Size(161, 22);
            this.houseUpDown.TabIndex = 12;
            this.houseUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.banNon_int);
            // 
            // apartmentUpDown
            // 
            this.apartmentUpDown.Location = new System.Drawing.Point(222, 491);
            this.apartmentUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.countryComboBox.Location = new System.Drawing.Point(224, 212);
            this.countryComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.countryComboBox.Name = "countryComboBox";
            this.countryComboBox.Size = new System.Drawing.Size(161, 24);
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
            this.townComboBox.Location = new System.Drawing.Point(222, 242);
            this.townComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.townComboBox.Name = "townComboBox";
            this.townComboBox.Size = new System.Drawing.Size(161, 24);
            this.townComboBox.TabIndex = 19;
            this.townComboBox.Text = "Город";
            this.townComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(432, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 29);
            this.label3.TabIndex = 20;
            this.label3.Text = "Застройщик";
            // 
            // devNameTextBox
            // 
            this.devNameTextBox.Location = new System.Drawing.Point(438, 215);
            this.devNameTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.devNameTextBox.Name = "devNameTextBox";
            this.devNameTextBox.Size = new System.Drawing.Size(175, 22);
            this.devNameTextBox.TabIndex = 21;
            this.devNameTextBox.Text = "Название";
            // 
            // OOOradioButton
            // 
            this.OOOradioButton.AutoSize = true;
            this.OOOradioButton.Location = new System.Drawing.Point(438, 247);
            this.OOOradioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.OAOradioButton.Location = new System.Drawing.Point(562, 247);
            this.OAOradioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.IEradioButton.Location = new System.Drawing.Point(502, 247);
            this.IEradioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.IEradioButton.Name = "IEradioButton";
            this.IEradioButton.Size = new System.Drawing.Size(48, 20);
            this.IEradioButton.TabIndex = 24;
            this.IEradioButton.TabStop = true;
            this.IEradioButton.Text = "ИП";
            this.IEradioButton.UseVisualStyleBackColor = true;
            // 
            // legalAddressTextBox
            // 
            this.legalAddressTextBox.Location = new System.Drawing.Point(438, 294);
            this.legalAddressTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.legalAddressTextBox.Name = "legalAddressTextBox";
            this.legalAddressTextBox.Size = new System.Drawing.Size(175, 22);
            this.legalAddressTextBox.TabIndex = 25;
            this.legalAddressTextBox.Text = "Юрид. адрес";
            // 
            // INNtextBox
            // 
            this.INNtextBox.Location = new System.Drawing.Point(438, 342);
            this.INNtextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.INNtextBox.Name = "INNtextBox";
            this.INNtextBox.Size = new System.Drawing.Size(175, 22);
            this.INNtextBox.TabIndex = 26;
            this.INNtextBox.Text = "ИНН";
            // 
            // priceTrackBar
            // 
            this.priceTrackBar.Location = new System.Drawing.Point(438, 401);
            this.priceTrackBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.priceTrackBar.Maximum = 150;
            this.priceTrackBar.Name = "priceTrackBar";
            this.priceTrackBar.Size = new System.Drawing.Size(157, 56);
            this.priceTrackBar.TabIndex = 27;
            this.priceTrackBar.Scroll += new System.EventHandler(this.priceTrackBar_Scroll);
            this.priceTrackBar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // dateOfSaleTimePicker
            // 
            this.dateOfSaleTimePicker.Location = new System.Drawing.Point(438, 492);
            this.dateOfSaleTimePicker.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateOfSaleTimePicker.Name = "dateOfSaleTimePicker";
            this.dateOfSaleTimePicker.Size = new System.Drawing.Size(157, 22);
            this.dateOfSaleTimePicker.TabIndex = 28;
            this.dateOfSaleTimePicker.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // priceForMeterTextBox
            // 
            this.priceForMeterTextBox.Location = new System.Drawing.Point(438, 455);
            this.priceForMeterTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.priceForMeterTextBox.Name = "priceForMeterTextBox";
            this.priceForMeterTextBox.Size = new System.Drawing.Size(157, 22);
            this.priceForMeterTextBox.TabIndex = 29;
            this.priceForMeterTextBox.Text = "Цена за кв.м $";
            this.priceForMeterTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.banNon_floats);
            // 
            // calculateButton
            // 
            this.calculateButton.Location = new System.Drawing.Point(437, 547);
            this.calculateButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.calculateButton.Name = "calculateButton";
            this.calculateButton.Size = new System.Drawing.Size(157, 46);
            this.calculateButton.TabIndex = 30;
            this.calculateButton.Text = "Рассчитать расходы";
            this.calculateButton.UseVisualStyleBackColor = true;
            this.calculateButton.Click += new System.EventHandler(this.calculateButton_Click);
            // 
            // resultLabel
            // 
            this.resultLabel.AutoSize = true;
            this.resultLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.resultLabel.Location = new System.Drawing.Point(189, 97);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(249, 29);
            this.resultLabel.TabIndex = 31;
            this.resultLabel.Text = "Итоговый результат";
            this.resultLabel.Click += new System.EventHandler(this.resultLabel_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(16, 547);
            this.saveButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(157, 46);
            this.saveButton.TabIndex = 34;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(222, 547);
            this.loadButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(157, 46);
            this.loadButton.TabIndex = 35;
            this.loadButton.Text = "Загрузить";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // resultTextBox
            // 
            this.resultTextBox.Location = new System.Drawing.Point(161, 145);
            this.resultTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.resultTextBox.Name = "resultTextBox";
            this.resultTextBox.Size = new System.Drawing.Size(323, 22);
            this.resultTextBox.TabIndex = 36;
            this.resultTextBox.TextChanged += new System.EventHandler(this.resultTextBox_TextChanged);
            this.resultTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(12, 216);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 20);
            this.label6.TabIndex = 37;
            this.label6.Text = "Метраж";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(12, 468);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 20);
            this.label7.TabIndex = 38;
            this.label7.Text = "Этаж";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(222, 347);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 20);
            this.label8.TabIndex = 39;
            this.label8.Text = "Дом";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(218, 468);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 20);
            this.label9.TabIndex = 40;
            this.label9.Text = "Номер кв.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(420, 377);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(192, 20);
            this.label10.TabIndex = 41;
            this.label10.Text = "Слайдер стоимости $";
            // 
            // districtTextBox
            // 
            this.districtTextBox.Location = new System.Drawing.Point(224, 272);
            this.districtTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.districtTextBox.Name = "districtTextBox";
            this.districtTextBox.Size = new System.Drawing.Size(160, 22);
            this.districtTextBox.TabIndex = 42;
            this.districtTextBox.Text = "Район";
            // 
            // streetTextBox
            // 
            this.streetTextBox.Location = new System.Drawing.Point(222, 302);
            this.streetTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.streetTextBox.Name = "streetTextBox";
            this.streetTextBox.Size = new System.Drawing.Size(160, 22);
            this.streetTextBox.TabIndex = 43;
            this.streetTextBox.Text = "Улица";
            // 
            // frameTextBox
            // 
            this.frameTextBox.Location = new System.Drawing.Point(222, 429);
            this.frameTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.frameTextBox.Name = "frameTextBox";
            this.frameTextBox.Size = new System.Drawing.Size(160, 22);
            this.frameTextBox.TabIndex = 44;
            this.frameTextBox.Text = "Корпус";
            // 
            // serchButt
            // 
            this.serchButt.Location = new System.Drawing.Point(872, 190);
            this.serchButt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.serchButt.Name = "serchButt";
            this.serchButt.Size = new System.Drawing.Size(155, 53);
            this.serchButt.TabIndex = 45;
            this.serchButt.Text = "Искать";
            this.serchButt.UseVisualStyleBackColor = true;
            this.serchButt.Click += new System.EventHandler(this.searchButt_Click);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(645, 204);
            this.searchTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(219, 22);
            this.searchTextBox.TabIndex = 46;
            this.searchTextBox.Text = "Ввведите поисковый запрос";
            // 
            // defaultRadioButton
            // 
            this.defaultRadioButton.AutoSize = true;
            this.defaultRadioButton.Location = new System.Drawing.Point(755, 234);
            this.defaultRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.defaultRadioButton.Name = "defaultRadioButton";
            this.defaultRadioButton.Size = new System.Drawing.Size(88, 20);
            this.defaultRadioButton.TabIndex = 47;
            this.defaultRadioButton.TabStop = true;
            this.defaultRadioButton.Text = "Обычный";
            this.defaultRadioButton.UseVisualStyleBackColor = true;
            // 
            // regexRadioButton
            // 
            this.regexRadioButton.AutoSize = true;
            this.regexRadioButton.Location = new System.Drawing.Point(645, 234);
            this.regexRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.regexRadioButton.Name = "regexRadioButton";
            this.regexRadioButton.Size = new System.Drawing.Size(97, 20);
            this.regexRadioButton.TabIndex = 48;
            this.regexRadioButton.TabStop = true;
            this.regexRadioButton.Text = "Регулярки";
            this.regexRadioButton.UseVisualStyleBackColor = true;
            // 
            // orderComboBox
            // 
            this.orderComboBox.FormattingEnabled = true;
            this.orderComboBox.Items.AddRange(new object[] {
            "Возрастанию",
            "Убыванию",
            "Без"});
            this.orderComboBox.Location = new System.Drawing.Point(645, 268);
            this.orderComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.orderComboBox.Name = "orderComboBox";
            this.orderComboBox.Size = new System.Drawing.Size(161, 24);
            this.orderComboBox.TabIndex = 49;
            this.orderComboBox.Text = "Сортировать по";
            this.orderComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            this.orderComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // saveButton1
            // 
            this.saveButton1.Location = new System.Drawing.Point(872, 257);
            this.saveButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.saveButton1.Name = "saveButton1";
            this.saveButton1.Size = new System.Drawing.Size(157, 46);
            this.saveButton1.TabIndex = 50;
            this.saveButton1.Text = "Сохранить";
            this.saveButton1.UseVisualStyleBackColor = true;
            this.saveButton1.Click += new System.EventHandler(this.saveButton1_Click);
            // 
            // aboutProgramLabel
            // 
            this.aboutProgramLabel.AutoSize = true;
            this.aboutProgramLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.aboutProgramLabel.Location = new System.Drawing.Point(1044, 22);
            this.aboutProgramLabel.Name = "aboutProgramLabel";
            this.aboutProgramLabel.Size = new System.Drawing.Size(169, 29);
            this.aboutProgramLabel.TabIndex = 51;
            this.aboutProgramLabel.Text = "О программе";
            this.aboutProgramLabel.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // cleanButton
            // 
            this.cleanButton.Location = new System.Drawing.Point(353, 15);
            this.cleanButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cleanButton.Name = "cleanButton";
            this.cleanButton.Size = new System.Drawing.Size(155, 53);
            this.cleanButton.TabIndex = 52;
            this.cleanButton.Text = "Очистить";
            this.cleanButton.UseVisualStyleBackColor = true;
            this.cleanButton.Click += new System.EventHandler(this.cleanButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(184, 15);
            this.nextButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(155, 53);
            this.nextButton.TabIndex = 53;
            this.nextButton.Text = "Вперед";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // prevButton
            // 
            this.prevButton.Location = new System.Drawing.Point(15, 15);
            this.prevButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.prevButton.Name = "prevButton";
            this.prevButton.Size = new System.Drawing.Size(155, 53);
            this.prevButton.TabIndex = 54;
            this.prevButton.Text = "Назад";
            this.prevButton.UseVisualStyleBackColor = true;
            this.prevButton.Click += new System.EventHandler(this.prevButton_Click);
            // 
            // searchButton1
            // 
            this.searchButton1.Location = new System.Drawing.Point(679, 15);
            this.searchButton1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.searchButton1.Name = "searchButton1";
            this.searchButton1.Size = new System.Drawing.Size(155, 53);
            this.searchButton1.TabIndex = 56;
            this.searchButton1.Text = "Искать";
            this.searchButton1.UseVisualStyleBackColor = true;
            this.searchButton1.Click += new System.EventHandler(this.searchButt_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(516, 15);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(155, 53);
            this.deleteButton.TabIndex = 57;
            this.deleteButton.Text = "Удалить";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // orderComboBox1
            // 
            this.orderComboBox1.FormattingEnabled = true;
            this.orderComboBox1.Items.AddRange(new object[] {
            "Возрастанию",
            "Убыванию",
            "Без"});
            this.orderComboBox1.Location = new System.Drawing.Point(679, 74);
            this.orderComboBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.orderComboBox1.Name = "orderComboBox1";
            this.orderComboBox1.Size = new System.Drawing.Size(155, 24);
            this.orderComboBox1.TabIndex = 58;
            this.orderComboBox1.Text = "Сортировать по";
            this.orderComboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox2_SelectedIndexChanged);
            this.orderComboBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.prohibitTextChanges);
            // 
            // toggleVisibityButton
            // 
            this.toggleVisibityButton.Location = new System.Drawing.Point(852, 15);
            this.toggleVisibityButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.toggleVisibityButton.Name = "toggleVisibityButton";
            this.toggleVisibityButton.Size = new System.Drawing.Size(155, 53);
            this.toggleVisibityButton.TabIndex = 59;
            this.toggleVisibityButton.Text = "Скрыть элементы";
            this.toggleVisibityButton.UseVisualStyleBackColor = true;
            this.toggleVisibityButton.Click += new System.EventHandler(this.toggleVisibityButton_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.infoLabel.Location = new System.Drawing.Point(639, 357);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(80, 29);
            this.infoLabel.TabIndex = 60;
            this.infoLabel.Text = "Инфа";
            this.infoLabel.Click += new System.EventHandler(this.analyzeButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1352, 683);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.toggleVisibityButton);
            this.Controls.Add(this.orderComboBox1);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.searchButton1);
            this.Controls.Add(this.prevButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.cleanButton);
            this.Controls.Add(this.aboutProgramLabel);
            this.Controls.Add(this.saveButton1);
            this.Controls.Add(this.orderComboBox);
            this.Controls.Add(this.regexRadioButton);
            this.Controls.Add(this.defaultRadioButton);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.serchButt);
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
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
        private System.Windows.Forms.Button serchButt;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.RadioButton defaultRadioButton;
        private System.Windows.Forms.RadioButton regexRadioButton;
        private System.Windows.Forms.ComboBox orderComboBox;
        private System.Windows.Forms.Button saveButton1;
        private System.Windows.Forms.Label aboutProgramLabel;
        private System.Windows.Forms.Button cleanButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button prevButton;
        private System.Windows.Forms.Button searchButton1;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.ComboBox orderComboBox1;
        private System.Windows.Forms.Button toggleVisibityButton;
        private System.Windows.Forms.Label infoLabel;
    }
}

