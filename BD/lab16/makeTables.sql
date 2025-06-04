CREATE DATABASE [HUT-MyBase];
CREATE TABLE Товары (
    ID_Товара INT IDENTITY PRIMARY KEY, 
    Название NVARCHAR(255) NOT NULL, 
    Количество_на_складе INT NOT NULL, 
    Цена DECIMAL(10,2) NOT NULL, 
    Единица_измерения NVARCHAR(50) NOT NULL 
);
GO


CREATE TABLE Клиенты (
    ID_Клиента INT IDENTITY PRIMARY KEY, 
    Фамилия NVARCHAR(100) NOT NULL, 
    Имя NVARCHAR(100) NOT NULL, 
    Отчество NVARCHAR(100) NULL, 
    Адрес NVARCHAR(255) NOT NULL, 
    Телефон NVARCHAR(20) NOT NULL, 
    Email NVARCHAR(100) NOT NULL UNIQUE, 
    Признак_скидки BIT DEFAULT 0 -- 
);
GO


CREATE TABLE Заказы (
    ID_Заказа INT IDENTITY PRIMARY KEY, 
    ID_Клиента INT NOT NULL, 
    ID_Товара INT NOT NULL, 
    Количество_заказанного INT NOT NULL CHECK (Количество_заказанного > 0), 
    Дата_продажи DATE NOT NULL,  
    FOREIGN KEY (ID_Клиента) REFERENCES Клиенты(ID_Клиента),
    FOREIGN KEY (ID_Товара) REFERENCES Товары(ID_Товара)
);
GO
INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
VALUES 
('Ноутбук Lenovo', 10, 75000.00, 'шт'),
('Клавиатура Logitech', 50, 3500.00, 'шт'),
('Мышь Razer', 30, 5500.00, 'шт');

INSERT INTO Клиенты (Фамилия, Имя, Отчество, Адрес, Телефон, Email)
VALUES 
('Иванов', 'Алексей', 'Петрович', 'Москва, ул. Ленина, 10', '+79001112233', 'ivanov@mail.ru'),
('Петров', 'Игорь', 'Александрович', 'Санкт-Петербург, Невский пр., 25', '+79112223344', 'petrov@mail.ru');

INSERT INTO Заказы (ID_Клиента, ID_Товара, Количество_заказанного, Дата_продажи)
VALUES 
(1, 1, 1, '2025-02-18'),
(2, 2, 2, '2025-02-18'),
(2, 3, 1, '2025-02-18');
/*lab16*/
/*Задание 1*/
UPDATE Клиенты SET Признак_скидки = 1 WHERE ID_Клиента = 1;

-- Повторный запрос
SELECT 
    c.ID_Клиента "@ID",
    c.Фамилия,
    c.Имя,
    c.Отчество,
    c.Адрес,
    c.Телефон,
    c.Email
FROM Клиенты c
WHERE c.Признак_скидки = 1
FOR XML PATH('Клиент'), ROOT('Клиенты');

/*Задание 2*/
SELECT 
    t.Название AS "Аудитория",
    t.Единица_измерения AS "Тип_аудитории",
    t.Количество_на_складе AS "Вместимость"
FROM Товары t
WHERE t.Единица_измерения = 'шт'
FOR XML AUTO, ROOT('Аудитории');
/*Задание 3*/
DECLARE @hdoc INT;
DECLARE @xml XML = '
<Заказы>
  <Заказ>
    <ID_Клиента>1</ID_Клиента>
    <ID_Товара>1</ID_Товара>
    <Количество_заказанного>2</Количество_заказанного>
    <Дата_продажи>2025-05-28</Дата_продажи>
  </Заказ>
  <Заказ>
    <ID_Клиента>2</ID_Клиента>
    <ID_Товара>2</ID_Товара>
    <Количество_заказанного>1</Количество_заказанного>
    <Дата_продажи>2025-05-28</Дата_продажи>
  </Заказ>
  <Заказ>
    <ID_Клиента>1</ID_Клиента>
    <ID_Товара>3</ID_Товара>
    <Количество_заказанного>3</Количество_заказанного>
    <Дата_продажи>2025-05-28</Дата_продажи>
  </Заказ>
</Заказы>';

EXEC sp_xml_preparedocument @hdoc OUTPUT, @xml;

INSERT INTO Заказы (ID_Клиента, ID_Товара, Количество_заказанного, Дата_продажи)
SELECT 
    ID_Клиента,
    ID_Товара,
    Количество_заказанного,
    Дата_продажи
FROM OPENXML(@hdoc, '/Заказы/Заказ', 2)
WITH (
    ID_Клиента INT,
    ID_Товара INT,
    Количество_заказанного INT,
    Дата_продажи DATE
);

EXEC sp_xml_removedocument @hdoc;


	/*Задание 4*/
-- Удаляем столбец INFO, если он уже существует, чтобы начать с чистого листа
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
           WHERE TABLE_NAME = 'Клиенты' AND COLUMN_NAME = 'INFO')
BEGIN
    ALTER TABLE Клиенты DROP COLUMN INFO;
END

-- Удаляем схему КлиентыSchema, если она уже существует
IF EXISTS (SELECT * FROM sys.xml_schema_collections WHERE name = 'КлиентыSchema')
BEGIN
    DROP XML SCHEMA COLLECTION КлиентыSchema;
END

-- Создаем схему КлиентыSchema
CREATE XML SCHEMA COLLECTION КлиентыSchema AS '
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="ПаспортныеДанные">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="Серия_и_номер" type="xs:string" minOccurs="1" maxOccurs="1"/>
        <xs:element name="Личный_номер" type="xs:int" minOccurs="1" maxOccurs="1"/>
        <xs:element name="Дата_выдачи" type="xs:date" minOccurs="1" maxOccurs="1"/>
        <xs:element name="Адрес_прописки" type="xs:string" minOccurs="1" maxOccurs="1"/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>';

-- Добавляем столбец INFO с типизацией через КлиентыSchema
ALTER TABLE Клиенты
ADD INFO XML (КлиентыSchema);

-- INSERT: Добавляем нового клиента с паспортными данными в XML
DECLARE @xml XML (КлиентыSchema) = '
<ПаспортныеДанные>
  <Серия_и_номер>1234 567890</Серия_и_номер>
  <Личный_номер>3</Личный_номер>
  <Дата_выдачи>2020-01-15</Дата_выдачи>
  <Адрес_прописки>Казань, ул. Победы, 5</Адрес_прописки>
</ПаспортныеДанные>';

INSERT INTO Клиенты (Фамилия, Имя, Отчество, Адрес, Телефон, Email, INFO)
VALUES ('Сидоров', 'Василий', 'Иванович', 'Казань, ул. Победы, 5', '+79223334455', 'sidorov@mail.ru', @xml);

-- UPDATE: Изменяем дату выдачи
UPDATE Клиенты
SET INFO.modify('
    replace value of (/ПаспортныеДанные/Дата_выдачи)[1]
    with xs:date("2021-01-15")
')
WHERE ID_Клиента = (SELECT MAX(ID_Клиента) FROM Клиенты);

-- SELECT: Извлекаем данные с помощью методов query() и value()
SELECT 
    c.ID_Клиента,
    c.Фамилия,
    c.Имя,
    c.INFO.query('/ПаспортныеДанные') AS Паспортные_данные,
    c.INFO.value('(/ПаспортныеДанные/Серия_и_номер)[1]', 'NVARCHAR(20)') AS Серия_и_номер,
    c.INFO.value('(/ПаспортныеДанные/Личный_номер)[1]', 'INT') AS Личный_номер,
    c.INFO.value('(/ПаспортныеДанные/Дата_выдачи)[1]', 'DATE') AS Дата_выдачи,
    c.INFO.value('(/ПаспортныеДанные/Адрес_прописки)[1]', 'NVARCHAR(255)') AS Адрес_прописки
FROM Клиенты c;
	/*Задание 5*/

-- Удаляем старую схему КлиентыSchema, если она существует (чтобы заменить на новую)
IF EXISTS (SELECT * FROM sys.xml_schema_collections WHERE name = 'КлиентыSchema')
BEGIN
    -- Сначала снимаем типизацию с столбца INFO
    ALTER TABLE Клиенты
    ALTER COLUMN INFO XML;

    -- Удаляем схему
    DROP XML SCHEMA COLLECTION КлиентыSchema;
END


CREATE XML SCHEMA COLLECTION КлиентыPassportSchema AS '
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="ПаспортныеДанные">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="Серия_и_номер" type="xs:string" minOccurs="1" maxOccurs="1"/>
        <xs:element name="Личный_номер" type="xs:int" minOccurs="1" maxOccurs="1"/>
        <xs:element name="Дата_выдачи" type="xs:date" minOccurs="1" maxOccurs="1"/>
        <xs:element name="Адрес_прописки" type="xs:string" minOccurs="1" maxOccurs="1"/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>';

-- Очищаем данные в столбце INFO, чтобы избежать конфликтов при применении новой схемы
UPDATE Клиенты
SET INFO = NULL;

-- Применяем новую схему к столбцу INFO
ALTER TABLE Клиенты
ALTER COLUMN INFO XML (КлиентыPassportSchema);

-- Корректный INSERT
INSERT INTO Клиенты (Фамилия, Имя, Адрес, Телефон, Email, INFO)
VALUES ('Петров2', 'Игорь2', 'СПб, пр. Невский, 30', '+79114445566', 'petrov2@mail.ru',
        '<ПаспортныеДанные>
           <Серия_и_номер>4321 098765</Серия_и_номер>
           <Личный_номер>4</Личный_номер>
           <Дата_выдачи>2022-03-10</Дата_выдачи>
           <Адрес_прописки>СПб, пр. Невский, 30</Адрес_прописки>
         </ПаспортныеДанные>');

-- INSERT с ошибкой (отсутствует обязательный элемент Личный_номер)
BEGIN TRY
    INSERT INTO Клиенты (Фамилия, Имя, Адрес, Телефон, Email, INFO)
    VALUES ('Ошибка', 'Тест', 'Москва', '+79998887766', 'error@mail.ru',
            '<ПаспортныеДанные>
               <Серия_и_номер>1111 222222</Серия_и_номер>
               <Дата_выдачи>2022-03-10</Дата_выдачи>
               <Адрес_прописки>Москва</Адрес_прописки>
             </ПаспортныеДанные>');
END TRY
BEGIN CATCH
    PRINT 'Ошибка при INSERT: ' + ERROR_MESSAGE();
END CATCH;

-- Корректный UPDATE
UPDATE Клиенты
SET INFO.modify('
    replace value of (/ПаспортныеДанные/Дата_выдачи)[1]
    with xs:date("2023-03-10")
')
WHERE ID_Клиента = (SELECT MAX(ID_Клиента) FROM Клиенты WHERE INFO IS NOT NULL);

-- UPDATE с ошибкой (несуществующий элемент)
BEGIN TRY
    UPDATE Клиенты
    SET INFO.modify('
        replace value of (/ПаспортныеДанные/Неверный_элемент)[1]
        with xs:date("2023-03-10")
    ')
    WHERE ID_Клиента = (SELECT MAX(ID_Клиента) FROM Клиенты WHERE INFO IS NOT NULL);
END TRY
BEGIN CATCH
    PRINT 'Ошибка при UPDATE: ' + ERROR_MESSAGE();
END CATCH;

-- Создаём новую схему ДополнительнаяКлиентыSchema
IF EXISTS (SELECT * FROM sys.xml_schema_collections WHERE name = 'ДополнительнаяКлиентыSchema')
BEGIN
    DROP XML SCHEMA COLLECTION ДополнительнаяКлиентыSchema;
END

CREATE XML SCHEMA COLLECTION ДополнительнаяКлиентыSchema AS '
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="КлиентДанные">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="ФИО" type="xs:string"/>
        <xs:element name="Телефон" type="xs:string"/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>';

-- Добавляем новый столбец ДопИнфо с новой схемой
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
           WHERE TABLE_NAME = 'Клиенты' AND COLUMN_NAME = 'ДопИнфо')
BEGIN
    ALTER TABLE Клиенты DROP COLUMN ДопИнфо;
END

ALTER TABLE Клиенты
ADD ДопИнфо XML (ДополнительнаяКлиентыSchema);

-- Вставляем данные в новый столбец
UPDATE Клиенты
SET ДопИнфо = '
    <КлиентДанные>
      <ФИО>Петров2 Игорь2</ФИО>
      <Телефон>+79114445566</Телефон>
    </КлиентДанные>'
WHERE ID_Клиента = (SELECT MAX(ID_Клиента) FROM Клиенты);
