CREATE TABLE INVOICES
(
	ID VARCHAR(32) PRIMARY KEY,
	CONTENT NVARCHAR(MAX),
	ORIGINALCONTENT NVARCHAR(MAX),
	DOWNLOADED INTEGER DEFAULT 0,
	DHEMI Datetime
)

CREATE TABLE PRODUCTS
(
	CDPROD VARCHAR(36) PRIMARY KEY,
	DESCRIPTION VARCHAR(50),
	BRAND VARCHAR(50)
)

CREATE TABLE SALES
(
	ID VARCHAR(36) PRIMARY KEY,
	IDINV VARCHAR(32),
	CDPROD VARCHAR(36),
	PRICE DECIMAL(13,4),
	UCOM VARCHAR(5),
	VUNCOM DECIMAL(13,4),
	VPROD DECIMAL(13,4),
	UTRIB VARCHAR(5),
	QTRIB DECIMAL(13,4),
	VUNTRIB DECIMAL(13,4),
	TAX DECIMAL(13,4),
	DTEMIT Datetime,
	FOREIGN KEY (CDPROD) REFERENCES PRODUCTS (CDPROD),
	FOREIGN KEY (IDINV) REFERENCES INVOICES (ID)
)
