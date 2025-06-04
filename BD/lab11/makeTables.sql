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
SELECT *FROM Заказы
/*lab11*/
-- Задание 1
DECLARE @Name NVARCHAR(255), @List NVARCHAR(MAX) = '';

DECLARE TovarCursor CURSOR LOCAL
FOR SELECT RTRIM(Название) FROM Товары GROUP BY Название; 

OPEN TovarCursor;

FETCH TovarCursor INTO @Name;

WHILE @@FETCH_STATUS = 0
BEGIN
    IF @List = ''
        SET @List = @Name;
    ELSE
        SET @List = @List + ', ' + @Name;
    FETCH TovarCursor INTO @Name;
END;

PRINT 'Список товаров: ' + @List;

CLOSE TovarCursor;
DEALLOCATE TovarCursor;
GO
-- Задание 2
 -- Глобальный курсор
DECLARE @tv NVARCHAR(255), @cena DECIMAL(10,2);

DECLARE TovarCursorGlobal CURSOR GLOBAL
FOR SELECT Название, Цена FROM Товары;

OPEN TovarCursorGlobal;
FETCH TovarCursorGlobal INTO @tv, @cena;

PRINT 'Глобальный курсор (первая выборка): ' + @tv + ' - ' + CAST(@cena AS VARCHAR(10));

-- Локальный курсор
DECLARE @tv2 NVARCHAR(255), @cena2 DECIMAL(10,2);

DECLARE TovarCursorLocal CURSOR LOCAL
FOR SELECT Название, Цена FROM Товары;

OPEN TovarCursorLocal;
FETCH TovarCursorLocal INTO @tv2, @cena2;

PRINT 'Локальный курсор (первая выборка): ' + @tv2 + ' - ' + CAST(@cena2 AS VARCHAR(10));

CLOSE TovarCursorLocal;
DEALLOCATE TovarCursorLocal;

-- Проверка доступности глобального курсора в новом блоке
FETCH TovarCursorGlobal INTO @tv, @cena;
PRINT 'Глобальный курсор (вторая выборка в новом блоке): ' + @tv + ' - ' + CAST(@cena AS VARCHAR(10));

CLOSE TovarCursorGlobal;
DEALLOCATE TovarCursorGlobal;
GO
-- Задание 3
-- Статический курсор
DECLARE @tv NVARCHAR(255), @cena DECIMAL(10,2);

DECLARE TovarCursorStatic CURSOR STATIC
FOR SELECT Название, Цена FROM Товары;

OPEN TovarCursorStatic;
FETCH TovarCursorStatic INTO @tv, @cena;

PRINT 'Статический курсор (до изменения): ' + @tv + ' - ' + CAST(@cena AS VARCHAR(10));

-- Изменяем данные
UPDATE Товары SET Цена = Цена + 1000 WHERE Название = 'Ноутбук Lenovo';

FETCH TovarCursorStatic INTO @tv, @cena;
PRINT 'Статический курсор (после изменения): ' + @tv + ' - ' + CAST(@cena AS VARCHAR(10));

CLOSE TovarCursorStatic;
DEALLOCATE TovarCursorStatic;

-- Динамический курсор
DECLARE @tv2 NVARCHAR(255), @cena2 DECIMAL(10,2);

DECLARE TovarCursorDynamic CURSOR DYNAMIC
FOR SELECT Название, Цена FROM Товары;

OPEN TovarCursorDynamic;
FETCH TovarCursorDynamic INTO @tv2, @cena2;

PRINT 'Динамический курсор (до изменения): ' + @tv2 + ' - ' + CAST(@cena2 AS VARCHAR(10));

-- Изменяем данные
UPDATE Товары SET Цена = Цена + 1000 WHERE Название = 'Ноутбук Lenovo';

FETCH TovarCursorDynamic INTO @tv2, @cena2;
PRINT 'Динамический курсор (после изменения): ' + @tv2 + ' - ' + CAST(@cena2 AS VARCHAR(10));

CLOSE TovarCursorDynamic;
DEALLOCATE TovarCursorDynamic;
GO
-- Задание 4: 
 DECLARE @tv NVARCHAR(255), @cena DECIMAL(10,2);

DECLARE TovarCursor CURSOR SCROLL
FOR SELECT Название, Цена FROM Товары;

OPEN TovarCursor;

FETCH FIRST FROM TovarCursor INTO @tv, @cena;
PRINT 'FIRST: ' + @tv + ' - ' + CAST(@cena AS VARCHAR(10));

FETCH NEXT FROM TovarCursor INTO @tv, @cena;
PRINT 'NEXT: ' + @tv + ' - ' + CAST(@cena AS VARCHAR(10));

FETCH PRIOR FROM TovarCursor INTO @tv, @cena;
PRINT 'PRIOR: ' + @tv + ' - ' + CAST(@cena AS VARCHAR(10));

FETCH LAST FROM TovarCursor INTO @tv, @cena;
PRINT 'LAST: ' + @tv + ' - ' + CAST(@cena AS VARCHAR(10));

FETCH ABSOLUTE 2 FROM TovarCursor INTO @tv, @cena;
PRINT 'ABSOLUTE 2: ' + @tv + ' - ' + CAST(@cena AS VARCHAR(10));

FETCH RELATIVE -1 FROM TovarCursor INTO @tv, @cena;
PRINT 'RELATIVE -1: ' + @tv + ' - ' + CAST(@cena AS VARCHAR(10));

CLOSE TovarCursor;
DEALLOCATE TovarCursor;
GO
-- ЗАДАНИЕ 5 
 DECLARE @tid INT, @tvid INT;

DECLARE ZakazCursor CURSOR LOCAL
FOR SELECT ID_Заказа, ID_Товара FROM Заказы FOR UPDATE;

OPEN ZakazCursor;
FETCH ZakazCursor INTO @tid, @tvid;

WHILE @@FETCH_STATUS = 0
BEGIN
    IF @tvid = 1
    BEGIN
        UPDATE Заказы 
        SET Количество_заказанного = Количество_заказанного + 1 
        WHERE CURRENT OF ZakazCursor;
    END
    ELSE
    BEGIN
        DELETE FROM Заказы 
        WHERE CURRENT OF ZakazCursor;
    END
    FETCH ZakazCursor INTO @tid, @tvid;
END;

CLOSE ZakazCursor;
DEALLOCATE ZakazCursor;
GO
-- Задание 6: 
DECLARE @tid INT, @clientid INT;

DECLARE ZakazCursor CURSOR LOCAL
FOR SELECT z.ID_Заказа, z.ID_Клиента 
FROM Заказы z 
JOIN Клиенты k ON z.ID_Клиента = k.ID_Клиента 
WHERE z.Количество_заказанного < 4 FOR UPDATE;

OPEN ZakazCursor;
FETCH ZakazCursor INTO @tid, @clientid;

WHILE @@FETCH_STATUS = 0
BEGIN
    DELETE FROM Заказы 
    WHERE CURRENT OF ZakazCursor;
    FETCH ZakazCursor INTO @tid, @clientid;
END;

CLOSE ZakazCursor;
DEALLOCATE ZakazCursor;
GO

DECLARE @tid INT, @clientid INT;

DECLARE ZakazCursor CURSOR LOCAL
FOR SELECT ID_Заказа, ID_Клиента 
FROM Заказы 
WHERE ID_Клиента = 1 FOR UPDATE OF Количество_заказанного;

OPEN ZakazCursor;
FETCH ZakazCursor INTO @tid, @clientid;

WHILE @@FETCH_STATUS = 0
BEGIN
    UPDATE Заказы 
    SET Количество_заказанного = Количество_заказанного + 1 
    WHERE CURRENT OF ZakazCursor;
    FETCH ZakazCursor INTO @tid, @clientid;
END;

CLOSE ZakazCursor;
DEALLOCATE ZakazCursor;
GO



