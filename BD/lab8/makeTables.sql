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
/*lab8*/
/*1*/
CREATE VIEW [Клиенты_кратко] AS
SELECT 
    ID_Клиента AS [Код],
    Фамилия + ' ' + Имя AS [Имя клиента],
    Телефон AS [Контакт]
FROM Клиенты;
GO
/*2*/
CREATE VIEW [Количество_товаров_по_категориям] AS
SELECT 
    Название AS [Категория],
    COUNT(ID_Товара) AS [Количество товаров]
FROM Товары
GROUP BY Название;
GO
/*3*/
CREATE VIEW [Товары_высокий_спрос] 
    (Название, Цена, Единица_измерения)
AS
SELECT 
    Название,
    Цена,
    Единица_измерения
FROM Товары
WHERE Название LIKE '%Ноутбук%'; 
GO

-- Проверка:
SELECT * FROM [Товары_высокий_спрос];
/*4*/
CREATE VIEW [Товары_премиум] 
    (Название, Цена, Единица_измерения)
AS
SELECT 
    Название,
    Цена,
    Единица_измерения
FROM Товары
WHERE Цена > 10000
WITH CHECK OPTION; 
GO

SELECT * FROM [Товары_премиум];
INSERT INTO [Товары_премиум] VALUES ('Бюджетный товар', 500.00, 'шт');
/*5*/
CREATE VIEW [Товары_алфавит] 
    (Название, Цена, Единица_измерения)
AS
SELECT TOP 100 PERCENT
    Название,
    Цена,
    Единица_измерения
FROM Товары
ORDER BY Название;
GO
/*6*/
ALTER VIEW [Количество_товаров_по_категориям] WITH SCHEMABINDING
AS
SELECT 
    Единица_измерения AS [Категория],
    COUNT_BIG(*) AS [Количество товаров]
FROM dbo.Товары
GROUP BY Единица_измерения;
GO
ALTER TABLE Товары DROP COLUMN Единица_измерения;