USE [HUT-MyBase];
GO

-- Задание 4: Транзакция B (для обоих уровней изоляции)
BEGIN TRY
    BEGIN TRAN;
        UPDATE Товары SET Название = 'Ноутбук Lenovo NEW 52 52'
        WHERE Название = 'Ноутбук Lenovo'; -- t1
        WAITFOR DELAY '00:00:10'; -- Задержка
        COMMIT; -- t2
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 4 (Транзакция B): ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK;
END CATCH;

-- Задание 5: Транзакция B (для обоих уровней изоляции)
BEGIN TRY
    BEGIN TRAN;
        UPDATE Товары SET Название = 'Ноутбук Lenovo NEW 5222'
        WHERE Название = 'Ноутбук Lenovo NEW 52'; -- t1
        COMMIT; -- t2
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 5 (Транзакция B): ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK;
END CATCH;

-- Задание 6: Транзакция B (для обоих уровней изоляции)
BEGIN TRY
    BEGIN TRAN;
        INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
        VALUES ('Принтер HPР', 3, 15000.00, 'шт'); -- t1
        COMMIT; -- t2
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 6 (Транзакция B): ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK;
END CATCH;

-- Задание 7: SERIALIZABLE (Транзакция B)
BEGIN TRY
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRAN;
        DELETE FROM Товары WHERE Название = 'Ноутбук Lenovo';
        INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)
        VALUES ('Принтер HP', 3, 15000.00, 'шт');
        UPDATE Товары SET Название = 'Принтер HP NEW' WHERE Название = 'Принтер HP';
        SELECT * FROM Товары WHERE Название = 'Принтер HP NEW52';
    COMMIT;
END TRY
BEGIN CATCH
    PRINT 'Ошибка в Задании 7 (Транзакция B): ' + CAST(ERROR_NUMBER() AS NVARCHAR) + ', ' + ERROR_MESSAGE();
    IF @@TRANCOUNT > 0
        ROLLBACK;
END CATCH;