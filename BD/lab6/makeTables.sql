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
/*lab6*/
/*1*/
SELECT 
    t.�������� AS ������������_������,
    MAX(z.����������_�����������) AS ������������_����������,
    MIN(z.����������_�����������) AS �����������_����������,
    AVG(z.����������_�����������) AS �������_����������,
    SUM(z.����������_�����������) AS ���������_����������,
    COUNT(*) AS ����������_�������
FROM 
    ������ t
INNER JOIN 
    ������ z ON t.ID_������ = z.ID_������
GROUP BY 
    t.��������;
	/*2*/
	SELECT * FROM (
    SELECT 
        CASE 
            WHEN ����������_����������� BETWEEN 1 AND 5 THEN '1-5'
            WHEN ����������_����������� BETWEEN 6 AND 10 THEN '6-10'
            ELSE '����� 10'
        END AS ��������_����������,
        COUNT(*) AS ����������_�������
    FROM 
        ������
    GROUP BY 
        CASE 
            WHEN ����������_����������� BETWEEN 1 AND 5 THEN '1-5'
            WHEN ����������_����������� BETWEEN 6 AND 10 THEN '6-10'
            ELSE '����� 10'
        END
) AS T
ORDER BY 
    CASE ��������_����������
        WHEN '����� 10' THEN 1
        WHEN '6-10' THEN 2
        WHEN '1-5' THEN 3
        ELSE 4
    END;
	/*3*/
SELECT 
    k.�������,
    k.���,
    CAST(ROUND(AVG(t.����), 2) AS DECIMAL(10,2)) AS �������_����
FROM 
    ������� k
INNER JOIN 
    ������ z ON k.ID_������� = z.ID_�������
INNER JOIN 
    ������ t ON z.ID_������ = t.ID_������
GROUP BY 
    k.�������, k.���
ORDER BY 
    �������_���� DESC;

/*4*/
SELECT 
    k.�������,
    k.���,
    CAST(ROUND(AVG(t.����), 2) AS DECIMAL(10,2)) AS �������_����
FROM 
    ������� k
INNER JOIN 
    ������ z ON k.ID_������� = z.ID_�������
INNER JOIN 
    ������ t ON z.ID_������ = t.ID_������
WHERE 
    t.ID_������ IN (1, 2)
GROUP BY 
    k.�������, k.���
ORDER BY 
    �������_���� DESC;

/*5*/
SELECT 
    t.�������� AS �����,
    k.�����,
    CAST(ROUND(AVG(t.����), 2) AS DECIMAL(10,2)) AS �������_����
FROM 
    ������ t
INNER JOIN 
    ������ z ON t.ID_������ = z.ID_������
INNER JOIN 
    ������� k ON z.ID_������� = k.ID_�������
WHERE 
    k.����� LIKE '%������%' 
GROUP BY 
    t.��������, k.�����;

	/*6*/
	SELECT 
    t.�������� AS �����,
    COUNT(*) AS ����������_�������
FROM 
    ������ t
INNER JOIN 
    ������ z ON t.ID_������ = z.ID_������
WHERE 
    z.����������_����������� IN (1, 2) 
GROUP BY 
    t.��������
HAVING 
    COUNT(*) > 0
ORDER BY 
    ����������_������� DESC;