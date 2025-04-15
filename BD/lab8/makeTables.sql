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
/*lab8*/
/*1*/
CREATE VIEW [�������_������] AS
SELECT 
    ID_������� AS [���],
    ������� + ' ' + ��� AS [��� �������],
    ������� AS [�������]
FROM �������;
GO
/*2*/
CREATE VIEW [����������_�������_��_����������] AS
SELECT 
    �������� AS [���������],
    COUNT(ID_������) AS [���������� �������]
FROM ������
GROUP BY ��������;
GO
/*3*/
CREATE VIEW [������_�������_�����] 
    (��������, ����, �������_���������)
AS
SELECT 
    ��������,
    ����,
    �������_���������
FROM ������
WHERE �������� LIKE '%�������%'; 
GO

-- ��������:
SELECT * FROM [������_�������_�����];
/*4*/
CREATE VIEW [������_�������] 
    (��������, ����, �������_���������)
AS
SELECT 
    ��������,
    ����,
    �������_���������
FROM ������
WHERE ���� > 10000
WITH CHECK OPTION; 
GO

SELECT * FROM [������_�������];
INSERT INTO [������_�������] VALUES ('��������� �����', 500.00, '��');
/*5*/
CREATE VIEW [������_�������] 
    (��������, ����, �������_���������)
AS
SELECT TOP 100 PERCENT
    ��������,
    ����,
    �������_���������
FROM ������
ORDER BY ��������;
GO
/*6*/
ALTER VIEW [����������_�������_��_����������] WITH SCHEMABINDING
AS
SELECT 
    �������_��������� AS [���������],
    COUNT_BIG(*) AS [���������� �������]
FROM dbo.������
GROUP BY �������_���������;
GO
ALTER TABLE ������ DROP COLUMN �������_���������;