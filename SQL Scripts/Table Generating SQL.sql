DROP TABLE IF EXISTS ReturnItems;
DROP TABLE IF EXISTS Refunds;
DROP TABLE IF EXISTS Returns;
DROP TABLE IF EXISTS TransactionItems;
DROP TABLE IF EXISTS Items;
DROP TABLE IF EXISTS Transactions;
DROP TABLE IF EXISTS Profile;
DROP TABLE IF EXISTS Address;

-- Create the Address table
CREATE TABLE Address
(
    pkAddressId INT PRIMARY KEY IDENTITY,
    unit INT,
	address VARCHAR(100) NOT NULL,
	city VARCHAR(100) NOT NULL,
	province VARCHAR(2) NOT NULL,
	postalCode VARCHAR(6) NOT NULL,
);

-- Create the Profile table
CREATE TABLE Profile
(
    pkEmail VARCHAR(100) PRIMARY KEY,
	fkMailingAddressId INT FOREIGN KEY REFERENCES Address (pkAddressId) NOT NULL,
	fkShippingAddressId INT FOREIGN KEY REFERENCES Address (pkAddressId),
	firstName VARCHAR(100) NOT NULL,
	lastName VARCHAR(100) NOT NULL,
	phone BIGINT,
	isAdmin CHAR(1) NOT NULL,
);

-- Create the Transactions table
CREATE TABLE Transactions
(
    pkTransactionId INT PRIMARY KEY IDENTITY,
	fkEmail VARCHAR(100) FOREIGN KEY REFERENCES Profile (pkEmail) NOT NULL,
	status VARCHAR (100) NOT NULL,
	purchaseDate DATE NOT NULL,
	shippingMethod VARCHAR(100) NOT NULL,
);

-- Create the Items table
CREATE TABLE Items
(
    pkItemId INT PRIMARY KEY IDENTITY,
	supplier VARCHAR (100) NOT NULL,
	itemName VARCHAR (100) NOT NULL,
	itemDetails VARCHAR (1000) NOT NULL,
	price FLOAT NOT NULL,
	stock INT NOT NULL,
	category VARCHAR (50) NOT NULL,
	weight FLOAT NOT NULL,
	size VARCHAR (50),
	colour VARCHAR (50),
);

-- Create the TransactionItems table
CREATE TABLE TransactionItems
(
	fkTransactionId INT FOREIGN KEY REFERENCES Transactions (pkTransactionId) NOT NULL,
	fkItemId INT FOREIGN KEY REFERENCES Items (pkItemId) NOT NULL,
	quantity INT NOT NULL,
);

-- Create the Returns table
CREATE TABLE Returns
(
    pkReturnId INT PRIMARY KEY IDENTITY,
	fkTransactionId INT FOREIGN KEY REFERENCES Transactions (pkTransactionId) NOT NULL,
	reasonForReturn VARCHAR (500) NOT NULL,
	date DATE NOT NULL,
	status VARCHAR(100) NOT NULL,
);

-- Create the Refunds table
CREATE TABLE Refunds
(
	fkReturnId INT FOREIGN KEY REFERENCES Returns (pkReturnId) NOT NULL,
	amount INT NOT NULL,
	status VARCHAR(100) NOT NULL,
);

-- Create the ReturnItems table
CREATE TABLE ReturnItems
(
	fkReturnId INT FOREIGN KEY REFERENCES Returns (pkReturnId) NOT NULL,
	fkItemId INT FOREIGN KEY REFERENCES Items (pkItemId) NOT NULL,
	quantity INT NOT NULL,
);