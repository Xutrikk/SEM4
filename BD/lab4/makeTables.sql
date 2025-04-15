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
/*UPDATE �������
SET �������_������ = 1
WHERE ID_������� IN (
    SELECT ID_������� FROM ������
    GROUP BY ID_�������
    HAVING SUM(�����_���������) > 50000
);

UPDATE ������
SET �����_��������� = �����_��������� * 0.98
WHERE ID_������� IN (
    SELECT ID_������� FROM ������� WHERE �������_������ = 1
);
ALTER TABLE �������
ADD ����_����������� DATE DEFAULT GETDATE();
GO

ALTER TABLE ������
ADD CONSTRAINT CK_���� CHECK (���� >= 0);
ALTER TABLE �������
ADD CONSTRAINT UQ_������� UNIQUE (�������);

ALTER TABLE ������� DROP CONSTRAINT DF__�������__����_��__01142BA1;
ALTER TABLE �������
DROP COLUMN ����_�����������;
GO

SELECT * FROM �������;
SELECT �������, ������� FROM �������;
SELECT COUNT(*) AS ����������_������� FROM ������;
UPDATE ������
SET ���� = 5000
WHERE �������� = '���� Razer';
SELECT * FROM ������ WHERE �������� = '���� Razer';*/
/*lab4*/
SELECT 
    �.ID_������ AS ���_������,
    �.�������� AS ���_������
FROM ������ �
INNER JOIN ������ � ON �.ID_������ = �.ID_������;

SELECT 
    �.ID_������ AS ���_������,
    �.�������� AS ���_������
FROM ������ �
INNER JOIN ������ � ON �.ID_������ = �.ID_������
WHERE �.�������� LIKE '%�������%';

SELECT 
    �.������� + ' ' + �.��� AS ���_�������,
    �.�������� AS �����,
    CASE 
        WHEN �.����������_����������� = 1 THEN '����'
        WHEN �.����������_����������� = 2 THEN '���'
        ELSE CAST(�.����������_����������� AS NVARCHAR) + ' ��'
    END AS ����������_��������
FROM ������� �
INNER JOIN ������ � ON �.ID_������� = �.ID_�������
INNER JOIN ������ � ON �.ID_������ = �.ID_������
ORDER BY �.����������_����������� DESC;


SELECT 
    �.������� + ' ' + �.��� AS ������,
    ISNULL(CAST(�.ID_������ AS NVARCHAR(20)), '***') AS �����
FROM ������� �
LEFT JOIN ������ � ON �.ID_������� = �.ID_�������;


SELECT  
    �.ID_������, 
    �.�������� AS �����_���_�������
FROM ������ �
FULL OUTER JOIN ������ � ON �.ID_������ = �.ID_������
WHERE �.ID_������ IS NULL;

SELECT 
    �.ID_������, 
    �.����������_����������� AS �����_���_������
FROM ������ �
FULL OUTER JOIN ������ � ON �.ID_������ = �.ID_������
WHERE �.ID_������ IS NULL;

SELECT 
    �.ID_������, 
    �.�������� AS �����, 
    �.ID_������, 
    �.����������_�����������
FROM 
������ � FULL OUTER JOIN ������ � ON �.ID_������ = �.ID_������;

SELECT 
    �.ID_������, 
    �.����������_�����������,
    �.ID_������, 
    �.�������� AS �����
FROM ������ �
FULL OUTER JOIN ������ � ON �.ID_������ = �.ID_������;


SELECT 
    �.ID_������ AS ���_������,
    �.�������� AS ���_������
FROM ������ �
CROSS JOIN ������ �
WHERE �.ID_������ = �.ID_������;