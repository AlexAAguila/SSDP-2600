-- Insert fake data into Address table
INSERT INTO Address (unit, address, city, province, postalCode)
VALUES 
(101, '123 Main St', 'New York City', 'NY', '10001'),
(NULL, '456 Elm St', 'Los Angeles', 'CA', '90001'),
(202, '789 Oak St', 'Chicago', 'IL', '60601');

-- Insert fake data into Profile table
INSERT INTO Profile (pkEmail, fkMailingAddressId, fkShippingAddressId, firstName, lastName, phone, isAdmin)
VALUES 
('user1@example.com', 1, 2, 'John', 'Doe', 1234567890, 'N'),
('admin@example.com', 3, NULL, 'Admin', 'User', 9876543210, 'Y');

-- Insert fake data into Transactions table
INSERT INTO Transactions (fkEmail, status, purchaseDate, shippingMethod)
VALUES 
('user1@example.com', 'Completed', '2023-01-15', 'Standard'),
('user1@example.com', 'Pending', '2023-02-20', 'Express'),
('user1@example.com', 'Completed', '2023-03-10', 'Standard');

-- Insert fake data into Items table
INSERT INTO Items (supplier, itemName, itemDetails, price, stock, category, weight, size, colour)
VALUES 
('Supplier A', 'Widget 1', 'Description for Widget 1', 29.99, 50, 'Electronics', 0.5, 'Medium', 'Blue'),
('Supplier B', 'Gadget 2', 'Description for Gadget 2', 49.99, 30, 'Gadgets', 0.3, 'Small', 'Red'),
('Supplier C', 'Tool 3', 'Description for Tool 3', 19.99, 20, 'Tools', 1.0, 'Large', 'Green');

-- Insert fake data into TransactionItems table
INSERT INTO TransactionItems (fkTransactionId, fkItemId, quantity)
VALUES 
(1, 1, 2),
(1, 2, 1),
(2, 3, 3),
(3, 1, 1),
(3, 2, 2);

-- Insert fake data into Returns table
INSERT INTO Returns (fkTransactionId, reasonForReturn, date, status)
VALUES 
(1, 'Defective product', '2023-02-01', 'Processing'),
(3, 'Wrong size', '2023-03-20', 'Received');

-- Insert fake data into Refunds table
INSERT INTO Refunds (fkReturnId, amount, status)
VALUES 
(1, 59.98, 'Issued'),
(2, 99.98, 'Pending');

-- Insert fake data into ReturnItems table
INSERT INTO ReturnItems (fkReturnId, fkItemId, quantity)
VALUES 
(1, 1, 1),
(2, 3, 2);
