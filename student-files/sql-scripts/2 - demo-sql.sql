-- CRUD operations

/*
C - Create | INSERT INTO
R - Read | SELECT
U - Update | UPDATE
D - Delete | DELETE
*/

--
INSERT INTO Users (FirstName, LastName, IdCard, TaxNumber)
VALUES ('Bruno', 'Silva', '123456', '12345');

INSERT INTO Users (FirstName, LastName, IdCard, TaxNumber)
VALUES ('Bruno', 'Silva', '123455', '123456'),
('André', 'Cruz', '123457', '123457'),
('Edson', 'Manuel', '123458', '123458'),
('Gonçalo', 'Ferreira', '123459', '123459');

SELECT FirstName, Id, LastName FROM Users;

INSERT INTO Accounts (Username, [Password], UserId)
VALUES ('besilva', '12345', 1);



SELECT TOP 2 FirstName AS Name, LastName As Surname FROM Users
-- WHERE FirstName LIKE '%r%'
ORDER BY FirstName ASC;
--

UPDATE Users SET FirstName = 'Bruno', LastName = 'Manuel'
WHERE Id = 2;

UPDATE Accounts SET Username = 'bmanuel'
WHERE UserId = 1;


SELECT * FROM Accounts
WHERE Id = 1;

DELETE FROM Users
WHERE Id = 1;

SELECT * FROM Users;
SELECT * FROM Accounts;

SELECT u.FirstName From Users as u
JOIN Accounts as a ON u.Id = a.UserId
WHERE a.Username = 'bmsilva' AND a.Password = '12345';

EXEC firstNameBasedOnLogin('bmsivla', '12345');

-- Insert Username: 
-- Insert password: 
SELECT UserId FROM Accounts
WHERE Username = 'bmsilva' AND Password = '12345';


-- Olá, [FirstName]
SELECT Firstname FROM Users
WHERE Id = 2;


