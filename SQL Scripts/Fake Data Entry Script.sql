-- Insert data into the Address table
INSERT INTO Address (unit, address, city, province, postalCode)
VALUES 
(101, '123 Main St', 'Anytown', 'AB', 'A1B2C3'),
(202, '456 Elm St', 'Sometown', 'BC', 'X1Y2Z3'),
(303, '789 Oak St', 'Othertown', 'ON', 'M4N5P6');

-- Insert data into the ContactInfo table
INSERT INTO ContactInfo (fkMailingAddressId, fkShippingAddressId, phone)
VALUES 
(1, 1, 1234567890),
(2, 3, 2345678901),
(3, NULL, 3456789012);

-- Insert data into the Transactions table
INSERT INTO Transactions (status, purchaseDate, shippingMethod)
VALUES 
('Completed', '2024-02-10', 'Standard'),
('Pending', '2024-02-12', 'Express'),
('Completed', '2024-02-13', 'Standard');

-- Insert data into the Items table
INSERT INTO Items (supplier, itemName, itemDetails, price, stock, category, weight, size, colour)
VALUES 
('Outdoor Supplier X', 'Camping Tent', 'A durable camping tent suitable for 2 people', 149.99, 30, 'Tents', 5.0, 'Medium', 'Green'),
('Adventure Gear Co.', 'Sleeping Bag', 'A cozy sleeping bag with synthetic insulation', 79.99, 50, 'Sleeping Bags', 3.0, 'One Size', 'Blue'),
('Backpacks R Us', 'Hiking Backpack', 'A sturdy hiking backpack with multiple compartments', 99.99, 40, 'Backpacks', 2.5, 'Medium', 'Black');

-- Insert data into the TransactionItems table
INSERT INTO TransactionItems (fkTransactionId, fkItemId, quantity)
VALUES 
(1, 1, 2),
(1, 2, 1),
(2, 3, 3);

-- Insert data into the Returns table
INSERT INTO Returns (fkTransactionId, reasonForReturn, date, status)
VALUES 
(1, 'Wrong size', '2024-02-11', 'Pending'),
(3, 'Defective', '2024-02-14', 'Processing');

-- Insert data into the Refunds table
INSERT INTO Refunds (fkReturnId, amount, status)
VALUES 
(1, 20, 'Pending'),
(2, 30, 'Processing');

-- Insert data into the ReturnItems table
INSERT INTO ReturnItems (fkReturnId, fkItemId, quantity)
VALUES 
(1, 1, 1),
(2, 3, 2);
