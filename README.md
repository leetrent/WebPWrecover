# Purpose
To learn how to build an ASP.NET Core app with email confirmation and password reset that uses ASP.NET Core Identity, ASP.NET Razor Pages, Entity Framework Core and a MySQL database that can run on either localhost or on CLOUD.GOV.

# Microsoft Tutorial Used
```https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?tabs=aspnetcore2x```

# Create Tables (MySQL)
## (localhost only)
```
CREATE DATABASE <insert database name here>;
USE <insert database name here>; 
```
## (localhost and cloud.gov)
```
CREATE TABLE `AspNetUsers` (
  `Id` varchar(256) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `ConcurrencyStamp` text,
  `Email` text,
  `EmailConfirmed` int(11) NOT NULL,
  `LockoutEnabled` int(11) NOT NULL,
  `LockoutEnd` text,
  `NormalizedEmail` text,
  `NormalizedUserName` text,
  `PasswordHash` text,
  `PhoneNumber` text,
  `PhoneNumberConfirmed` int(11) NOT NULL,
  `SecurityStamp` text,
  `TwoFactorEnabled` int(11) NOT NULL,
  `UserName` text,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `AspNetRoles` (
  `Id` varchar(256) NOT NULL,
  `ConcurrencyStamp` text,
  `Name` text,
  `NormalizedName` text,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `AspNetUserRoles` (
  `UserId` varchar(256) NOT NULL,
  `RoleId` varchar(256) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `FK_AspNetUserRoles_AspNetRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE `AspNetUserClaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ClaimType` text,
  `ClaimValue` text,
  `UserId` varchar(256) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_AspNetUserClaims_AspNetUsers_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
```
# Setting Environment Variables
## localhost only (on Mac or Linux)
```
export LOCAL_CONNECTION_STRING="Username=<insert username here>;Password=<insert password here>;Host=localhost;Port=3306;Database=<insert database (schema) name here;Pooling=true;"
```
# Using cloud.gov
Learning how to use cloud.gov is a bit involved and beyond the scope of this README file. I recommend starting here: 
1. ```https://cloud.gov/quickstart/```
2. ```https://cloud.gov/docs/services/relational-database/```
