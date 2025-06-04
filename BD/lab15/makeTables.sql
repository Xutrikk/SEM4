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
/*lab15*/
/*Задание 1*/
-- Создание таблицы TR_AUDIT
CREATE TABLE TR_AUDIT
(
    ID INT IDENTITY, -- Уникальный номер записи
    STMT VARCHAR(20), -- Событие (INSERT, DELETE, UPDATE)
    TR_NAME VARCHAR(50), -- Имя триггера
    CC VARCHAR(300) -- Комментарий с данными
);
GO

-- Создание AFTER-триггера TR_TOVAR_INS для таблицы Товары
CREATE TRIGGER TR_TOVAR_INS
ON Товары
AFTER INSERT
AS
BEGIN
    INSERT INTO TR_AUDIT (STMT, TR_NAME, CC)
    SELECT 'INS', 'TR_TOVAR_INS', 'Добавлен товар: ID=' + CAST(ID_Товара AS VARCHAR(10)) + ', Название=' + Название + ', Количество=' + CAST(Количество_на_складе AS VARCHAR(10)) + ', Цена=' + CAST(Цена AS VARCHAR(10)) + ', Ед.изм.=' + Единица_измерения
    FROM inserted;
END;
GO

INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
VALUES ('Принтер HP', 20, 15000.00, 'шт');

SELECT * FROM TR_AUDIT;
/*Задание 2*/
CREATE TRIGGER TR_TOVAR_DEL
ON Товары
AFTER DELETE
AS
BEGIN
    INSERT INTO TR_AUDIT (STMT, TR_NAME, CC)
    SELECT 'DEL', 'TR_TOVAR_DEL', 'Удален товар: ID=' + CAST(ID_Товара AS VARCHAR(10)) + ', Название=' + Название
    FROM deleted;
END;
GO

DELETE FROM Товары WHERE Название = 'Принтер HP';

SELECT * FROM TR_AUDIT;
/*Задание 3*/
CREATE TRIGGER TR_TOVAR_UPD
ON Товары
AFTER UPDATE
AS
BEGIN
    INSERT INTO TR_AUDIT (STMT, TR_NAME, CC)
    SELECT 'UPD', 'TR_TOVAR_UPD', 'Обновлен товар: ID=' + CAST(i.ID_Товара AS VARCHAR(10)) + ', Название=' + i.Название + ', Старая цена=' + CAST(d.Цена AS VARCHAR(10)) + ', Новая цена=' + CAST(i.Цена AS VARCHAR(10))
    FROM inserted i
    JOIN deleted d ON i.ID_Товара = d.ID_Товара;
END;
GO

UPDATE Товары
SET Цена = 16000.00
WHERE Название = 'Ноутбук Lenovo';

SELECT * FROM TR_AUDIT;
/*Задание 4*/
CREATE TRIGGER TR_TOVAR_ALL
ON Товары
AFTER INSERT, DELETE, UPDATE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted)
    BEGIN
        INSERT INTO TR_AUDIT (STMT, TR_NAME, CC)
        SELECT 'INS', 'TR_TOVAR_ALL', 'Добавлен товар: ID=' + CAST(ID_Товара AS VARCHAR(10)) + ', Название=' + Название
        FROM inserted;
    END
    IF EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO TR_AUDIT (STMT, TR_NAME, CC)
        SELECT 'DEL', 'TR_TOVAR_ALL', 'Удален товар: ID=' + CAST(ID_Товара AS VARCHAR(10)) + ', Название=' + Название
        FROM deleted;
    END
    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO TR_AUDIT (STMT, TR_NAME, CC)
        SELECT 'UPD', 'TR_TOVAR_ALL', 'Обновлен товар: ID=' + CAST(i.ID_Товара AS VARCHAR(10)) + ', Название=' + i.Название + ', Старая цена=' + CAST(d.Цена AS VARCHAR(10)) + ', Новая цена=' + CAST(i.Цена AS VARCHAR(10))
        FROM inserted i
        JOIN deleted d ON i.ID_Товара = d.ID_Товара;
    END
END;
GO

INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
VALUES ('Монитор Samsung', 15, 18000.00, 'шт');

UPDATE Товары
SET Цена = 19000.00
WHERE Название = 'Монитор Samsung';

DELETE FROM Товары WHERE Название = 'Монитор Samsung';

SELECT * FROM TR_AUDIT;
/*Задание 5*/
-- Добавляем ограничение
ALTER TABLE Товары
ADD CONSTRAINT CHK_Quantity CHECK (Количество_на_складе >= 0);

-- Триггер для проверки
CREATE TRIGGER TR_TOVAR_CHK
ON Товары
AFTER INSERT
AS
BEGIN
    INSERT INTO TR_AUDIT (STMT, TR_NAME, CC)
    SELECT 'INS', 'TR_TOVAR_CHK', 'Проверка вставки: ' + Название
    FROM inserted;
END;
GO

BEGIN TRY
    INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
    VALUES ('Тест', -5, 1000.00, 'шт');
END TRY
BEGIN CATCH
    PRINT 'Ошибка: ' + ERROR_MESSAGE();
END CATCH;

SELECT * FROM TR_AUDIT;
/*Задание 6*/
CREATE TRIGGER TR_TOVAR_DEL1
ON Товары
AFTER DELETE
AS
BEGIN
    INSERT INTO TR_AUDIT (STMT, TR_NAME, CC)
    SELECT 'DEL', 'TR_TOVAR_DEL1', 'DEL1: Удален товар: ' + Название
    FROM deleted;
END;
GO

CREATE TRIGGER TR_TOVAR_DEL2
ON Товары
AFTER DELETE
AS
BEGIN
    INSERT INTO TR_AUDIT (STMT, TR_NAME, CC)
    SELECT 'DEL', 'TR_TOVAR_DEL2', 'DEL2: Удален товар: ' + Название
    FROM deleted;
END;
GO

CREATE TRIGGER TR_TOVAR_DEL3
ON Товары
AFTER DELETE
AS
BEGIN
    INSERT INTO TR_AUDIT (STMT, TR_NAME, CC)
    SELECT 'DEL', 'TR_TOVAR_DEL3', 'DEL3: Удален товар: ' + Название
    FROM deleted;
END;
GO

-- Упорядочивание
EXEC sp_settriggerorder @triggername = 'TR_TOVAR_DEL3', @order = 'First', @stmttype = 'DELETE';
EXEC sp_settriggerorder @triggername = 'TR_TOVAR_DEL2', @order = 'Last', @stmttype = 'DELETE';

SELECT name, is_disabled
FROM sys.triggers
WHERE parent_id = OBJECT_ID('Товары');

-- Проверка событий триггеров
SELECT t.name AS trigger_name, te.type_desc AS event_type
FROM sys.triggers t
JOIN sys.trigger_events te ON t.object_id = te.object_id
WHERE t.parent_id = OBJECT_ID('Товары')
AND te.type_desc = 'DELETE';

INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
VALUES ('Тестовый товар', 10, 5000.00, 'шт');

DELETE FROM Товары WHERE Название = 'Тестовый товар';

SELECT * FROM TR_AUDIT WHERE CC LIKE '%Тестовый товар%';
/*Задание 7*/
CREATE TRIGGER TR_TOVAR_TXN
ON Товары
AFTER UPDATE
AS
BEGIN
    DECLARE @min_price DECIMAL(10,2) = 10000.00;
    IF EXISTS (SELECT 1 FROM inserted WHERE Цена < @min_price)
    BEGIN
        RAISERROR ('Цена не может быть меньше 10000!', 16, 1);
        ROLLBACK;
    END
    ELSE
    BEGIN
        INSERT INTO TR_AUDIT (STMT, TR_NAME, CC)
        SELECT 'UPD', 'TR_TOVAR_TXN', 'Обновление: ' + Название
        FROM inserted;
    END
END;
GO

-- Тест 1: Обновление с нарушением
BEGIN TRANSACTION;
UPDATE Товары
SET Цена = 5000.00
WHERE Название = 'Ноутбук Lenovo';
COMMIT;

SELECT * FROM TR_AUDIT WHERE CC LIKE '%Ноутбук Lenovo%';

-- Тест 2: Обновление без нарушения
BEGIN TRANSACTION;
UPDATE Товары
SET Цена = 20000.00
WHERE Название = 'Ноутбук Lenovo';
COMMIT;

SELECT * FROM TR_AUDIT WHERE CC LIKE '%Ноутбук Lenovo%';
/*Задание 8*/
CREATE TRIGGER TR_TOVAR_INSTEAD_OF_DEL
ON Товары
INSTEAD OF DELETE
AS
BEGIN
    RAISERROR ('Удаление запрещено!', 16, 1);
    ROLLBACK;
END;
GO

-- Проверка
BEGIN TRY
    DELETE FROM Товары WHERE Название = 'Ноутбук Lenovo';
END TRY
BEGIN CATCH
    PRINT 'Ошибка: ' + ERROR_MESSAGE();
END CATCH;

DROP TRIGGER TR_TOVAR_DEL1, TR_TOVAR_DEL2, TR_TOVAR_DEL3, TR_TOVAR_TXN, TR_TOVAR_INSTEAD_OF_DEL;
/*Задание 9*/
CREATE TRIGGER TR_DDL_PRODAJI
ON DATABASE
FOR DDL_DATABASE_LEVEL_EVENTS
AS
BEGIN
    DECLARE @EventType VARCHAR(50) = EVENTDATA().value('(/EVENT_INSTANCE/EventType)[1]', 'varchar(50)');
    DECLARE @ObjectName VARCHAR(50) = EVENTDATA().value('(/EVENT_INSTANCE/ObjectName)[1]', 'varchar(50)');
    IF @EventType IN ('CREATE_TABLE', 'DROP_TABLE')
    BEGIN
        RAISERROR ('Запрещено %s таблицу %s!', 16, 1, @EventType, @ObjectName);
        ROLLBACK;
    END;
END;
GO

-- Проверка
BEGIN TRY
    CREATE TABLE TestTable (ID INT);
END TRY
BEGIN CATCH
    PRINT 'Ошибка: ' + ERROR_MESSAGE();
END CATCH;



