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
    Общая_стоимость DECIMAL(10,2) NOT NULL, 
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

INSERT INTO Заказы (ID_Клиента, ID_Товара, Количество_заказанного, Дата_продажи, Общая_стоимость)
VALUES 
(1, 1, 1, '2025-02-18', 75000.00),
(2, 2, 2, '2025-02-18', 7000.00),
(2, 3, 1, '2025-02-18', 5500.00);
UPDATE Клиенты
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
SELECT * FROM Товары WHERE Название = 'Мышь Razer';
