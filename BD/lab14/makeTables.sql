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
/*lab14*/
/*Задание 1*/
USE [HUT-MyBase];
GO

-- Создание скалярной функции
CREATE FUNCTION Zakazy(@ID_Клиента INT)
RETURNS INT
AS
BEGIN
    DECLARE @rc INT = 0;
    SELECT @rc = SUM(Количество_заказанного)
    FROM Заказы
    WHERE ID_Клиента = @ID_Клиента;
    RETURN ISNULL(@rc, 0);
END;
GO

SELECT dbo.Zakazy(1) AS Количество_заказов; 
SELECT dbo.Zakazy(2) AS Количество_заказов; 
GO
/*Задание 2*/
USE [HUT-MyBase];
GO

-- Создание скалярной функции
CREATE FUNCTION FSUBJECTS(@P INT) -- @P как ID_Товара
RETURNS INT
AS
BEGIN
    DECLARE @rc INT = 0;
    SELECT @rc = COUNT(DISTINCT ID_Клиента)
    FROM Заказы
    WHERE ID_Товара = @P
    AND EXISTS (SELECT 1 FROM Заказы z WHERE z.ID_Товара = @P);
    RETURN ISNULL(@rc, 0);
END;
GO

SELECT dbo.FSUBJECTS(1) AS Количество_уникальных_клиентов; 
SELECT dbo.FSUBJECTS(2) AS Количество_уникальных_клиентов; 
GO
/*Задание 3*/
USE [HUT-MyBase];
GO

-- Создание табличной функции
CREATE FUNCTION FPUCena(@P VARCHAR(50)) -- @P как фильтр по названию товара
RETURNS TABLE
AS
RETURN
(
    SELECT t.Название, t.Количество_на_складе, t.Цена, t.Единица_измерения,
           COALESCE(SUM(z.Количество_заказанного), 0) AS Общее_количество_заказов
    FROM Товары t
    LEFT JOIN Заказы z ON t.ID_Товара = z.ID_Товара
    WHERE t.Название LIKE '%' + @P + '%'
    GROUP BY t.Название, t.Количество_на_складе, t.Цена, t.Единица_измерения
);
GO

SELECT * FROM dbo.FPUCena('Lenovo');
SELECT * FROM dbo.FPUCena('Razer');  
GO
/*Задание 4*/
USE [HUT-MyBase];
GO

IF OBJECT_ID('FKolTov', 'FN') IS NOT NULL
    DROP FUNCTION FKolTov;
GO

CREATE FUNCTION FKolTov (@p VARCHAR(50))
RETURNS INT
AS
BEGIN
    DECLARE @rc INT = (SELECT COUNT(*) FROM Заказы
                       WHERE ID_Товара IN (SELECT ID_Товара FROM Товары WHERE Название LIKE '%' + @p + '%'));
    RETURN ISNULL(@rc, 0);
END;
GO

SELECT dbo.FKolTov('Lenovo') AS Количество_заказов;
SELECT dbo.FKolTov('Razer') AS Количество_заказов;
SELECT dbo.FKolTov('Logitech') AS Количество_заказов;
GO