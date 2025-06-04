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
/*lab13*/
-- Задание 1
CREATE PROCEDURE PrZakazy
AS
BEGIN
    DECLARE @k INT;
    SELECT @k = (SELECT COUNT(*) FROM Заказы);
    PRINT 'Количество заказов: ' + CAST(@k AS VARCHAR(10));
END;
GO

EXEC PrZakazy;

-- Задание 2
ALTER PROCEDURE PrZakazy
    @p VARCHAR(20)
AS
BEGIN
    DECLARE @k INT;
    SELECT @k = (SELECT COUNT(*) FROM Заказы WHERE ID_Клиента = @p);
    IF @k = 0
        PRINT 'Клиент ' + @p + ' не делал заказов';
    ELSE
        PRINT 'Количество заказов клиента ' + @p + ': ' + CAST(@k AS VARCHAR(10));
END;
GO

DECLARE @k INT;
EXEC @k = PrZakazy @p = '1';
PRINT 'Код возврата: ' + CAST(@k AS VARCHAR(3));

EXEC @k = PrZakazy @p = '2';
PRINT 'Код возврата: ' + CAST(@k AS VARCHAR(3));

-- Задание 3
-- Создание временной таблицы #ZK
CREATE TABLE #ZK
(
    Название_товара NVARCHAR(255),
    Цена DECIMAL(10,2),
    Количество_заказанного INT
);

INSERT INTO #ZK (Название_товара, Цена, Количество_заказанного)
SELECT Товары.Название, Товары.Цена, Заказы.Количество_заказанного
FROM Заказы
JOIN Товары ON Заказы.ID_Товара = Товары.ID_Товара;

-- Изменение процедуры PrZakazy
ALTER PROCEDURE PrZakazy
    @p VARCHAR(20)
AS
BEGIN
    DECLARE @k INT;
    SELECT @k = (SELECT COUNT(*) FROM #ZK);
    IF @k = 0
        PRINT 'Нет данных о заказах';
    ELSE
        SELECT * FROM #ZK;
END;
GO

EXEC PrZakazy @p = '1';

-- Задание 4
CREATE PROCEDURE TovaryInsert
    @a NVARCHAR(255),  -- Название
    @b DECIMAL(10,2),  -- Цена
    @c INT,            -- Количество на складе
    @d NVARCHAR(50)    -- Единица измерения
AS
BEGIN
    DECLARE @rc INT = 1;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO Товары (Название, Цена, Количество_на_складе, Единица_измерения)
        VALUES (@a, @b, @c, @d);
        COMMIT TRANSACTION;
        RETURN @rc;
    END TRY
    BEGIN CATCH
        PRINT 'Номер ошибки: ' + CAST(ERROR_NUMBER() AS VARCHAR(6));
        PRINT 'Сообщение: ' + ERROR_MESSAGE();
        PRINT 'Уровень: ' + CAST(ERROR_SEVERITY() AS VARCHAR(6));
        PRINT 'Метка: ' + CAST(ERROR_STATE() AS VARCHAR(8));
        PRINT 'Номер строки: ' + CAST(ERROR_LINE() AS VARCHAR(8));
        IF ERROR_PROCEDURE() IS NOT NULL
            PRINT 'Имя процедуры: ' + ERROR_PROCEDURE();
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RETURN @rc;
    END CATCH;
END;
GO

DECLARE @rc INT;
EXEC @rc = TovaryInsert @a = 'Монитор Dell', @b = 20000.00, @c = 15, @d = 'шт';
PRINT 'Код ошибки: ' + CAST(@rc AS VARCHAR(3));

-- Задание 5
CREATE PROCEDURE ZKlovCURSOR
    @p CHAR(10)
AS
BEGIN
    DECLARE @rc INT = 0;
    DECLARE @t1 NVARCHAR(255), @t2 INT;
    DECLARE ZKlov CURSOR FOR
        SELECT Товары.Название, Заказы.Количество_заказанного
        FROM Заказы
        JOIN Товары ON Заказы.ID_Товара = Товары.ID_Товара
        WHERE Заказы.ID_Клиента = @p;

    IF NOT EXISTS (
        SELECT Название
        FROM Заказы
        JOIN Товары ON Заказы.ID_Товара = Товары.ID_Товара
        WHERE Заказы.ID_Клиента = @p
    )
    BEGIN
        RAISERROR('Нет заказов', 11, 1);
        RETURN @rc;
    END;

    OPEN ZKlov;
    FETCH ZKlov INTO @t1, @t2;
    PRINT 'Заказы клиента:';
    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT @t1 + ': ' + CAST(@t2 AS NVARCHAR(10));
        FETCH ZKlov INTO @t1, @t2;
    END;
    CLOSE ZKlov;
    DEALLOCATE ZKlov;
    RETURN @rc;
END;
GO

DECLARE @rc INT;
EXEC @rc = ZKlovCURSOR @p = '1';
PRINT 'Код ошибки: ' + CAST(@rc AS VARCHAR(3));

-- Задание 6
ALTER PROCEDURE TovaryInsert
    @a NVARCHAR(255),  -- Название
    @b DECIMAL(10,2),  -- Цена
    @c INT,            -- Количество на складе
    @d NVARCHAR(50)    -- Единица измерения
AS
BEGIN
    DECLARE @rc INT = 1;
    BEGIN TRY
        -- Проверка допустимости данных
        IF @a IS NULL OR LTRIM(RTRIM(@a)) = ''
            THROW 50001, 'Название не может быть пустым', 1;
        IF @b <= 0
            THROW 50002, 'Цена должна быть больше 0', 1;
        IF @c < 0
            THROW 50003, 'Количество на складе не может быть отрицательным', 1;
        IF @d IS NULL OR LTRIM(RTRIM(@d)) = ''
            THROW 50004, 'Единица измерения не может быть пустой', 1;

        BEGIN TRANSACTION;
        INSERT INTO Товары (Название, Цена, Количество_на_складе, Единица_измерения)
        VALUES (@a, @b, @c, @d);
        COMMIT TRANSACTION;
        RETURN @rc;
    END TRY
    BEGIN CATCH
        PRINT 'Номер ошибки: ' + CAST(ERROR_NUMBER() AS VARCHAR(6));
        PRINT 'Сообщение: ' + ERROR_MESSAGE();
        PRINT 'Уровень: ' + CAST(ERROR_SEVERITY() AS VARCHAR(6));
        PRINT 'Метка: ' + CAST(ERROR_STATE() AS VARCHAR(8));
        PRINT 'Номер строки: ' + CAST(ERROR_LINE() AS VARCHAR(8));
        IF ERROR_PROCEDURE() IS NOT NULL
            PRINT 'Имя процедуры: ' + ERROR_PROCEDURE();
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        RETURN @rc;
    END CATCH;
END;
GO

-- Вызов процедуры с некорректными данными
DECLARE @rc INT;
EXEC @rc = TovaryInsert @a = '', @b = 0, @c = -5, @d = '';
PRINT 'Код ошибки: ' + CAST(@rc AS VARCHAR(3));