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
/*lab12*/
-- Задание 1: Демонстрация работы в режиме неявной транзакции (SET IMPLICIT_TRANSACTIONS OFF и ON)
-- Часть 1: SET IMPLICIT_TRANSACTIONS OFF
USE [HUT-MyBase];
GO

-- Задание 1: Демонстрация работы в режиме неявной транзакции (SET IMPLICIT_TRANSACTIONS OFF и ON)
-- Часть 1: SET IMPLICIT_TRANSACTIONS OFF
SET IMPLICIT_TRANSACTIONS OFF;

BEGIN TRY
    BEGIN TRAN T1;
        DELETE FROM Товары WHERE Название = 'Ноутбук Lenovo';
        INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
        VALUES ('Монитор Dell', 5, 20000.00, 'шт');
    COMMIT TRAN T1;
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 1 (Часть 1): ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK TRAN T1;
END CATCH;


SELECT * FROM Товары;

-- Часть 2: SET IMPLICIT_TRANSACTIONS ON
SET IMPLICIT_TRANSACTIONS ON;

BEGIN TRY
    DELETE FROM Товары WHERE Название = 'Ноутбук Lenovo';
    INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
    VALUES ('Монитор Dell', 5, 20000.00, 'шт');
    COMMIT;
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 1 (Часть 2): ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK;
END CATCH;

SET IMPLICIT_TRANSACTIONS OFF;


SELECT * FROM Товары;

-- Результат: Здесь ошибок нет, так как данные корректны.

-- Задание 2: Демонстрация свойства атомарности транзакций
BEGIN TRY
    BEGIN TRAN;
        DELETE FROM Товары WHERE Название = 'Ноутбук Lenovo';
        INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
        VALUES ('Монитор Dell', -5, 20000.00, 'шт'); -- Ошибка: Количество_на_складе не может быть отрицательным
    COMMIT TRAN;
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 2: ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK TRAN;
END CATCH;

SELECT * FROM Товары;

-- Результат: Теперь должна появиться ошибка из-за нарушения ограничения CHECK. ROLLBACK сохранит атомарность.

-- Задание 3: Применение оператора SAVE TRANSACTION
BEGIN TRY
    BEGIN TRAN;
        DELETE FROM Товары WHERE Название = 'Ноутбук Lenovo';
        SAVE TRAN SP1; 
        INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
        VALUES ('Монитор Dell', 5, 20000.00, 'шт');
        SAVE TRAN SP2; 
        INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
        VALUES ('Принтер HP', -5, 15000.00, 'шт'); -- Ошибка: Количество_на_складе не может быть отрицательным
    COMMIT TRAN;
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 3: ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK TRAN SP1; -- Откат до SP1
        COMMIT TRAN; -- Фиксируем изменения до SP1
END CATCH;

SELECT * FROM Товары;

-- Результат: Ошибка из-за отрицательного Количество_на_складе вызовет откат до SP1. Удаление "Ноутбук Lenovo" останется, остальные изменения отменятся.

USE [HUT-MyBase];
GO

-- Задание 4: READ UNCOMMITTED (Транзакция A)
BEGIN TRY
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
    BEGIN TRAN;
        SELECT * FROM Товары WHERE Название = 'Ноутбук Lenovo'; -- t1
        WAITFOR DELAY '00:00:05'; -- Задержка
        SELECT * FROM Товары WHERE Название = 'Ноутбук Lenovo'; -- t2
    COMMIT;
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 4 (READ UNCOMMITTED, Транзакция A): ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK;
END CATCH;

-- Задание 4: READ COMMITTED (Транзакция A)
BEGIN TRY
    SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
    BEGIN TRAN;
        SELECT * FROM Товары WHERE Название = 'Ноутбук Lenovo NEW 52'; -- t1
        WAITFOR DELAY '00:00:05'; -- Задержка
        SELECT * FROM Товары WHERE Название = 'Ноутбук Lenovo'; -- t2
    COMMIT;
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 4 (READ COMMITTED, Транзакция A): ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK;
END CATCH;

-- Задание 5: READ COMMITTED (Транзакция A)
BEGIN TRY
    SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
    BEGIN TRAN;
        SELECT COUNT(*) FROM Товары WHERE Название = 'Ноутбук Lenovo'; -- t1
        WAITFOR DELAY '00:00:05'; -- Задержка
        SELECT COUNT(*) FROM Товары WHERE Название = 'Ноутбук Lenovo'; -- t2
    COMMIT;
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 5 (READ COMMITTED, Транзакция A): ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK;
END CATCH;

-- Задание 5: REPEATABLE READ (Транзакция A)
BEGIN TRY
    SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
    BEGIN TRAN;
        SELECT COUNT(*) FROM Товары WHERE Название = 'Ноутбук Lenovo'; -- t1
        WAITFOR DELAY '00:00:05'; -- Задержка
        SELECT COUNT(*) FROM Товары WHERE Название = 'Ноутбук Lenovo'; -- t2
    COMMIT;
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 5 (REPEATABLE READ, Транзакция A): ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK;
END CATCH;

-- Задание 6: REPEATABLE READ (Транзакция A)
BEGIN TRY
    SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
    BEGIN TRAN;
        SELECT * FROM Товары WHERE Название = 'Ноутбук Lenovo'; -- t1
        WAITFOR DELAY '00:00:05'; -- Задержка
        INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
        VALUES ('Монитор Dell', 5, 20000.00, 'шт'); -- t2
    COMMIT;
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 6 (REPEATABLE READ, Транзакция A): ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK;
END CATCH;

-- Задание 6: SERIALIZABLE (Транзакция A)
BEGIN TRY
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRAN;
        SELECT * FROM Товары WHERE Название = 'Ноутбук Lenovo'; -- t1
        WAITFOR DELAY '00:00:05'; -- Задержка
        INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
        VALUES ('Монитор Dell', 5, 20000.00, 'шт'); -- t2
    COMMIT;
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 6 (SERIALIZABLE, Транзакция A): ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK;
END CATCH;

-- Задание 7: SERIALIZABLE (Транзакция A)
BEGIN TRY
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRAN;
        DELETE FROM Товары WHERE Название = 'Ноутбук Lenovo';
        INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
        VALUES ('Монитор Dell', 5, 20000.00, 'шт');
        UPDATE Товары SET Название = 'Монитор Dell NEW' WHERE Название = 'Монитор Dell'; -- t1
        SELECT * FROM Товары WHERE Название = 'Монитор Dell NEW'; -- t2
    COMMIT;
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 7 (Транзакция A): ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK;
END CATCH;
-- Задание 8: Демонстрация свойств блокировки транзакций
BEGIN TRY
    BEGIN TRAN;
        INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
        VALUES ('Монитор Dell', -5, 20000.00, 'шт'); -- Ошибка: Количество_на_складе не может быть отрицательным
        BEGIN TRAN InnerTran;
            UPDATE Товары SET Название = 'Монитор Dell NEW' WHERE Название = 'Монитор Dell';
            COMMIT;
        IF @@TRANCOUNT > 0
            ROLLBACK;
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 8: ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK;
END CATCH;

SELECT (SELECT COUNT(*) FROM Товары WHERE Название = 'Монитор Dell') AS 'До_отката',
       (SELECT COUNT(*) FROM Товары WHERE Название = 'Монитор Dell NEW') AS 'После_отката';

-- Результат: Теперь ошибка из-за отрицательного Количество_на_складе должна быть отображена, и ROLLBACK отменит все изменения.
