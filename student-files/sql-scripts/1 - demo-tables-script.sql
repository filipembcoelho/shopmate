CREATE TABLE dbo.Users
(
    Id          INT IDENTITY(1,1) NOT NULL,
    FirstName   NVARCHAR(100) NOT NULL,
    LastName    NVARCHAR(100) NOT NULL,
    IdCard      NVARCHAR(20) NOT NULL,
    TaxNumber   NVARCHAR(20) NOT NULL,

    CONSTRAINT PK_Users
        PRIMARY KEY (Id),

    CONSTRAINT UQ_Users_IdCard
        UNIQUE (IdCard)
);
GO

CREATE TABLE dbo.Accounts
(
    Id          INT IDENTITY(1,1) NOT NULL,
    Username    NVARCHAR(100) NOT NULL,
    [Password]  NVARCHAR(255) NOT NULL,
    UserId      INT NOT NULL,

    CONSTRAINT PK_Accounts
        PRIMARY KEY (Id),

    CONSTRAINT UQ_Accounts_Username
        UNIQUE (Username),

    CONSTRAINT UQ_Accounts_UserId
        UNIQUE (UserId),

    CONSTRAINT FK_Accounts_Users
        FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id)
        ON DELETE CASCADE
);
GO

CREATE TABLE dbo.Contacts
(
    Id          INT IDENTITY(1,1) NOT NULL,
    [Value]     NVARCHAR(255) NOT NULL,
    [Type]      NVARCHAR(20) NOT NULL,
    UserId      INT NOT NULL,

    CONSTRAINT PK_Contacts
        PRIMARY KEY (Id),

    CONSTRAINT CK_Contacts_Type
        CHECK ([Type] IN ('Phone', 'Email')),

    CONSTRAINT FK_Contacts_Users
        FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id)
        ON DELETE CASCADE
);
GO

CREATE INDEX IX_Contacts_UserId
    ON dbo.Contacts (UserId);
GO

CREATE TABLE dbo.Addresses
(
    Id          INT IDENTITY(1,1) NOT NULL,
    StreetOne   NVARCHAR(200) NOT NULL,
    StreetTwo   NVARCHAR(200) NULL,
    City        NVARCHAR(100) NOT NULL,
    ZipCode     NVARCHAR(20) NULL,
    Country     NVARCHAR(100) NOT NULL,
    [Type]      NVARCHAR(20) NOT NULL,

    CONSTRAINT PK_Addresses
        PRIMARY KEY (Id),

    CONSTRAINT CK_Addresses_Type
        CHECK ([Type] IN ('Billing', 'Shipping'))
);
GO

CREATE TABLE dbo.UserAddresses
(
    UserId      INT NOT NULL,
    AddressId   INT NOT NULL,

    CONSTRAINT PK_UserAddresses
        PRIMARY KEY (UserId, AddressId),

    CONSTRAINT FK_UserAddresses_Users
        FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id)
        ON DELETE CASCADE,

    CONSTRAINT FK_UserAddresses_Addresses
        FOREIGN KEY (AddressId)
        REFERENCES dbo.Addresses (Id)
        ON DELETE CASCADE
);
GO

CREATE INDEX IX_UserAddresses_AddressId
    ON dbo.UserAddresses (AddressId);
GO