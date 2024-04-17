use dogsitting;


DROP TABLE roles;
DROP TABLE userroles;
DROP TABLE teamusers;
DROP TABLE reservations;
DROP TABLE Calendars;
drop table teamusers;
DROP TABLE teams;
drop table userlogins;
drop table usertokens;
DROP TABLE USERS;


CREATE TABLE Roles (
    id varchar(255) PRIMARY KEY,
    name varchar(255),
    displayName varchar(255),
    description varchar(255)
);
insert into Roles (id, name, displayName, description) values (UUID(), "Admin", "Administrateur", "A les permissions pour géré une équipe");
insert into Roles (id, name, displayName, description) values (UUID(), "SuperAdmin", "Super Administrateur", "A toutes les permissions pour géré l'ensemble des équipes.");

CREATE TABLE Users (
    id varchar(255) PRIMARY KEY,
    UserName varchar(255) not null,
    NormalizedUserName varchar(255) NULL default "",
    Email varchar(255) not null default "",
    NormalizedEmail varchar(255) NOT NULL default "",
    EmailConfirmed bool not null default false,
    PasswordHash varchar(6000) NULL default NULL,
    FirstName varchar(255) NULL default "",
    LastName varchar(255) NULL default "",
    Address varchar(255) NULL default "",
    City varchar(255) NULL default "",
    PhoneNumber varchar(255) NULL default "",
    PhoneNumberConfirmed bool NULL default false,
    TwoFactorEnabled bool NULL default false,
    LockoutEnd datetime NULL,
    LockoutEnabled bool NULL default false,
    AccessFailedCount int default 0 not null
);

insert into Users (id, UserName, NormalizedUserName, EmailConfirmed, PasswordHash, FirstName, LastName, Address, city, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
 values ("e0b2801d-f67c-11ee-a26a-00155dd4f30d", "aguay", "AGUAY",  "alexis_raphael_guay@hotmail.com", true, "passwordhash", "alexis", "guay", "address", "quebec", "555-555-5555", true, false, "2024-05-05", false, 0);
 
select * from Users2;

CREATE TABLE UserLogins (
    LoginProvider varchar(255) not null,
    ProviderKey varchar(255) not null,
    ProviderDisplayName varchar(255),
    UserId varchar(255) not null,
	PRIMARY KEY (LoginProvider, ProviderKey),
	CONSTRAINT fk_UserLogins_UserId FOREIGN KEY (UserId) REFERENCES Users(id)
);
insert into UserLogins (LoginProvider, ProviderKey, ProviderDisplayName, UserId) values ("Facebook", "badkey", "Facebook", "e0b2801d-f67c-11ee-a26a-00155dd4f30d");

select * from Users;
select * from Users inner join UserLogins on userLogins.UserId = users.id;

CREATE TABLE UserTokens (
    UserId varchar(255) not null,
    LoginProvider varchar(255) not null,
    Name varchar(255) not null, #name or type of the token
    TokenValue varchar(6000) not null, 
	PRIMARY KEY (UserId, LoginProvider, Name),
	CONSTRAINT fk_UserTokens_UserId FOREIGN KEY (UserId) REFERENCES Users(id)
);


CREATE TABLE UserRoles (
    id varchar(255) PRIMARY KEY,
    userId varchar(255),
    roleId varchar(255)
);

select * from users inner join TeamUsers on TeamUsers.userId = users.id where users.id = 'e0b2801d-f67c-11ee-a26a-00155dd4f30d';
#give possibility to switch team from user interface, being in multiple teams.




CREATE TABLE Teams (
    id varchar(255) PRIMARY KEY,
    name varchar(255)
);

CREATE TABLE TeamUsers (
    id varchar(255) PRIMARY KEY,
    userId varchar(255),
    teamId varchar(255),
	CONSTRAINT fk_TeamUsers_userId FOREIGN KEY (userId) REFERENCES users(id),
    CONSTRAINT fk_TeamUsers_teamId FOREIGN KEY (teamId) REFERENCES teams(id)
);
insert into Teams(id, name) values ("2e731e68-f682-11ee-a26a-00155dd4f30d", "Annie & Annick");
insert into Teams(id, name) values ("2efa6903-f682-11ee-a26a-00155dd4f30d", "Alexis");
insert into Teams(id, name) values ("2f68968c-f682-11ee-a26a-00155dd4f30d", "Annie L.");
insert into teamusers (id, userId, teamId) values (UUID(), "e0b2801d-f67c-11ee-a26a-00155dd4f30d", "2e731e68-f682-11ee-a26a-00155dd4f30d");


CREATE TABLE Calendars (
    id varchar(255) PRIMARY KEY,
    teamId varchar(255),
    MaxWeekDaysLodgerCount int4 not null default(1),
    MaxWeekendDaysLodgerCount int4 not null default(1),
    UseAvailabilities boolean not null default(false),
    UseUnAvailabilities boolean not null default(false),
       CONSTRAINT fk_calendar_teamId FOREIGN KEY (teamId) REFERENCES teams(id)
);

insert into calendars (id, teamId, MaxWeekDaysLodgerCount, MaxWeekendDaysLodgerCount, UseAvailabilities, UseUnAvailabilities) values ("2e731e68-f682-11ee-a26a-00155dd4f30d", "2e731e68-f682-11ee-a26a-00155dd4f30d", 1, 1, false, false);

select * from calendars;



#ALTER TABLE users ADD CONSTRAINT fk_grade_id FOREIGN KEY (grade_id) REFERENCES grades(id);
CREATE TABLE Reservations (
    id varchar(255) PRIMARY KEY,
    dateFrom datetime,
    dateTo datetime,
    lodgerCount integer,
    userId varchar(255),
    calendarId varchar(255),
    CONSTRAINT fk_reservations_userId FOREIGN KEY (userId) REFERENCES users(id),
    CONSTRAINT fk_reservations_calendarId FOREIGN KEY (calendarId) REFERENCES calendars(id)
);
insert into reservations (id, dateFrom, dateTo, lodgerCount, userId, calendarId) values (UUID(), "2024-04-10", "2024-04-15", 3, "e0b2801d-f67c-11ee-a26a-00155dd4f39d", "2e731e68-f682-11ee-a26a-00155dd4f30d");
insert into reservations (id, dateFrom, dateTo, lodgerCount, userId, calendarId) values (UUID(), "2024-04-01", "2024-04-25", 5, "e0b2801d-f67c-11ee-a26a-00155dd4f39d", "2e731e68-f682-11ee-a26a-00155dd4f30d");
insert into reservations (id, dateFrom, dateTo, lodgerCount, userId, calendarId) values (UUID(), "2024-05-01", "2024-05-12", 1, "e0b2801d-f67c-11ee-a26a-00155dd4f39d", "2e731e68-f682-11ee-a26a-00155dd4f30d");
select * from calendars inner join reservations on reservations.calendarId = calendars.id where teamId = "2e731e68-f682-11ee-a26a-00155dd4f30d";
#SET SQL_SAFE_UPDATES = 0;
delete from reservations where id is not null;
#SET SQL_SAFE_UPDATES = 1;
#UUID()


CREATE TABLE LoginProvider (
    id varchar(255) PRIMARY KEY,
    dateFrom datetime,
    dateTo datetime,
    lodgerCount integer,
    userId varchar(255),
    calendarId varchar(255),
    CONSTRAINT fk_reservations_userId FOREIGN KEY (userId) REFERENCES users(id),
    CONSTRAINT fk_reservations_calendarId FOREIGN KEY (calendarId) REFERENCES calendars(id)
);



select * from users;
select * from teams;
select * from teamusers;
select * from reservations;
select * from calendars;
select * from reservations where teamId = "2e731e68-f682-11ee-a26a-00155dd4f30d";

