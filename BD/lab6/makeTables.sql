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
/*lab6*/
/*1*/
SELECT 
    t.Название AS Наименование_товара,
    MAX(z.Количество_заказанного) AS Максимальное_количество,
    MIN(z.Количество_заказанного) AS Минимальное_количество,
    AVG(z.Количество_заказанного) AS Среднее_количество,
    SUM(z.Количество_заказанного) AS Суммарное_количество,
    COUNT(*) AS Количество_заказов
FROM 
    Товары t
INNER JOIN 
    Заказы z ON t.ID_Товара = z.ID_Товара
GROUP BY 
    t.Название;
	/*2*/
	SELECT * FROM (
    SELECT 
        CASE 
            WHEN Количество_заказанного BETWEEN 1 AND 5 THEN '1-5'
            WHEN Количество_заказанного BETWEEN 6 AND 10 THEN '6-10'
            ELSE 'Более 10'
        END AS Интервал_количества,
        COUNT(*) AS Количество_заказов
    FROM 
        Заказы
    GROUP BY 
        CASE 
            WHEN Количество_заказанного BETWEEN 1 AND 5 THEN '1-5'
            WHEN Количество_заказанного BETWEEN 6 AND 10 THEN '6-10'
            ELSE 'Более 10'
        END
) AS T
ORDER BY 
    CASE Интервал_количества
        WHEN 'Более 10' THEN 1
        WHEN '6-10' THEN 2
        WHEN '1-5' THEN 3
        ELSE 4
    END;
	/*3*/
SELECT 
    k.Фамилия,
    k.Имя,
    CAST(ROUND(AVG(t.Цена), 2) AS DECIMAL(10,2)) AS Средняя_цена
FROM 
    Клиенты k
INNER JOIN 
    Заказы z ON k.ID_Клиента = z.ID_Клиента
INNER JOIN 
    Товары t ON z.ID_Товара = t.ID_Товара
GROUP BY 
    k.Фамилия, k.Имя
ORDER BY 
    Средняя_цена DESC;

/*4*/
SELECT 
    k.Фамилия,
    k.Имя,
    CAST(ROUND(AVG(t.Цена), 2) AS DECIMAL(10,2)) AS Средняя_цена
FROM 
    Клиенты k
INNER JOIN 
    Заказы z ON k.ID_Клиента = z.ID_Клиента
INNER JOIN 
    Товары t ON z.ID_Товара = t.ID_Товара
WHERE 
    t.ID_Товара IN (1, 2)
GROUP BY 
    k.Фамилия, k.Имя
ORDER BY 
    Средняя_цена DESC;

/*5*/
SELECT 
    t.Название AS Товар,
    k.Адрес,
    CAST(ROUND(AVG(t.Цена), 2) AS DECIMAL(10,2)) AS Средняя_цена
FROM 
    Товары t
INNER JOIN 
    Заказы z ON t.ID_Товара = z.ID_Товара
INNER JOIN 
    Клиенты k ON z.ID_Клиента = k.ID_Клиента
WHERE 
    k.Адрес LIKE '%Москва%' 
GROUP BY 
    t.Название, k.Адрес;

	/*6*/
	SELECT 
    t.Название AS Товар,
    COUNT(*) AS Количество_заказов
FROM 
    Товары t
INNER JOIN 
    Заказы z ON t.ID_Товара = z.ID_Товара
WHERE 
    z.Количество_заказанного IN (1, 2) 
GROUP BY 
    t.Название
HAVING 
    COUNT(*) > 0
ORDER BY 
    Количество_заказов DESC;