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
    customer_phone char(10),
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
    top_notes varchar(50),
    heart_notes varchar(50),
    base_notes varchar(50),
    gender varchar(10) check (gender in ('Men', 'Women', 'Unisex')),
    ingredients varchar(500) not null,
    form varchar(10),
    year_launched char(4),
    strength varchar(10),
    origin varchar(15),
    price decimal(20,2) default 0,
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




create user if not exists 'pf15'@'localhost' identified by 'vtcacademy';
grant all on LoginDB.* to 'pf15'@'localhost';

insert into Cashiers(cashier_name, user_name, user_pass) values
('son', 'sondoannam', '48a3ba2f7c991b6fa4fdfea00fb4f87c'), -- 20052002Zz
('hoang', 'lehuyhoang', '1dd2fe3029eb4e02c5057e1a289db77f'); -- PF15VTCAcademy
select * from Cashiers;

insert into Customers(customer_name, customer_phone) values
('Nguyen Thanh Tung', '0975479078'),
('Thieu Bao Tram', '0374553099');
select * from Customers;

insert into Brands(brand_ID, brand_name, brand_description) values
(1, 'Versace', 'Gianni Versace S.r.l., usually referred to simply as Versace, is an Italian luxury fashion company and trade name founded by Gianni Versace in 1978');
select * from Brands;

insert into Perfumes(perfume_name, fragrance_family, classification, volume, top_notes, heart_notes, base_notes, gender, ingredients, form, year_launched, strength, origin, price, total_quantity, product_status, perfume_description, brand_ID)
values
('Bright Crystal','Floral, Fruity and Musk', 'Eau De Toilette', '50ml', 'Yuzu, Pomegranate, Water Notes', 'Peory, Lotus, Magnolia', 'Musk, Mahogany, Amber', 'Women', 'Alcohol, Denat. (Sd Alcohol 39-C), Fragrance, Water, Butylphenyl, Methylpropional, Ethylhexyl Methoxycinnamate, Hydroxysohexyl 3-Cyclohexene Carboxaldehyde, Linalool, Citronellol, Ethylhexyl Salicylate, Butyl, Methoxydibenzoylmethane, Limonene, Geraniol, Ci 17200 (Red 33), Ci 15985 (Yellow 6)', 'Liquid', '2006', 'Medium', 'Italy', 39.73, 5, 1, 'Designed and created by the master perfumer Alberto Morillas for the highly beloved Italian designer label Versace. Inspired by the favorite floral mixes by the fashion-forward Donatella Versace herself, this is the scent that perfectly captures the essence of summer. Whether you are looking for a fragrance that reminds you of your favorite season or celebrating it, this was made for you', 1);
select * from Perfumes;

