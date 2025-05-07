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
/*lab10*/
-- Задание 1: Определение индексов, создание временной таблицы и кластеризованного индекса

EXEC sp_helpindex 'Товары';


EXEC sp_helpindex 'Клиенты';


EXEC sp_helpindex 'Заказы';

CREATE TABLE #TempSales (  
    SaleID INT IDENTITY,  
    КлиентID INT,  
    ТоварID INT,  
    Количество INT,  
    Дата DATE  
);  
 
INSERT INTO #TempSales (КлиентID, ТоварID, Количество, Дата)  
SELECT  
    ABS(CHECKSUM(NEWID())) % 2 + 1,  
    ABS(CHECKSUM(NEWID())) % 3 + 1,  
    ABS(CHECKSUM(NEWID())) % 10 + 1,  
    DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 365, GETDATE())  
FROM sys.all_objects a, sys.all_objects b;  

SELECT * FROM #TempSales  
WHERE Количество BETWEEN 5 AND 10  
ORDER BY Количество;  

CREATE CLUSTERED INDEX IX_TempSales_Количество  
ON #TempSales (Количество);  
DROP TABLE #TempSales
-- Задание 2: Временная таблица (10000+ строк) и составной индекс
CREATE TABLE #BigSales (  
    SaleID INT IDENTITY PRIMARY KEY,  
    КлиентID INT,  
    ТоварID INT,  
    Дата DATE  
);  

INSERT INTO #BigSales (КлиентID, ТоварID, Дата)  
SELECT  
    ABS(CHECKSUM(NEWID())) % 100 + 1,  
    ABS(CHECKSUM(NEWID())) % 50 + 1,  
    DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 365, GETDATE())  
FROM sys.all_objects a, sys.all_objects b;  

CREATE NONCLUSTERED INDEX IX_BigSales_Клиент_Товар  
ON #BigSales (КлиентID, ТоварID);  

SELECT * FROM #BigSales  
WHERE КлиентID = 5 AND ТоварID = 10;  
-- Задание 3: Индекс покрытия
CREATE NONCLUSTERED INDEX IX_Товары_Покрытие  
ON Товары (ID_Товара)  
INCLUDE (Название, Цена);  

SELECT Название, Цена  
FROM Товары  
WHERE ID_Товара = 1;  
-- Задание 4: Фильтруемый индекс
CREATE NONCLUSTERED INDEX IX_Товары_Фильтр  
ON Товары (Количество_на_складе)  
WHERE Количество_на_складе > 20;  

SELECT * FROM Товары  
WHERE Количество_на_складе > 20;  
-- ЗАДАНИЕ 5  Фрагментация индекса
-- 1. Создание временной таблицы
CREATE TABLE #TempStock (
    ID INT,
    Количество INT
);

-- 2. Заполнение таблицы начальными данными (~1 млн строк, фильтруется до ~20000)
INSERT INTO #TempStock (ID, Количество)
SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)), ABS(CHECKSUM(NEWID())) % 50
FROM sys.all_objects a, sys.all_objects b;

-- 3. Создание некластеризованного индекса
CREATE NONCLUSTERED INDEX IX_TempStock_Количество ON #TempStock (Количество);

-- 4. Проверка фрагментации до добавления данных
SELECT 
    name AS [Индекс], 
    avg_fragmentation_in_percent AS [Фрагментация (%)]
FROM sys.dm_db_index_physical_stats(DB_ID(N'tempdb'), OBJECT_ID(N'tempdb..#TempStock'), NULL, NULL, NULL) ss
JOIN sys.indexes ii 
    ON ss.object_id = ii.object_id 
    AND ss.index_id = ii.index_id
WHERE name IS NOT NULL;

-- 5. Увеличение фрагментации путём добавления 10000 строк (дубликатов)
INSERT INTO #TempStock (ID, Количество)
SELECT TOP 10000 ID, Количество FROM #TempStock;

-- 6. Проверка фрагментации после добавления данных
SELECT 
    name AS [Индекс], 
    avg_fragmentation_in_percent AS [Фрагментация (%)]
FROM sys.dm_db_index_physical_stats(DB_ID(N'tempdb'), OBJECT_ID(N'tempdb..#TempStock'), NULL, NULL, NULL) ss
JOIN sys.indexes ii 
    ON ss.object_id = ii.object_id 
    AND ss.index_id = ii.index_id
WHERE name IS NOT NULL;

-- 7. Реорганизация индекса
ALTER INDEX IX_TempStock_Количество ON #TempStock REORGANIZE;

-- 8. Проверка фрагментации после реорганизации
SELECT 
    name AS [Индекс], 
    avg_fragmentation_in_percent AS [Фрагментация (%)]
FROM sys.dm_db_index_physical_stats(DB_ID(N'tempdb'), OBJECT_ID(N'tempdb..#TempStock'), NULL, NULL, NULL) ss
JOIN sys.indexes ii 
    ON ss.object_id = ii.object_id 
    AND ss.index_id = ii.index_id
WHERE name IS NOT NULL;

-- 9. Перестройка индекса
ALTER INDEX IX_TempStock_Количество ON #TempStock REBUILD;

-- 10. Проверка фрагментации после перестройки
SELECT 
    name AS [Индекс], 
    avg_fragmentation_in_percent AS [Фрагментация (%)]
FROM sys.dm_db_index_physical_stats(DB_ID(N'tempdb'), OBJECT_ID(N'tempdb..#TempStock'), NULL, NULL, NULL) ss
JOIN sys.indexes ii 
    ON ss.object_id = ii.object_id 
    AND ss.index_id = ii.index_id
WHERE name IS NOT NULL;
-- Задание 6: FILLFACTOR 
CREATE NONCLUSTERED INDEX IX_Товары_FillFactor  
ON Товары (Количество_на_складе)  
WITH (FILLFACTOR = 65);  

INSERT INTO Товары (Название, Количество_на_складе, Цена, Единица_измерения)  
SELECT TOP 10000 Название, Количество_на_складе, Цена, Единица_измерения  
FROM Товары;  

SELECT name [Индекс], avg_fragmentation_in_percent [Фрагментация (%)]  
FROM sys.dm_db_index_physical_stats(DB_ID(), OBJECT_ID(N'Товары'), NULL, NULL, NULL) ss  
JOIN sys.indexes ii ON ss.object_id = ii.object_id AND ss.index_id = ii.index_id;  



