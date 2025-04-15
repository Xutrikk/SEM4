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
/*UPDATE Клиенты
SET Признак_скидки = 1
WHERE ID_Клиента IN (
    SELECT ID_Клиента FROM Заказы
    GROUP BY ID_Клиента
    HAVING SUM(Общая_стоимость) > 50000
);

UPDATE Заказы
SET Общая_стоимость = Общая_стоимость * 0.98
WHERE ID_Клиента IN (
    SELECT ID_Клиента FROM Клиенты WHERE Признак_скидки = 1
);
ALTER TABLE Клиенты
ADD Дата_Регистрации DATE DEFAULT GETDATE();
GO

ALTER TABLE Товары
ADD CONSTRAINT CK_Цена CHECK (Цена >= 0);
ALTER TABLE Клиенты
ADD CONSTRAINT UQ_Телефон UNIQUE (Телефон);

ALTER TABLE Клиенты DROP CONSTRAINT DF__Клиенты__Дата_Ре__01142BA1;
ALTER TABLE Клиенты
DROP COLUMN Дата_Регистрации;
GO

SELECT * FROM Клиенты;
SELECT Фамилия, Телефон FROM Клиенты;
SELECT COUNT(*) AS Количество_товаров FROM Товары;
UPDATE Товары
SET Цена = 5000
WHERE Название = 'Мышь Razer';
SELECT * FROM Товары WHERE Название = 'Мышь Razer';*/
/*lab4*/
SELECT 
    Т.ID_Товара AS Код_товара,
    Т.Название AS Тип_товара
FROM Товары Т
INNER JOIN Заказы З ON Т.ID_Товара = З.ID_Товара;

SELECT 
    Т.ID_Товара AS Код_товара,
    Т.Название AS Тип_товара
FROM Товары Т
INNER JOIN Заказы З ON Т.ID_Товара = З.ID_Товара
WHERE Т.Название LIKE '%Ноутбук%';

SELECT 
    К.Фамилия + ' ' + К.Имя AS Имя_клиента,
    Т.Название AS Товар,
    CASE 
        WHEN З.Количество_заказанного = 1 THEN 'один'
        WHEN З.Количество_заказанного = 2 THEN 'два'
        ELSE CAST(З.Количество_заказанного AS NVARCHAR) + ' шт'
    END AS Количество_прописью
FROM Клиенты К
INNER JOIN Заказы З ON К.ID_Клиента = З.ID_Клиента
INNER JOIN Товары Т ON З.ID_Товара = Т.ID_Товара
ORDER BY З.Количество_заказанного DESC;


SELECT 
    К.Фамилия + ' ' + К.Имя AS Клиент,
    ISNULL(CAST(З.ID_Заказа AS NVARCHAR(20)), '***') AS Заказ
FROM Клиенты К
LEFT JOIN Заказы З ON К.ID_Клиента = З.ID_Клиента;


SELECT  
    Т.ID_Товара, 
    Т.Название AS Товар_без_заказов
FROM Товары Т
FULL OUTER JOIN Заказы З ON Т.ID_Товара = З.ID_Товара
WHERE З.ID_Товара IS NULL;

SELECT 
    З.ID_Заказа, 
    З.Количество_заказанного AS Заказ_без_товара
FROM Заказы З
FULL OUTER JOIN Товары Т ON З.ID_Товара = Т.ID_Товара
WHERE Т.ID_Товара IS NULL;

SELECT 
    Т.ID_Товара, 
    Т.Название AS Товар, 
    З.ID_Заказа, 
    З.Количество_заказанного
FROM 
Товары Т FULL OUTER JOIN Заказы З ON Т.ID_Товара = З.ID_Товара;

SELECT 
    З.ID_Заказа, 
    З.Количество_заказанного,
    Т.ID_Товара, 
    Т.Название AS Товар
FROM Заказы З
FULL OUTER JOIN Товары Т ON З.ID_Товара = Т.ID_Товара;


SELECT 
    Т.ID_Товара AS Код_товара,
    Т.Название AS Тип_товара
FROM Товары Т
CROSS JOIN Заказы З
WHERE Т.ID_Товара = З.ID_Товара;