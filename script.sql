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
    name varchar(255) not null,
    NormalizedName varchar(255) not null default '',
    displayName varchar(255) not null default '',
    description varchar(255)
);
insert into Roles (id, name, NormalizedName, displayName, description) values (UUID(), "Admin", "ADMIN","Administrateur", "A les permissions pour géré une équipe");
insert into Roles (id, name, NormalizedName, displayName, description) values (UUID(), "SuperAdmin", "SUPERADMIN", "Super Administrateur", "A toutes les permissions pour géré l'ensemble des équipes.");

select * from users inner join userroles on userroles.userId = users.id inner join roles on roles.id = userroles.roleId;
select * from roles;

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
 
 INSERT INTO Users (id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, FirstName, LastName, Address, City, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES ("e0b2801d-f67c-11ee-a26a-00155dd4f30d", "aguay", "AGUAY", "alexis_raphael_guay@hotmail.com", "ALEXIS_RAPHAEL_GUAY@HOTMAIL.COM", true, "actual_password_hash", "alexis", "guay", "address", "quebec", "555-555-5555", true, false, '2024-05-05 00:00:00', false, 0);
 
 CREATE TABLE UserRoles (
    id varchar(255) PRIMARY KEY,
    userId varchar(255),
    roleId varchar(255)
);
 
 insert into UserRoles (id, userId, roleId) values(UUID(), "e0b2801d-f67c-11ee-a26a-00155dd4f30d", "2761adc6-fd92-11ee-89f0-00155dbc9e2a");
 insert into UserRoles (id, userId, roleId) values(UUID(), "e0b2801d-f67c-11ee-a26a-00155dd4f30d", "289bebd9-fd92-11ee-89f0-00155dbc9e2a");
 select * from roles;
 select * from teams;
 
 select * from calendars;
 
 

CREATE TABLE Availabilities (
    Id varchar(255) PRIMARY KEY,
    CalendarId varchar(255),
    DateFrom datetime NULL,
    DateTo datetime NULL,
	IsAllDay bool NULL default false,
    IsAvailable bool NULL default false,
	CONSTRAINT fk_calendars_CalendarId FOREIGN KEY (CalendarId) REFERENCES Calendars(id)
);

CREATE TABLE Animals (
    Id varchar(255) PRIMARY KEY,
    Name varchar(255),
    Species varchar(255),
    Breed varchar(255),
	Gender ENUM('Male', 'Female', 'Unknown'),
    WeightKg DECIMAL(5, 2),
    Birthdate datetime NULL,
	Notes TEXT,
	UserId varchar(255),
    MediaId varchar(255) NULL,
	CreatedAt datetime NULL,
	CONSTRAINT fk_animalusers_UserId FOREIGN KEY (UserId) REFERENCES Users(id),
    CONSTRAINT fk_animalmedias_MediaId FOREIGN KEY (MediaId) REFERENCES Medias(id)
);

select * from animals;

CREATE TABLE animalmedia (
    AnimalId varchar(255),
    MediaId varchar(255),
    PRIMARY KEY (AnimalId, MediaId ),
    FOREIGN KEY (AnimalId) REFERENCES Animals(Id) ON DELETE CASCADE,
    FOREIGN KEY (MediaId ) REFERENCES medias(Id) ON DELETE CASCADE
);

select * from medias;
select * from teammedias;
#SET SQL_SAFE_UPDATES = 1;
delete from medias where FileType = 'image/jpeg';

CREATE TABLE medias (
    Id varchar(255) PRIMARY KEY,
    FileName VARCHAR(255) NOT NULL,
    FileType VARCHAR(50),
    FileSize INT,
    FileData LONGBLOB,
    UploadedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE reservationmedia (
    ReservationId varchar(255),
    MediaId varchar(255),
    PRIMARY KEY (ReservationId, MediaId),
    FOREIGN KEY (ReservationId) REFERENCES reservations(Id) ON DELETE CASCADE,
    FOREIGN KEY (MediaId) REFERENCES medias(Id) ON DELETE CASCADE
);


CREATE TABLE teammedia (
    TeamId varchar(255),
    MediaId varchar(255),
    position INT null,
    PRIMARY KEY (TeamId, MediaId),
    FOREIGN KEY (TeamId) REFERENCES teams(Id) ON DELETE CASCADE,
    FOREIGN KEY (MediaId) REFERENCES medias(Id) ON DELETE CASCADE
);

ALTER TABLE teammedia ADD COLUMN position INT(11) null;

select * from teammedia;

CREATE TABLE usermedia (
    UserId varchar(255),
    MediaId varchar(255),
    PRIMARY KEY (UserId, MediaId ),
    FOREIGN KEY (UserId) REFERENCES users(Id) ON DELETE CASCADE,
    FOREIGN KEY (MediaId ) REFERENCES medias(Id) ON DELETE CASCADE
);

select * from medias;
select * from teammedia;


select * from reservationmedia;


select * from Availabilities;
select * from Users;

select * from calendars;
 
select * from Users;


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
select * from UserLogins;
select * from Roles;
select * from Reservations;

select * from teamUsers;

CREATE TABLE UserTokens (
    UserId varchar(255) not null,
    LoginProvider varchar(255) not null,
    Name varchar(255) not null, #name or type of the token
    TokenValue varchar(6000) not null, 
	PRIMARY KEY (UserId, LoginProvider, Name),
	CONSTRAINT fk_UserTokens_UserId FOREIGN KEY (UserId) REFERENCES Users(id)
);

select * from reservationmedia;

select * from teams;
delete from teams where id  = '08dc5fb2-12c8-47d3-8453-934b4b0fb586';

select * from users inner join TeamUsers on TeamUsers.userId = users.id where users.id = 'e0b2801d-f67c-11ee-a26a-00155dd4f30d';
#give possibility to switch team from user interface, being in multiple teams.




CREATE TABLE Teams (
    id varchar(255) PRIMARY KEY,
    Name varchar(255) not null default '',
    NormalizedName varchar(255) not null default '',
    CreatedAt datetime NULL,
    ApprovedAt datetime NULL
);

ALTER TABLE teams ADD COLUMN CreatedAt datetime null;

ALTER TABLE reservations MODIFY COLUMN Notes text null;

select * from reservations;
delete from reservations where calendarId = '2e731e68-f682-11ee-a26a-00155dd4f30d';

select * from teams;

select * from availabilities;

update Teams set NormalizedName = 'annieannick' where id = '2e731e68-f682-11ee-a26a-00155dd4f30d';
update Teams set NormalizedName = 'alexis' where id = '2efa6903-f682-11ee-a26a-00155dd4f30d';
update Teams set NormalizedName = 'anniel' where id = '2f68968c-f682-11ee-a26a-00155dd4f30d';

#SET SQL_SAFE_UPDATES = 1;
delete from teams where NormalizedName is null;
select * from teams;

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
select * from teams;



#ALTER TABLE users ADD CONSTRAINT fk_grade_id FOREIGN KEY (grade_id) REFERENCES grades(id);
CREATE TABLE Reservations (
    id varchar(255) PRIMARY KEY,
    dateFrom datetime,
    dateTo datetime,
    lodgerCount integer,
    userId varchar(255),
    calendarId varchar(255),
    notes text NULL,
	CreatedAt datetime NULL,
    ApprovedAt datetime NULL,
    CONSTRAINT fk_reservations_userId FOREIGN KEY (userId) REFERENCES users(id),
    CONSTRAINT fk_reservations_calendarId FOREIGN KEY (calendarId) REFERENCES calendars(id)
);
insert into reservations (id, dateFrom, dateTo, lodgerCount, userId, calendarId) values (UUID(), "2024-04-10", "2024-04-15", 3, "e0b2801d-f67c-11ee-a26a-00155dd4f30d", "2e731e68-f682-11ee-a26a-00155dd4f30d");
insert into reservations (id, dateFrom, dateTo, lodgerCount, userId, calendarId) values (UUID(), "2024-04-01", "2024-04-25", 5, "e0b2801d-f67c-11ee-a26a-00155dd4f30d", "2e731e68-f682-11ee-a26a-00155dd4f30d");
insert into reservations (id, dateFrom, dateTo, lodgerCount, userId, calendarId) values (UUID(), "2024-05-01", "2024-05-12", 1, "e0b2801d-f67c-11ee-a26a-00155dd4f30d", "2e731e68-f682-11ee-a26a-00155dd4f30d");
select * from calendars inner join reservations on reservations.calendarId = calendars.id where teamId = "2e731e68-f682-11ee-a26a-00155dd4f30d";
select * from teams where id = "2e731e68-f682-11ee-a26a-00155dd4f30d";

#SET SQL_SAFE_UPDATES = 0;
delete from reservations where id is not null;


select * from users where id = "e0b2801d-f67c-11ee-a26a-00155dd4f30d";



#SET SQL_SAFE_UPDATES = 1;
#UUID()
select * from reservations;

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

