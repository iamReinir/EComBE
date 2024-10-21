-- Insert mock data into Categories table
INSERT INTO Categories (CategoryId, CreatedAt, Description, IsDeleted, Name, ParentCategoryId, UpdatedAt)
VALUES
    ('CAT001', '2024-08-01 09:00:00', 'Laptops and notebooks category', 0, 'Laptops', NULL, '2024-10-01 09:00:00'),
    ('CAT002', '2024-07-15 11:00:00', 'Peripherals including mice, keyboards, etc.', 0, 'Peripherals', NULL, '2024-10-01 11:00:00'),
    ('CAT003', '2024-07-20 12:30:00', 'Accessories for laptops and PCs', 0, 'Accessories', 'CAT002', '2024-10-05 12:30:00'),
    ('CAT004', '2024-08-10 10:45:00', 'Monitors and display devices', 0, 'Monitors', NULL, '2024-09-15 10:45:00'),
    ('CAT005', '2024-09-05 14:00:00', 'Headphones and audio devices', 0, 'Audio', 'CAT003', '2024-10-10 14:00:00');
   
   
   -- Insert more mock data into Products table
INSERT INTO Products (CategoryId, CreatedAt, Description, ImageUrl, IsDeleted, Name, Price, ProductId, QuantityAvailable, UpdatedAt)
VALUES
    ('CAT001', '2024-08-21 13:20:00', 'Ultra-lightweight business laptop', 'https://picsum.photos/200', 0, 'Business Laptop', 1099.99, 'PROD006', 20, '2024-10-16 13:20:00'),
    ('CAT001', '2024-07-14 15:50:00', 'Gaming laptop with RTX 3080', 'https://picsum.photos/200', 0, 'Gaming Laptop RTX', 1999.99, 'PROD007', 12, '2024-10-12 15:50:00'),
    ('CAT002', '2024-09-25 09:10:00', 'RGB wireless gaming mouse', 'https://picsum.photos/200', 0, 'RGB Gaming Mouse', 79.99, 'PROD008', 60, '2024-10-10 09:10:00'),
    ('CAT003', '2024-07-18 11:30:00', 'Gaming keyboard with anti-ghosting feature', 'https://picsum.photos/200', 0, 'Gaming Keyboard', 139.99, 'PROD009', 40, '2024-10-11 11:30:00'),
    ('CAT004', '2024-10-01 16:25:00', '4K UHD 32-inch curved monitor', 'https://picsum.photos/200', 0, '4K Curved Monitor', 499.99, 'PROD010', 10, '2024-10-16 16:25:00'),
    ('CAT005', '2024-09-29 12:40:00', 'Over-ear gaming headphones with surround sound', 'https://picsum.photos/200', 0, 'Surround Sound Headphones', 159.99, 'PROD011', 55, '2024-10-14 12:40:00'),
    ('CAT002', '2024-09-12 13:15:00', 'Wireless keyboard and mouse combo', 'https://picsum.photos/200', 0, 'Wireless Keyboard & Mouse', 89.99, 'PROD012', 80, '2024-10-10 13:15:00'),
    ('CAT001', '2024-08-05 14:00:00', 'Touchscreen convertible laptop', 'https://picsum.photos/200', 0, 'Convertible Laptop', 1199.99, 'PROD013', 18, '2024-10-08 14:00:00'),
    ('CAT004', '2024-07-30 10:45:00', '144Hz gaming monitor with G-Sync', 'https://picsum.photos/200', 0, '144Hz Gaming Monitor', 349.99, 'PROD014', 22, '2024-10-06 10:45:00'),
    ('CAT005', '2024-09-18 17:00:00', 'Wireless earbuds with active noise canceling', 'https://picsum.photos/200', 0, 'Noise-Canceling Earbuds', 99.99, 'PROD015', 150, '2024-10-12 17:00:00');
	
	
INSERT INTO Categories (CategoryId, CreatedAt, Description, IsDeleted, Name, ParentCategoryId, UpdatedAt,ImageUrl)
VALUES
    ('CAT006', '2024-08-01 09:00:00', 'Desktop Personal Computer', 0, 'Computers', NULL, '2024-10-21 09:00:00','http://reinir.mooo.com:5002/uploaded_image_1729478179.png'),
    ('CAT007', '2024-07-15 11:00:00', 'Gaming devices like playstations, psps, switch', 0, 'GamingDevices', NULL, '2024-10-21 11:00:00','http://reinir.mooo.com:5002/uploaded_image_1729478180.png'),
    ('CAT008', '2024-07-20 12:30:00', 'Smartphones devices', 0, 'Smartphones', null, '2024-10-21 12:30:00','http://reinir.mooo.com:5002/uploaded_image_1729478183.png'),
    ('CAT009', '2024-08-10 10:45:00', 'Tablets', 0, 'Tablets', NULL, '2024-10-21 10:45:00','http://reinir.mooo.com:5002/uploaded_image_1729478184.png'),    

UPDATE Categories SET ImageUrl ='http://reinir.mooo.com:5002/uploaded_image_1729478182.png'
WHERE CategoryId = 'CAT001';

UPDATE Categories SET ImageUrl ='http://reinir.mooo.com:5002/uploaded_image_1729478974.png'
WHERE CategoryId = 'CAT002';

UPDATE Categories SET IsDeleted='1' WHERE CategoryId = 'CAT003';

UPDATE Categories SET ImageUrl ='http://reinir.mooo.com:5002/uploaded_image_1729478185.png'
WHERE CategoryId = 'CAT004';

UPDATE Categories SET ImageUrl ='http://reinir.mooo.com:5002/uploaded_image_1729478181.png'
WHERE CategoryId = 'CAT005';

INSERT INTO WishLists (UserId,ProductId,IsDeleted,CreatedAt,UpdatedAt)
VALUES 
	('efa70cbd-daea-48b6-a8d6-aeaf46cb5273','PROD007','0',NOW(),NOW()),
	('efa70cbd-daea-48b6-a8d6-aeaf46cb5273','PROD008','0',NOW(),NOW()),
	('efa70cbd-daea-48b6-a8d6-aeaf46cb5273','PROD009','0',NOW(),NOW()),
	('efa70cbd-daea-48b6-a8d6-aeaf46cb5273','PROD010','0',NOW(),NOW()),
	('efa70cbd-daea-48b6-a8d6-aeaf46cb5273','PROD011','0',NOW(),NOW());
