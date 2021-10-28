drop database if exists LoginDB;

create database LoginDB;

use LoginDB;


create table Cashiers
(
	cashier_ID int auto_increment primary key,
    cashier_name varchar(50) not null,
    user_name varchar(50) not null unique,
    user_pass varchar(50) not null,
    phone varchar(10),
    email varchar(50) unique
);

create table Customers
(
	customer_ID int auto_increment primary key,
    customer_name varchar(100) not null,
    customer_phone char(10) unique not null,
    customer_address varchar(200)
);

create table Brands
(
	brand_ID int primary key,
    brand_name varchar(20) unique not null,
    brand_description varchar(200) 
);

create table Perfumes
(
	perfume_ID int auto_increment primary key,
    perfume_name varchar(30) not null,
    fragrance_family varchar(50) not null,
    classification varchar(50) not null,
    volume varchar(10) not null,
    top_notes varchar(100) not null,
    heart_notes varchar(100) not null,
    base_notes varchar(100) not null,
    gender varchar(10) check (gender in ('Men', 'Women', 'Unisex')),
    ingredients varchar(500) not null,
    form varchar(10) not null,
    year_launched char(4) not null,
    strength varchar(10) not null,
    origin varchar(15),
    price decimal(20,2) default 0 not null,
    total_quantity int not null default 0,
    product_status tinyint not null, 
    perfume_description varchar(500),
    brand_ID int not null,
    constraint fk_Perfumes_Brands foreign key(brand_ID) references Brands(brand_ID)
);

create table Invoices
(
	invoice_ID int auto_increment primary key,
    invoice_date datetime default now() not null,
    invoice_status tinyint not null,
    customer_ID int not null,
    constraint fk_Invoices_Customers foreign key(customer_ID) references Customers(customer_ID)
);

create table InvoiceLineItems
(
	invoice_ID int not null,
    perfume_ID int not null,
	unit_price decimal (20,2) not null,
    quantity int not null default 1,
    constraint pf_InvoiceLineItems primary key(invoice_ID, perfume_ID),
    constraint fk_InvoiceLineItems_Invoices foreign key(invoice_ID) references Invoices(invoice_ID),
	constraint fk_InvoiceLineItems_Perfumes foreign key(perfume_ID) references Perfumes(perfume_ID)
);

delimiter $$
create trigger tg_before_insert 
before insert on Perfumes 
for each row
    begin
		if new.total_quantity < 0 then
            signal sqlstate '45001' set message_text = 'tg_before_insert: total quantity must > 0';
        end if;
    end $$
delimiter ;

delimiter $$
create trigger tg_CheckAmount
before update on Perfumes
for each row
	begin
		if new.total_quantity < 0 then
            signal sqlstate '45001' set message_text = 'tg_CheckAmount: total quantity must > 0';
        end if;
    end $$
delimiter ;

delimiter $$
create procedure sp_createCustomer(IN customerName varchar(100), IN customerPhone char(10), IN customerAddress varchar(200), OUT customerID int)
begin
	insert into Customers(customer_name, customer_phone, customer_address) values (customerName, customerPhone, customerAddress); 
    select max(customer_ID) into customerId from Customers;
end $$
delimiter ;



insert into Cashiers(cashier_name, user_name, user_pass) values
('son', 'sondoannam', '7c3932b697fde8336f3524bcac760790'), -- 20052002@Zz
('hoang', 'lehuyhoang', 'dde10c489d420b93c30cb6486194d2c7'), -- PF15@VTCAcademy
('khoa', 'lamdinhkhoa', 'dde10c489d420b93c30cb6486194d2c7'); -- PF15@VTCAcademy
select * from Cashiers;

insert into Customers(customer_name, customer_phone, customer_address) values
('Nguyen Thanh Tung', '0975479078', 'Thai Binh'),
('Thieu Bao Tram', '0374553099', 'Ha Noi'),
('Ngo Ba Kha', '0376669777', 'Hai Phong'),
('Taylor Swift', '0519891988', 'Texas');
select * from Customers;

insert into Brands(brand_ID, brand_name, brand_description) values
(1, 'Versace', 'Gianni Versace S.r.l., usually referred to simply as Versace, is an Italian luxury fashion company and trade name founded by Gianni Versace in 1978'),
(2, 'Dolce & Gabbana', ''),
(3, 'Calvin Klein',''),
(4, 'Mont Blanc', ''),
(5, 'Christian Dior', ''),
(6, 'Jimmy Choo', '');
select * from Brands;

insert into Perfumes(perfume_name, fragrance_family, classification, volume, top_notes, heart_notes, base_notes, gender, ingredients, form, year_launched, strength, origin, price, total_quantity, product_status, perfume_description, brand_ID)
values
('Bright Crystal','Floral, Fruity and Musk', 'Eau De Toilette', '50ml', 'Yuzu, Pomegranate, Water Notes', 'Peory, Lotus, Magnolia', 'Musk, Mahogany, Amber', 'Women', 'Alcohol, Denat. (Sd Alcohol 39-C), Fragrance, Water, Butylphenyl, Methylpropional, Ethylhexyl Methoxycinnamate, Hydroxysohexyl 3- Cyclohexene Carboxaldehyde, Linalool, Citronellol, Ethylhexyl Salicylate, Butyl, Methoxydibenzoylmethane, Limonene, Geraniol, Ci 17200 (Red 33), Ci 15985 (Yellow 6)', 'Liquid', '2006', 'Medium', 'Italy', 39.73, 5, 1, 'Designed and created by the master perfumer Alberto Morillas for the highly beloved Italian designer label Versace. Inspired by the favorite floral mixes by the fashion-forward Donatella Versace herself, this is the scent that perfectly captures the essence of summer. Whether you are looking for a fragrance that reminds you of your favorite season or celebrating it, this was made for you', 1),
('Light Blue','Citrus','Eau De Toilette','50ml','Sicilian Lemon, Apple, Cedar, Bellflower','Bamboo, Jasmine, White Rose','Cedar, Musk, Amber','Women','Alcohol, Fragrance, Water, Limonene, Ethylhexyl Methoxycinnamate, Diethylamino Hydroxybenzoyl Hexyl Benzoate, Citral, Cinnamal, Linalool, Bht','Liquid','2001','Strong',' ', 46.56, 5, 1,'Dolce & Gabbana Light Blue for women is a fresh, vibrant perfume thats citric base makes it perfect for those sparkling warm summer days that lead to perfect summer evenings that seem to last forever', 2),
('Jimmy Choo','Floral and fruity', 'Eau De Toilette', '60ml', 'Pear, Mandarin Orange, Green Notes', 'Orchid', 'Toffee, Patchouli', 'Women', 'Alcohol Denat. (Sd Alcohol 39-C) · Parfum (Fragrance) · Aqua (Water) · Benzyl Salicylate · Butyl Methoxydibenzoylmethane · Ethylhexyl', 'Liquid', '2011', 'Regular', 'France', 31.5, 5, 1 ,'With Jimmy Choo Perfume, you’ll feel as confident and captivating as ever in this mesmerizing fragrance. In a divine infusion of fruity, floral, and gourmand accords, this unique fragrance is both fresh and decadent', 6),
('Eternity','Floral', 'Eau De Parfum', '50ml', 'Green Notes, Freesia, Sage, Citruses, Mandarin Orange', 'Carnation, Lily, Lily of the valley, Narcissus, Marigold, Violet, Rose, Jasmine', 'Heliotrope, Musk, Sandalwood, Amber, Patchouli', 'Women', ' ', 'Liquid', '1988', 'Strong', 'Spain', 24.34, 5, 1,'A truly timeless Eau De Parfum became one of the most beloved perfumes that became part of the Fragrance Hall of Fame. This exquisite scent has somehow captured the very essence of summer, making it a perfect choice for the season as it evokes heady blooms in hazy, pleasant sunlight', 3),
('Euphoria','Woody', 'Eau De Parfum', '50ml', 'Pomegranate, Raspberry, Passionfruit, Persimmon, Peach and Green Notes', 'Orchid, Lotus and Champaca', 'Mahogany, Amber, Musk, Violet, Patchouli and Vanilla', 'Women', ' ', 'Liquid', '2005', 'Strong', 'USA',27.93,5,1,'Euphoria by Calvin Klein is a confidently sensual floral oriental fragrance with a dark allure, combining intoxicating blooms and exotic fruits with amber and warm spice',3),
('Eternity Cologne', 'Floral', 'Eau De Toilette', '50ml', 'Lavender, Lemon, Bergamot, Mandarin Orange', 'Juniper Berries, Sage, Coriander, Geranium, Basil, Jasmine, Orange Blossom, Lily, lily of the valley', 'Sandalwood, Vetiver, Musk, Brazilian Rosewood, Amber', 'Men', ' ', 'Liquid', '1990', 'Strong', 'Spain', 22.20, 5, 1, 'Wear your confidence and charisma as you spritz the Calvin Klein Eternity Cologne EDT Spray on yourself! It brightly opens up with the zesty notes of Mandarin orange, lemon, and bergamot and the soothing lavender', 3),
('CK One', 'Citrus', 'Eau De Toilette', '50ml', 'Lemon, Green Notes, Bergamot, Pineapple, Mandarin Orange, Cardamom and Papaya', 'Lily-of-the-Valley, Jasmine, Violet, Nutmeg, Rose, Orris Root and Freesia', 'Green Accord, Musk, Cedar, Sandalwood, Oakmoss, Green Tea and Amber', 'Unisex', ' ', 'Liquid', '1994', 'Medium', ' ', 20.77, 5, 1, 'Ck One Cologne by Calvin Klein, Ck One has remained at the top of most must-have perfume lists since its introduction in 1994. A versatile fragrance for men and women, this aromatic chypre combines green and fruit notes to culminate in a fresh, clean scent', 3),
('CK One Shock For Him', 'Amber Spicy', 'Eau De Toilette', '100ml', 'Lavender, Clementine and Cucumber', 'Cardamom, Pepper, Basil and Osmanthus', 'Tobacco, Amber, Patchouli, Woodsy Notes and Musk', 'Men', ' ', 'Liquid', '2011', 'Medium', ' ', 20.75, 5, 1, 'Ck One Shock Cologne by Calvin Klein, You will be shocked to smell how refreshing Ck One Shock from Calvin Klein is. This cologne was first released in 2011. It is recommended for casual use, and it opens with notes of fresh cucumber and citrusy clementine', 3),
('Obsession', 'Amber Spicy', 'Eau De Parfum', '50ml', ' Vanilla, Basil, Bergamot, Mandarin Orange, Green Notes, Peach and Lemon', 'Spices, Sandalwood, Coriander, Oakmoss, Cedar, Orange Blossom, Jasmine and Rose', 'Amber, Incense, Vanilla, Civet, Musk and Vetiver', 'Women', ' ', 'Liquid', '1985', 'Strong', ' ', 20.75, 5, 1, 'Created for the woman who is fleeting like the wind, Obsession Perfume is a delicious, warm, and enveloping oriental fragrance with a spicy, sophisticated, and alluring profile', 3),
('Obsession Cologne', 'Amber Woody', 'Eau De Toilette', '75ml', 'Cinnamon, Lavender, Coriander, Mandarin Orange, Lime, Bergamot and Grapefruit', 'Myrhh, Nutmeg, Carnation, Brazilian Rosewood, Pine Tree, Sage, Jasmine and Red Berries', 'Amber, Vanilla, Sandalwood, Musk, Patchouli and Vetiver', 'Men', ' ', 'Liquid', '1986', 'Strong', '', 20.75, 5, 1, 'Obsession Cologne by Calvin Klein, A classically elegant, sensual men’s fragrance, Obsession is an enduring woody oriental enhanced by elements of spice, amber and aromatic herbs', 3),
('The One Cologne', 'Oriental', 'Eau De Toilette', '100ml', 'Grapefruit, Coriander and Basil', 'Ginger, Cardamom, Orange Blossom', 'Amber, Tobacco, Cedar', 'Men', 'Alcohol Denat., Parfum/Fragrance, Water/Aqua/Eau, Benzyl Benzoate, Benzyl Salicylate, Butylphenyl Methylpropional, Citronellol, Coumarin, Geraniol, Hydroxycitronellal, Limonene, Linalool, Butyl Methoxydibenzoylmethane, Ethylhexyl Methoxycinnamate, and Ethylhexyl Salicylate.', 'Liquid', '2015', 'Medium', 'France', 52.03, 5, 1, 'A daring men’s fragrance, the Dolce & Gabbana The One Cologne EDT Spray is a classic that men are sure to adore! The One Cologne Perfume opens up with the energizing notes of coriander, basil, and grapefruit giving way to the tantalizing heart notes of orange blossom, ginger, and cardamom.', 2),
('Legend Cologne','Sweet and Fruity', 'Eau De Toilette', '50ml', 'Lavender, Pineapple, Bergamot and Lemon Verbena', 'Red Apple, Dried Fruits, oak moss, Geranium, Coumarin and Rose', 'Tonka Bean and Sandalwood', 'Men', '', 'Liquid', '2011', 'Medium', '', 29.55, 5, 1, 'Montblanc Legend Cologne by Mont Blanc, Created by nose Olivier Pescheux, Montblanc Legend was designed to invoke confidence, courage, and authenticity in the wearer. This aromatic fougere opens with top notes of lavender, pineapple leaf, verbena, and bergamot.', 4),
('Sauvage','Fresh spicy', 'Eau De Parfum', '60ml', 'Calabrian bergamot and Pepper', 'Sichuan Pepper, Lavender, Pink Pepper, Vetiver, Patchouli, Geranium and elemi', 'Ambroxan, Cedar and Labdanum', 'Men', '', 'Liquid', '2015', 'Strong', 'France', 111.7, 5, 1, 'The Christian Dior Sauvage Cologne is a fragrance inspired by vast landscapes and wide-open spaces in the wilderness: picture an endless blue sky sprawled over a white-hot desert', 5),
('J"adore','Floral Fruity', 'Eau De Parfum', '50ml', 'Pear, Melon, Magnolia, Peach, Mandarin Orange and Bergamot', 'Jasmine, Lily-of-the-Valley, Tuberose, Freesia, Rose, Orchid, Plum and Violet', 'Musk, Vanilla, Blackberry and Cedar', 'Women', '', 'Liquid', '1999', 'Medium', 'France', 97.3, 5, 1, 'Jadore Perfume by Christian Dior, This glamorous, feminine fragrance is a beautiful blend of elegant florals and fresh fruits with a touch of woody and earthy notes', 5),
('Fahrenheit','Aromatic Fougere', 'Eau De Toilette', '50ml', 'Nutmeg Flower, Lavender, Cedar, Mandarin Orange, Chamomile, Hawthorn, Bergamot and Lemon', 'Violet Leaf, Nutmeg, Cedar, Sandalwood, Honeysuckle, Carnation, Jasmine and Lily-of-the-Valley', 'Leather, Vetiver, Musk, Amber, Patchouli and Tonka Bean', 'Men', '', 'Liquid', '1988', 'Strong', '', 64.85, 5, 1, 'Fahrenheit Cologne by Christian Dior, For those who are looking for a bold, complex scent, there is Fahrenheit cologne for men', 5),
('The One','Amber Floral', 'Eau De Parfum', '50ml', 'Peach, Litchi, Mandarin Orange and Bergamot', 'Lily, Plum, Jasmine and Lily-of-the-Valley', 'Vanilla, Amber, Musk and Vetiver', 'Women', '', 'Liquid', '2006', 'Medium', 'France', 49.73, 5, 1, 'The idea of romance seems as old as time, and yet, when we experience it for ourselves, it feels brand new. Similarly, The One Perfume by Dolce & Gabbana binds together scents old and new to create a concoction that is both achingly familiar and strikingly modern', 2),
('Dolce & Gabbana Pour Homme','Aromatic Fougere', 'Eau De Toilette', '75ml', 'Citruses, Bergamot, Neroli and Mandarin Orange', 'Lavender, Sage and Pepper', 'Tobacco, Tonka Bean and Cedar', 'Men', '', 'Liquid', '2012', 'Medium', 'France', 45.50, 5, 1, 'Dolce & Gabbana Cologne by Dolce & Gabbana, Walk into any room feeling confident and debonair wearing Dolce & Gabbana, a robust men’s fragrance. This enigmatic cologne blends spicy, citrus and floral accords for a powerful and seductive aroma that’s sure to gather interest from those around you', 2),
('Versace Eros Cologne','Aromatic Fougere', 'Eau De Parfum', '50ml', 'Mint, Candy Apple, Lemon, Mandarin Orange', 'Ambroxan, Geranium, Clary sage', 'Vanilla, Cedar, Sandalwood, Leather, Bitter Orange, Patchouli', 'Men', 'Alcohol Denat. (Sd Alcohol 39-C), Parfum (Fragrance), Aqua (Water), Limonene, Linalool, Coumarin, Alpha-Isometyl Ionone, Citral, Citronellol, Geraniol, Cinnamal, Eugenol.', 'Liquid', '2020', 'Medium', 'Italy', 54, 5, 1, 'Versace Eros cologne for men is a bold, fresh fragrance inspired by the mythological Greek god who was able to make people fall in love. Eros was a symbol of love and all that encompasses it', 1),
('Crystal Noir','Amber Floral', 'Eau De Toilette', '50ml', 'Pepper, Ginger and Cardamom', 'Coconut, Gardenia, Orange Blossom and Peony', 'Sandalwood, Musk and Amber', 'Women', '', 'Liquid', '2004', 'Medium', 'Italy', 43.25, 5, 1, 'Crystal Noir Perfume by Versace, A dreamy, creamy white floral oriental, Crystal Noir beautifully combines elements of warm spice, sensual blooms, blended woods and exotic undertones of velvety smooth coconut', 1),
('Versace Pour Homme','Aromatic Fougere', 'Eau De Toilette', '50ml', 'Lemon, Bergamot, Neroli and Rose de Mai', 'Hyacinth, Cedar, Clary Sage and Geranium', 'Tonka Bean, Musk and Amber', 'Women', '', 'Liquid', '2008', 'Medium', 'Italy', 34.76, 5, 1, 'Versace Pour Homme Cologne by Versace, Perfumer Albert Morillas introduced Versace Pour Homme in 2008 to great acclaim. Since its launch, it has remained a popular aromatic fougere for men', 1);

select * from Perfumes;

insert into Invoices(customer_ID, invoice_status) values
(1,1), (2,1), (1,1), (3,1), (4,1), (4,1), (4,1);

insert into InvoiceLineItems(invoice_ID, perfume_ID, unit_price, quantity) values
(1, 1, 39.73, 2), (1, 2, 46.56, 1),
(2, 3, 31.50, 1), (2, 1, 39.73, 1), (2, 2, 46.56, 1),
(3, 2, 46.56, 1),
(4, 8, 20.75, 1),
(5, 8, 20.75, 1),
(6, 3, 31.50, 1),
(7, 16, 49.73, 1); 



select invoice_ID, invoice_date, invoice_status, customer_name, customer_phone
                                    from Invoices 
                                    inner join Customers
                                    on Invoices.customer_ID = Customers.customer_ID;
						
select invoice_ID, InvoiceLineItems.perfume_ID, perfume_name, unit_price, quantity
from InvoiceLineItems
inner join Perfumes on InvoiceLineItems.perfume_ID = Perfumes.perfume_ID and invoice_ID = 1;

                                    
select customer_ID from Customers order by customer_ID desc limit 1;
                                    
select count(*) from Invoices;

create user if not exists 'pf15'@'localhost' identified by 'vtcacademy';
grant all on LoginDB.* to 'pf15'@'localhost';
