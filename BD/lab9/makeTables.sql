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
/*lab9*/
-- ЗАДАНИЕ 1
DECLARE 
    @i INT = 1,
    @b VARCHAR(4) = 'БГТУ',
    @c DATETIME,
    @tm TIME,
    @ch CHAR(1),
    @si SMALLINT,
    @ti TINYINT,
    @n NUMERIC(12,5);

SET @c = GETDATE();
SET @tm = SYSDATETIME();
SET @ch = 'C';

SELECT @si = 4, @ch = 'D';

SELECT @i AS i, @b AS b, @c AS c;

PRINT 'tm = ' + CONVERT(VARCHAR, @tm);
PRINT 'si = ' + CONVERT(VARCHAR, @si);

DECLARE @h TABLE (
    num INT IDENTITY(1,1),
    fill VARCHAR(30) DEFAULT 'XXX'
);

INSERT INTO @h DEFAULT VALUES;
SELECT * FROM @h;

-- ЗАДАНИЕ 2
DECLARE 
    @totalCapacity INT,
    @averageCapacity DECIMAL(10,2),
    @countProducts INT,
    @lessThanAverage INT,
    @percentage DECIMAL(5,2);

SET @totalCapacity = (SELECT SUM(Количество_на_складе) FROM Товары);

IF @totalCapacity > 88
BEGIN
    SET @averageCapacity = (SELECT AVG(Количество_на_складе) FROM Товары);
    SET @countProducts = (SELECT COUNT(*) FROM Товары);
    SET @lessThanAverage = (SELECT COUNT(*) FROM Товары WHERE Количество_на_складе < @averageCapacity);
    SET @percentage = (@lessThanAverage * 100.0) / @countProducts;

    PRINT 'Количество товаров: ' + CAST(@countProducts AS VARCHAR);
    PRINT 'Средняя вместимость: ' + CAST(@averageCapacity AS VARCHAR);
    PRINT 'Товаров меньше средней: ' + CAST(@lessThanAverage AS VARCHAR);
    PRINT 'Процент: ' + CAST(@percentage AS VARCHAR) + '%';
END
ELSE
    PRINT 'Общая вместимость: ' + CAST(@totalCapacity AS VARCHAR);
-- ЗАДАНИЕ 3

PRINT 'Число обработанных строк: ' + CONVERT(varchar, @@ROWCOUNT);
PRINT 'Версия: ' + @@VERSION; 
PRINT 'Cистемный идентификатор процесса: ' + CONVERT(varchar,@@SPID);
PRINT 'Код последней ошибки: ' + CONVERT(varchar, @@ERROR);
PRINT 'Имя сервера: ' + CONVERT(varchar, @@SERVERNAME);
PRINT 'Уровень вложенности транзакций: ' + CONVERT(varchar, @@TRANCOUNT);
PRINT 'Проверка результата считывания строк: ' + CONVERT(varchar, @@FETCH_STATUS);
PRINT 'Уровень вложенности текущей процедуры: ' + CONVERT(varchar, @@NESTLEVEL);


 print 'Округление          : '+ cast(round(12345.12345, 2) as varchar(12)); 
 print 'Нижнее целое        : '+ cast(floor(24.5) as varchar(12));
 print 'Возведение в степень: '+ cast(power(12.0, 2) as varchar(12)); 
 print 'Логарифм            : '+ cast(log(144.0) as varchar(12));
 print 'Корень квадратный   : '+ cast(sqrt(144.0) as varchar(12));
 print 'Экпонента           : '+ cast(exp(4.96981) as varchar(12));
 print 'Абсолютное значение : '+ cast(abs(-5) as varchar(12));
 print 'Cинус               : '+ cast(sin(pi()) as varchar(12));
 print 'Подстрока              : '+  substring('1234567890', 3,2);   
 print 'Удалить пробелы справа : '+  rtrim('12345     ') +'X';
 print 'Удалить пробелы слева  : '+  'X'+ ltrim('     67890');
 print 'Нижний регистр         : '+  lower ('ВЕРХНИЙ РЕГИСТР');
 print 'Верхний регистр        : '+  upper ('нижний регистр');
 print 'Заменить               : '+  replace('1234512345', '5', 'X');
 print 'Строка пробелов        : '+  'X'+ space(5) +'X';
 print 'Повторить строку       : '+  replicate('12', 5);
 print 'Найти по шаблону       : '+  cast (patindex ('%Y_Y%', '123456YxY7890') as varchar(5));
 DECLARE @time time(7) = SYSDATETIME(), @dt datetime = getdate();
 print 'Текущее время       : '+  convert (varchar(12), @time);
 print 'Текущая дата        : '+  convert (varchar(12), @dt, 103);
 print '+1 день               : '+ convert(varchar(12), dateadd(d, 1, @dt), 103);

-- ЗАДАНИЕ 4.1 (Вычисление z)
DECLARE 
    @t INT = 5,
    @x INT = 3,
    @z FLOAT;

IF @t > @x 
    SET @z = POWER(SIN(@t), 2);
ELSE IF @t < @x 
    SET @z = 4 * (@t + @x);
ELSE 
    SET @z = 1 - EXP(POWER(@x, 2));

PRINT 'z = ' + CAST(@z AS VARCHAR);

-- ЗАДАНИЕ 4.2 (Сокращение ФИО)
DECLARE 
    @fullName VARCHAR(100) = 'Хуторцов Кирилл Владимирович',
    @shortName VARCHAR(50);

SET @shortName = 
    LEFT(@fullName, CHARINDEX(' ', @fullName)) + ' ' +
    SUBSTRING(@fullName, CHARINDEX(' ', @fullName) + 1, 1) + '. ' +
    SUBSTRING(@fullName, CHARINDEX(' ', @fullName, CHARINDEX(' ', @fullName) + 1) + 1, 1) + '.';

PRINT 'Полное ФИО: ' + @fullName;
PRINT 'Сокращенное ФИО: ' + @shortName;
-- ЗАДАНИЕ 4.3
CREATE TABLE #Студенты (
    ID_Студента INT IDENTITY PRIMARY KEY,
    Фамилия NVARCHAR(50) NOT NULL,
    Имя NVARCHAR(50) NOT NULL,
    Дата_рождения DATE NOT NULL,
    Группа NVARCHAR(20) NOT NULL
);

-- Вставка тестовых данных
INSERT INTO #Студенты (Фамилия, Имя, Дата_рождения, Группа)
VALUES 
    ('Иванов', 'Алексей', '2003-05-15', 'Группа-101'),
    ('Петрова', 'Мария', '2004-12-22', 'Группа-102'),
    ('Сидоров', 'Иван', '2002-02-10', 'Группа-101');

-- Поиск студентов, у которых день рождения в следующем месяце
DECLARE @currentDate DATE = GETDATE();
DECLARE @nextMonth INT = MONTH(DATEADD(MONTH, 1, @currentDate));

SELECT 
    Фамилия,
    Имя,
    Дата_рождения,
    -- Расчет возраста
    DATEDIFF(YEAR, Дата_рождения, @currentDate) - 
    CASE 
        WHEN DATEADD(YEAR, DATEDIFF(YEAR, Дата_рождения, @currentDate), Дата_рождения) > @currentDate 
        THEN 1 
        ELSE 0 
    END AS Возраст
FROM 
    #Студенты
WHERE 
    MONTH(Дата_рождения) = @nextMonth;
	DROP TABLE #Студенты;
	-- ЗАДАНИЕ 4.4
CREATE TABLE #Экзамены (
    ID_Экзамена INT IDENTITY PRIMARY KEY,
    ID_Студента INT NOT NULL,
    Название_экзамена NVARCHAR(100) NOT NULL,
    Дата_экзамена DATE NOT NULL,
    Группа NVARCHAR(20) NOT NULL
);

-- Вставка тестовых данных
INSERT INTO #Экзамены (ID_Студента, Название_экзамена, Дата_экзамена, Группа)
VALUES 
    (1, 'Базы данных', '2023-11-20', 'Группа-101'),
    (2, 'Базы данных', '2023-11-20', 'Группа-102'),
    (3, 'Программирование', '2023-11-25', 'Группа-101');

-- Поиск дня недели для экзамена по БД в заданной группе
DECLARE @targetGroup NVARCHAR(20) = 'Группа-101';

SELECT DISTINCT
    DATENAME(WEEKDAY, Дата_экзамена) AS День_недели
FROM 
    #Экзамены
WHERE 
    Название_экзамена = 'Базы данных' 
    AND Группа = @targetGroup;
	DROP TABLE #Экзамены;
-- ЗАДАНИЕ 5 
DECLARE 
    @totalOrders INT,
    @message NVARCHAR(100),
    @avgQuantity DECIMAL(10,2),
    @highValueOrders INT;

SET @totalOrders = (SELECT COUNT(*) FROM Заказы);

IF @totalOrders > 2
BEGIN
    SET @avgQuantity = (SELECT AVG(Количество_заказанного) FROM Заказы);
    SET @highValueOrders = (SELECT COUNT(*) FROM Заказы WHERE Количество_заказанного > @avgQuantity);

    SET @message = 'Анализ заказов: 
        - Среднее количество: ' + CAST(@avgQuantity AS NVARCHAR) + 
        ', 
        - Заказов выше среднего: ' + CAST(@highValueOrders AS NVARCHAR);
END
ELSE
    SET @message = 'Недостаточно данных для анализа. Всего заказов: ' + CAST(@totalOrders AS NVARCHAR);

PRINT @message;

-- ЗАДАНИЕ 6 (Анализ цен)
SELECT 
    CASE 
        WHEN Цена BETWEEN 0 AND 1000 THEN 'Дешево'
        WHEN Цена BETWEEN 1001 AND 5000 THEN 'Нормально'
        WHEN Цена BETWEEN 5001 AND 10000 THEN 'Дорого'
        ELSE 'Очень дорого'
    END AS [Категория цены],
    COUNT(*) AS [Количество товаров]
FROM Товары
GROUP BY 
    CASE 
        WHEN Цена BETWEEN 0 AND 1000 THEN 'Дешево'
        WHEN Цена BETWEEN 1001 AND 5000 THEN 'Нормально'
        WHEN Цена BETWEEN 5001 AND 10000 THEN 'Дорого'
        ELSE 'Очень дорого'
    END;

-- ЗАДАНИЕ 7 (Временная таблица)
CREATE TABLE #TempTable (
    id INT,
    field VARCHAR(50)
);

DECLARE @iterator INT = 0;

WHILE @iterator < 10
BEGIN
    INSERT INTO #TempTable (id, field) 
    VALUES (@iterator, REPLICATE('Строка', @iterator));
    SET @iterator += 1;
END;

SELECT * FROM #TempTable;
DROP TABLE #TempTable;

-- ЗАДАНИЕ 8 (RETURN)
DECLARE @var INT = 4;

PRINT 'Значение: ' + CAST(@var AS VARCHAR);
PRINT 'Значение + 1: ' + CAST(@var + 1 AS VARCHAR);
RETURN;
PRINT 'Этот код не выполнится';

-- ЗАДАНИЕ 9 (TRY...CATCH)
BEGIN TRY
    CREATE TABLE #PEOPLES (
        FirstName NVARCHAR(50) NOT NULL,
        Age INT NOT NULL
    );

    INSERT INTO #PEOPLES VALUES (NULL, NULL); -- Намеренная ошибка
END TRY
BEGIN CATCH
    PRINT 
        'Ошибка ' + CAST(ERROR_NUMBER() AS VARCHAR) + ': ' + 
        ERROR_MESSAGE() + ' (Строка: ' + CAST(ERROR_LINE() AS VARCHAR) + ')';
END CATCH;