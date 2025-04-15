CREATE DATABASE [HUT-MyBase];
CREATE TABLE ������ (
    ID_������ INT IDENTITY PRIMARY KEY, 
    �������� NVARCHAR(255) NOT NULL, 
    ����������_��_������ INT NOT NULL, 
    ���� DECIMAL(10,2) NOT NULL, 
    �������_��������� NVARCHAR(50) NOT NULL 
);
GO


CREATE TABLE ������� (
    ID_������� INT IDENTITY PRIMARY KEY, 
    ������� NVARCHAR(100) NOT NULL, 
    ��� NVARCHAR(100) NOT NULL, 
    �������� NVARCHAR(100) NULL, 
    ����� NVARCHAR(255) NOT NULL, 
    ������� NVARCHAR(20) NOT NULL, 
    Email NVARCHAR(100) NOT NULL UNIQUE, 
    �������_������ BIT DEFAULT 0 -- 
);
GO


CREATE TABLE ������ (
    ID_������ INT IDENTITY PRIMARY KEY, 
    ID_������� INT NOT NULL, 
    ID_������ INT NOT NULL, 
    ����������_����������� INT NOT NULL CHECK (����������_����������� > 0), 
    ����_������� DATE NOT NULL,  
    FOREIGN KEY (ID_�������) REFERENCES �������(ID_�������),
    FOREIGN KEY (ID_������) REFERENCES ������(ID_������)
);
GO
INSERT INTO ������ (��������, ����������_��_������, ����, �������_���������)
VALUES 
('������� Lenovo', 10, 75000.00, '��'),
('���������� Logitech', 50, 3500.00, '��'),
('���� Razer', 30, 5500.00, '��');

INSERT INTO ������� (�������, ���, ��������, �����, �������, Email)
VALUES 
('������', '�������', '��������', '������, ��. ������, 10', '+79001112233', 'ivanov@mail.ru'),
('������', '�����', '�������������', '�����-���������, ������� ��., 25', '+79112223344', 'petrov@mail.ru');

INSERT INTO ������ (ID_�������, ID_������, ����������_�����������, ����_�������)
VALUES 
(1, 1, 1, '2025-02-18'),
(2, 2, 2, '2025-02-18'),
(2, 3, 1, '2025-02-18');
/*lab9*/
-- ������� 1
DECLARE 
    @i INT = 1,
    @b VARCHAR(4) = '����',
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

-- ������� 2
DECLARE 
    @totalCapacity INT,
    @averageCapacity DECIMAL(10,2),
    @countProducts INT,
    @lessThanAverage INT,
    @percentage DECIMAL(5,2);

SET @totalCapacity = (SELECT SUM(����������_��_������) FROM ������);

IF @totalCapacity > 88
BEGIN
    SET @averageCapacity = (SELECT AVG(����������_��_������) FROM ������);
    SET @countProducts = (SELECT COUNT(*) FROM ������);
    SET @lessThanAverage = (SELECT COUNT(*) FROM ������ WHERE ����������_��_������ < @averageCapacity);
    SET @percentage = (@lessThanAverage * 100.0) / @countProducts;

    PRINT '���������� �������: ' + CAST(@countProducts AS VARCHAR);
    PRINT '������� �����������: ' + CAST(@averageCapacity AS VARCHAR);
    PRINT '������� ������ �������: ' + CAST(@lessThanAverage AS VARCHAR);
    PRINT '�������: ' + CAST(@percentage AS VARCHAR) + '%';
END
ELSE
    PRINT '����� �����������: ' + CAST(@totalCapacity AS VARCHAR);
-- ������� 3

PRINT '����� ������������ �����: ' + CONVERT(varchar, @@ROWCOUNT);
PRINT '������: ' + @@VERSION; 
PRINT 'C�������� ������������� ��������: ' + CONVERT(varchar,@@SPID);
PRINT '��� ��������� ������: ' + CONVERT(varchar, @@ERROR);
PRINT '��� �������: ' + CONVERT(varchar, @@SERVERNAME);
PRINT '������� ����������� ����������: ' + CONVERT(varchar, @@TRANCOUNT);
PRINT '�������� ���������� ���������� �����: ' + CONVERT(varchar, @@FETCH_STATUS);
PRINT '������� ����������� ������� ���������: ' + CONVERT(varchar, @@NESTLEVEL);


 print '����������          : '+ cast(round(12345.12345, 2) as varchar(12)); 
 print '������ �����        : '+ cast(floor(24.5) as varchar(12));
 print '���������� � �������: '+ cast(power(12.0, 2) as varchar(12)); 
 print '��������            : '+ cast(log(144.0) as varchar(12));
 print '������ ����������   : '+ cast(sqrt(144.0) as varchar(12));
 print '���������           : '+ cast(exp(4.96981) as varchar(12));
 print '���������� �������� : '+ cast(abs(-5) as varchar(12));
 print 'C����               : '+ cast(sin(pi()) as varchar(12));
 print '���������              : '+  substring('1234567890', 3,2);   
 print '������� ������� ������ : '+  rtrim('12345     ') +'X';
 print '������� ������� �����  : '+  'X'+ ltrim('     67890');
 print '������ �������         : '+  lower ('������� �������');
 print '������� �������        : '+  upper ('������ �������');
 print '��������               : '+  replace('1234512345', '5', 'X');
 print '������ ��������        : '+  'X'+ space(5) +'X';
 print '��������� ������       : '+  replicate('12', 5);
 print '����� �� �������       : '+  cast (patindex ('%Y_Y%', '123456YxY7890') as varchar(5));
 DECLARE @time time(7) = SYSDATETIME(), @dt datetime = getdate();
 print '������� �����       : '+  convert (varchar(12), @time);
 print '������� ����        : '+  convert (varchar(12), @dt, 103);
 print '+1 ����               : '+ convert(varchar(12), dateadd(d, 1, @dt), 103);

-- ������� 4.1 (���������� z)
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

-- ������� 4.2 (���������� ���)
DECLARE 
    @fullName VARCHAR(100) = '�������� ������ ������������',
    @shortName VARCHAR(50);

SET @shortName = 
    LEFT(@fullName, CHARINDEX(' ', @fullName)) + ' ' +
    SUBSTRING(@fullName, CHARINDEX(' ', @fullName) + 1, 1) + '. ' +
    SUBSTRING(@fullName, CHARINDEX(' ', @fullName, CHARINDEX(' ', @fullName) + 1) + 1, 1) + '.';

PRINT '������ ���: ' + @fullName;
PRINT '����������� ���: ' + @shortName;
-- ������� 4.3
CREATE TABLE #�������� (
    ID_�������� INT IDENTITY PRIMARY KEY,
    ������� NVARCHAR(50) NOT NULL,
    ��� NVARCHAR(50) NOT NULL,
    ����_�������� DATE NOT NULL,
    ������ NVARCHAR(20) NOT NULL
);

-- ������� �������� ������
INSERT INTO #�������� (�������, ���, ����_��������, ������)
VALUES 
    ('������', '�������', '2003-05-15', '������-101'),
    ('�������', '�����', '2004-12-22', '������-102'),
    ('�������', '����', '2002-02-10', '������-101');

-- ����� ���������, � ������� ���� �������� � ��������� ������
DECLARE @currentDate DATE = GETDATE();
DECLARE @nextMonth INT = MONTH(DATEADD(MONTH, 1, @currentDate));

SELECT 
    �������,
    ���,
    ����_��������,
    -- ������ ��������
    DATEDIFF(YEAR, ����_��������, @currentDate) - 
    CASE 
        WHEN DATEADD(YEAR, DATEDIFF(YEAR, ����_��������, @currentDate), ����_��������) > @currentDate 
        THEN 1 
        ELSE 0 
    END AS �������
FROM 
    #��������
WHERE 
    MONTH(����_��������) = @nextMonth;
	DROP TABLE #��������;
	-- ������� 4.4
CREATE TABLE #�������� (
    ID_�������� INT IDENTITY PRIMARY KEY,
    ID_�������� INT NOT NULL,
    ��������_�������� NVARCHAR(100) NOT NULL,
    ����_�������� DATE NOT NULL,
    ������ NVARCHAR(20) NOT NULL
);

-- ������� �������� ������
INSERT INTO #�������� (ID_��������, ��������_��������, ����_��������, ������)
VALUES 
    (1, '���� ������', '2023-11-20', '������-101'),
    (2, '���� ������', '2023-11-20', '������-102'),
    (3, '����������������', '2023-11-25', '������-101');

-- ����� ��� ������ ��� �������� �� �� � �������� ������
DECLARE @targetGroup NVARCHAR(20) = '������-101';

SELECT DISTINCT
    DATENAME(WEEKDAY, ����_��������) AS ����_������
FROM 
    #��������
WHERE 
    ��������_�������� = '���� ������' 
    AND ������ = @targetGroup;
	DROP TABLE #��������;
-- ������� 5 
DECLARE 
    @totalOrders INT,
    @message NVARCHAR(100),
    @avgQuantity DECIMAL(10,2),
    @highValueOrders INT;

SET @totalOrders = (SELECT COUNT(*) FROM ������);

IF @totalOrders > 2
BEGIN
    SET @avgQuantity = (SELECT AVG(����������_�����������) FROM ������);
    SET @highValueOrders = (SELECT COUNT(*) FROM ������ WHERE ����������_����������� > @avgQuantity);

    SET @message = '������ �������: 
        - ������� ����������: ' + CAST(@avgQuantity AS NVARCHAR) + 
        ', 
        - ������� ���� ��������: ' + CAST(@highValueOrders AS NVARCHAR);
END
ELSE
    SET @message = '������������ ������ ��� �������. ����� �������: ' + CAST(@totalOrders AS NVARCHAR);

PRINT @message;

-- ������� 6 (������ ���)
SELECT 
    CASE 
        WHEN ���� BETWEEN 0 AND 1000 THEN '������'
        WHEN ���� BETWEEN 1001 AND 5000 THEN '���������'
        WHEN ���� BETWEEN 5001 AND 10000 THEN '������'
        ELSE '����� ������'
    END AS [��������� ����],
    COUNT(*) AS [���������� �������]
FROM ������
GROUP BY 
    CASE 
        WHEN ���� BETWEEN 0 AND 1000 THEN '������'
        WHEN ���� BETWEEN 1001 AND 5000 THEN '���������'
        WHEN ���� BETWEEN 5001 AND 10000 THEN '������'
        ELSE '����� ������'
    END;

-- ������� 7 (��������� �������)
CREATE TABLE #TempTable (
    id INT,
    field VARCHAR(50)
);

DECLARE @iterator INT = 0;

WHILE @iterator < 10
BEGIN
    INSERT INTO #TempTable (id, field) 
    VALUES (@iterator, REPLICATE('������', @iterator));
    SET @iterator += 1;
END;

SELECT * FROM #TempTable;
DROP TABLE #TempTable;

-- ������� 8 (RETURN)
DECLARE @var INT = 4;

PRINT '��������: ' + CAST(@var AS VARCHAR);
PRINT '�������� + 1: ' + CAST(@var + 1 AS VARCHAR);
RETURN;
PRINT '���� ��� �� ����������';

-- ������� 9 (TRY...CATCH)
BEGIN TRY
    CREATE TABLE #PEOPLES (
        FirstName NVARCHAR(50) NOT NULL,
        Age INT NOT NULL
    );

    INSERT INTO #PEOPLES VALUES (NULL, NULL); -- ���������� ������
END TRY
BEGIN CATCH
    PRINT 
        '������ ' + CAST(ERROR_NUMBER() AS VARCHAR) + ': ' + 
        ERROR_MESSAGE() + ' (������: ' + CAST(ERROR_LINE() AS VARCHAR) + ')';
END CATCH;