use dogsitting;


DROP TABLE roles;
DROP TABLE userroles;
DROP TABLE teamusers;
DROP TABLE reservations;
DROP TABLE Calendars;
DROP TABLE teams;
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
    firstName varchar(255),
    lastName varchar(255),
    email varchar(255),
    address varchar(255),
    city varchar(255),
    phone varchar(255)
);
insert into Users (id, firstname, lastname, address, city, phone) values ("8825c601-f5c0-11ee-a26a-00155dd4f30d", "firstname", "lastname", "address", "quebec", "555-555-5555");
insert into Users (id, firstname, lastname, email, address, city, phone) values ("e0b2801d-f67c-11ee-a26a-00155dd4f30d", "alexis", "guay", "alexis_raphael_guay@hotmail.com", "address", "quebec", "555-555-5555");
insert into Users (id, firstname, lastname, address, city, phone) values ("e0b2801d-f67c-11ee-a26a-00155dd4f39d", "clientA", "clientA", "address", "quebec", "555-555-5555");
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



select * from users;
select * from teams;
select * from teamusers;
select * from reservations;
select * from calendars;
select * from reservations where teamId = "2e731e68-f682-11ee-a26a-00155dd4f30d";

