CREATE TABLE TSHUsers (
  UserId        VARCHAR(10)   PRIMARY KEY,
  UserPw        VARBINARY(50) NOT NULL,
  FullName      VARCHAR(50)   NOT NULL,
  Email         VARCHAR(50)   NOT NULL,
  UserRole      VARCHAR(10)   NOT NULL,
  LastLogin     DATETIME      NULL
);

INSERT INTO TSHUsers (UserId, UserPw, FullName, Email, UserRole) VALUES 
('supplier', HASHBYTES('SHA1', 'password0'), 'supplier', 'supplier@tsh.com',  'manager'),
('purchaser', HASHBYTES('SHA1', 'password1'), 'purchaser', 'purchaser@tsh.com',  'member');