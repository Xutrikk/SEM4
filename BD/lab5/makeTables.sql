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
/*lab5*/
SELECT 
    Т.Название AS Товар, 
    З.Дата_продажи AS Дата_продажи, 
    Т.Цена AS Цена
FROM Товары Т, Заказы З
WHERE 
    Т.ID_Товара = З.ID_Товара 
    AND 
    З.ID_Клиента IN (
        SELECT К.ID_Клиента 
        FROM Клиенты К 
        WHERE К.Адрес LIKE '%Москва%'
    );

	SELECT 
    Т.Название AS Товар, 
    З.Дата_продажи AS Дата_продажи, 
    Т.Цена AS Цена
FROM Товары Т
INNER JOIN Заказы З ON Т.ID_Товара = З.ID_Товара
WHERE 
    З.ID_Клиента IN (
        SELECT К.ID_Клиента 
        FROM Клиенты К 
        WHERE К.Адрес LIKE '%Москва%'
    );

	SELECT 
    Т.Название AS Товар, 
    З.Дата_продажи AS Дата_продажи, 
    Т.Цена AS Цена
FROM Товары Т
INNER JOIN Заказы З ON Т.ID_Товара = З.ID_Товара
INNER JOIN Клиенты К ON З.ID_Клиента = К.ID_Клиента
WHERE 
    К.Адрес LIKE '%Москва%';

	SELECT 
    Т1.Название, 
    Т1.Цена, 
    Т1.Единица_измерения
FROM Товары Т1
WHERE Т1.Цена = (
    SELECT TOP 1 Т2.Цена 
    FROM Товары Т2 
    WHERE Т2.Единица_измерения = Т1.Единица_измерения 
    ORDER BY Т2.Цена DESC
)
ORDER BY Т1.Цена DESC;

SELECT 
    К.Фамилия + ' ' + К.Имя AS Клиент
FROM Клиенты К
WHERE NOT EXISTS (
    SELECT 1 
    FROM Заказы З 
    WHERE З.ID_Клиента = К.ID_Клиента
);

SELECT 
    (SELECT AVG(Цена) FROM Товары WHERE Название LIKE '%Ноутбук%') AS Средняя_цена_ноутбуков,
    (SELECT AVG(Цена) FROM Товары WHERE Название LIKE '%Клавиатура%') AS Средняя_цена_клавиатур,
    (SELECT AVG(Цена) FROM Товары WHERE Название LIKE '%Мышь%') AS Средняя_цена_мышей;

	SELECT 
    Название, 
    Цена
FROM Товары
WHERE Цена >= ALL (
    SELECT Цена 
    FROM Товары 
    WHERE Название LIKE 'К%'
);

SELECT 
    Название, 
    Цена
FROM Товары
WHERE Цена > ANY (
    SELECT Цена 
    FROM Товары 
    WHERE Название LIKE 'М%'
);